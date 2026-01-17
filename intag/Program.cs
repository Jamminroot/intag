using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace intag
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    internal static class Program
    {
        private const int MMF_MAX_SIZE = 4096; // 4KB
        private const int DefaultLaunchDelay = 300;

        [Conditional("DEBUG")]
        private static void Log(string message)
        {
            Debug.WriteLine(message);
            Console.WriteLine(message);
            var fi = Path.Combine(new FileInfo(Application.ExecutablePath).Directory.FullName, "intag.log");
            var mtx = new Mutex(false, "InTagLogMutex");
            mtx.WaitOne();
            File.AppendAllText(fi, $"{DateTime.Now:O}\t{message}{Environment.NewLine}");
            mtx.ReleaseMutex();
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  intag --add <value> [--add <value> ...] --remove <value> [--remove <value> ...] --path <file|folder> [--path <file|folder> ...] --list <file>");
            Console.WriteLine("  intag --property <name> --add <value> --path <file>");
            Console.WriteLine("  intag --path <file|folder> [--path <file|folder> ...] --list <file>");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --property <name>  Property to modify: tags (default), title, subject, author, comments");
            Console.WriteLine("  --add <value>      Add value (for tags) or set value (for other properties)");
            Console.WriteLine("  --remove <value>   Remove value (only for tags)");
            Console.WriteLine("  --path <path>      File or folder to modify");
            Console.WriteLine("  --list <file>      File containing list of paths");
            Console.WriteLine("  --ui               Open the graphical interface");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  intag --add tag1 --add tag2 --remove tag3 --path file1.txt --path folder1");
            Console.WriteLine("  intag --property author --add \"John Doe\" --path document.docx");
            Console.WriteLine("  intag --property title --add \"My Document\" --path file.txt");
            Console.WriteLine("  intag --path file1.txt --list filelist.txt");
        }
        
        private static void HandleExplorerRunNoArgs()
        {
            if (UACHelper.UACHelper.IsElevated)
            {
                RegUtils.Install();
                MessageBox.Show("InTag is installed.", "InTag", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                UACHelper.UACHelper.StartElevated(new ProcessStartInfo(Application.ExecutablePath)
                {
                    UseShellExecute = true
                });
            }
        }
        
        private static Dictionary<string, List<string>> ParseArgs(string[] args)
        {
            var result = new Dictionary<string, List<string>>();
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    var key = args[i];
                    if (!result.ContainsKey(key))
                        result[key] = new List<string>();

                    if (i + 1 >= args.Length || args[i + 1].StartsWith("--"))
                    {
                        // Flag without value
                        continue;
                    }
                    var value = args[++i];
                    result[key].Add(value);
                }
            }

            return result;
        }
        
        
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                var parent = ParentProcessUtilities.GetParentProcess()?.ProcessName ?? "explorer"; // Workaround for elevation
                Log("Running from parent: " + parent);
                if (parent != "explorer")
                {
                    ConsoleWindow.Allocate();
                }
                if (args == null || args.Length == 0)
                {
                    if (parent.Equals("explorer", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Log("Goging to install");
                        HandleExplorerRunNoArgs();
                    }
                    else
                    {
                        Log("Showing usage");
                        ShowUsage();
                    }
                    Environment.Exit(0);
                }

                if (args!.Length == 1 && (args[0].Equals("--uninstall", StringComparison.CurrentCultureIgnoreCase) ||
                                          args[0].Equals("-u", StringComparison.CurrentCultureIgnoreCase)))
                {
                    HandleIntagUninstallArg();
                    Environment.Exit(0);
                }

                var parsedArgs = ParseArgs(args);
                var isWithModificationFlags = parsedArgs.ContainsKey("--add") || parsedArgs.ContainsKey("--remove");
                var isObjectsNotSpecified = !parsedArgs.ContainsKey("--path") && !parsedArgs.ContainsKey("--list");
                if (isWithModificationFlags && isObjectsNotSpecified && !parsedArgs.ContainsKey("--ui"))
                {
                    ShowUsage();
                    Environment.Exit(1);
                }

                var objects = parsedArgs.TryGetValue("--path", out var paths) ? paths : new List<string>();
                var listFiles = parsedArgs.TryGetValue("--list", out var lists) ? lists : new List<string>();
                var joinedList = listFiles.SelectMany(File.ReadAllLines).ToList();
                joinedList.AddRange(objects);

                var validObjects = joinedList.Where(arg => Directory.Exists(arg) || File.Exists(arg)).ToArray();

                if (parsedArgs.ContainsKey("--ui"))
                {
                    HandleUiStart(validObjects);
                }
                else
                {
                    HandleCliStart(parsedArgs, validObjects);
                }
            }
            catch (Win32Exception e) when (e.NativeErrorCode == 1223)
            {
                // User has canceled the elevation
            }
            catch (Exception e)
            {
                Log($"Caught exception:\n{e}");
                MessageBox.Show(e.Message, "InTag", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool HandleCliStart(Dictionary<string, List<string>> parsedArgs, string[] validObjects)
        {
            var valuesToAdd = parsedArgs.TryGetValue("--add", out var addList) ? addList.ToArray() : Array.Empty<string>();
            var valuesToRemove = parsedArgs.TryGetValue("--remove", out var removeList) ? removeList.ToArray() : Array.Empty<string>();
            var propertyName = parsedArgs.TryGetValue("--property", out var propList) && propList.Count > 0
                ? propList[0]
                : "tags";  // default to tags/keywords

            var propertyId = Constants.GetPropertyId(propertyName);
            var isMultiValue = Constants.IsMultiValueProperty(propertyId);

            if (valuesToAdd.Length == 0 && valuesToRemove.Length == 0)
            {
                // Display current values
                if (isMultiValue)
                {
                    var tagsCollection = FileUtils.GetObjectsTags(validObjects);
                    Console.WriteLine($"{Constants.PropertyIdToDisplayName[propertyId]} for objects:");
                    Console.WriteLine(string.Join(Environment.NewLine, tagsCollection.Select(kv => $"{kv.Key}: {string.Join(", ", kv.Value)}")));
                }
                else
                {
                    Console.WriteLine($"{Constants.PropertyIdToDisplayName[propertyId]} for objects:");
                    foreach (var obj in validObjects)
                    {
                        if (File.Exists(obj))
                        {
                            var value = FileUtils.GetFileStringProperty(obj, propertyId);
                            Console.WriteLine($"{obj}: {value}");
                        }
                        else
                        {
                            Console.WriteLine($"{obj}: (folders not supported for this property)");
                        }
                    }
                }
                return false;
            }

            if (isMultiValue)
            {
                // Multi-value property (tags/keywords) - add/remove behavior
                if (valuesToAdd.Length > 0)
                {
                    Log($"Adding {propertyName}: " + string.Join(", ", valuesToAdd));
                    FileUtils.AddTags(validObjects, valuesToAdd);
                }

                if (valuesToRemove.Length > 0)
                {
                    Log($"Removing {propertyName}: " + string.Join(", ", valuesToRemove));
                    FileUtils.RemoveTags(validObjects, valuesToRemove);
                }
            }
            else
            {
                // Single-value property - set behavior (--add sets, --remove clears)
                if (valuesToAdd.Length > 0)
                {
                    var valueToSet = string.Join(" ", valuesToAdd);  // Combine if multiple --add
                    Log($"Setting {propertyName} to: {valueToSet}");
                    foreach (var obj in validObjects)
                    {
                        if (File.Exists(obj))
                        {
                            FileUtils.AssignStringPropertyToFile(obj, propertyId, valueToSet);
                        }
                        else
                        {
                            Console.WriteLine($"Warning: {obj} is a folder, property '{propertyName}' not supported for folders");
                        }
                    }
                }

                if (valuesToRemove.Length > 0)
                {
                    Log($"Clearing {propertyName}");
                    foreach (var obj in validObjects)
                    {
                        if (File.Exists(obj))
                        {
                            FileUtils.AssignStringPropertyToFile(obj, propertyId, string.Empty);
                        }
                    }
                }
            }

            return true;
        }
        
        private static bool HandleUiStart(string[] validObjects)
        {
            var delay =  DefaultLaunchDelay;

            if (validObjects.Length == 0)
            {
                MessageBox.Show("No existing files or folders are passed in a command line.", "InTag", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(1);
            }

            using var mtx = new Mutex(false, "InTagMutex", out var createdNew);
            using var mmf = MemoryMappedFile.CreateOrOpen("InTagMemoryMap", MMF_MAX_SIZE + 4);

            mtx.WaitOne();

            using (var accessor = mmf.CreateViewAccessor())
            {
                var tempArray = new byte[MMF_MAX_SIZE];
                accessor.ReadArray(0, tempArray, 0, MMF_MAX_SIZE);
                var existingData = Encoding.UTF8.GetString(tempArray).TrimEnd('\0');
                var newData = string.IsNullOrEmpty(existingData) ? validObjects[0] : existingData + Environment.NewLine + validObjects[0];
                var bytes = Encoding.UTF8.GetBytes(newData);
                accessor.WriteArray(0, bytes, 0, bytes.Length);
            }

            mtx.ReleaseMutex();
            var ourName = Process.GetCurrentProcess().ProcessName;

            if (createdNew)
            {
                Log("Launched new instance with valid objects: " + string.Join(", ", validObjects));
                Thread.Sleep(delay);
                var start = DateTime.Now;
                while (Process.GetProcessesByName(ourName).Length > 1 && DateTime.Now - start < TimeSpan.FromSeconds(5))
                {
                    Thread.Sleep(100);
                }
            }
            else
            {
                Log("Appended valid objects to existing instance: " + string.Join(", ", validObjects));
                return true;
            }

            string batchData;
            using (var accessor = mmf.CreateViewAccessor())
            {
                var tempArray = new byte[MMF_MAX_SIZE];
                accessor.ReadArray(0, tempArray, 0, MMF_MAX_SIZE);
                batchData = Encoding.UTF8.GetString(tempArray).TrimEnd('\0');
            }

            Log($"Launching with batch: [{batchData}]");

            // Clear MMF
            using (var accessor = mmf.CreateViewAccessor())
            {
                var emptyBytes = new byte[MMF_MAX_SIZE];
                accessor.WriteArray(0, emptyBytes, 0, MMF_MAX_SIZE);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(batchData.Split(new[] { Environment.NewLine }, StringSplitOptions.None)));
            return false;
        }

        private static void HandleIntagUninstallArg()
        {
            if (UACHelper.UACHelper.IsElevated)
            {
                RegUtils.Uninstall();
                MessageBox.Show("InTag is uninstalled.", "InTag", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                UACHelper.UACHelper.StartElevated(new ProcessStartInfo(Application.ExecutablePath, "-u")
                {
                    UseShellExecute = true
                });
            }
        }
    }
}
