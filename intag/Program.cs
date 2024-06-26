using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
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
            File.AppendAllText( fi, $"{DateTime.Now:O}\t{message}{Environment.NewLine}");
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  intag --add <tag> [--add <tag> ...] --remove <tag> [--remove <tag> ...] --path <file|folder> [--path <file|folder> ...] --list <file>");
            Console.WriteLine("  intag --path <file|folder> [--path <file|folder> ...] --list <file>");
            Console.WriteLine("Examples:");
            Console.WriteLine("  intag --add tag1 --add tag2 --remove tag3 --path file1.txt --path folder1");
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
        
        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            var result = new Dictionary<string, string>();
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    var key = args[i];
                    if (i + 1 >= args.Length || args[i + 1].StartsWith("--"))
                    {
                        result[key] = string.Empty;
                        continue;
                    }
                    var value = args[++i];
                    result[key] = value;
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
                var isWithTagsModificationFlags = parsedArgs.ContainsKey("--add") || parsedArgs.ContainsKey("--remove");
                var isObjectsNotSpecified = !parsedArgs.ContainsKey("--path") && !parsedArgs.ContainsKey("--list");
                if (isWithTagsModificationFlags && isObjectsNotSpecified && !parsedArgs.ContainsKey("--ui"))
                {
                    ShowUsage();
                    Environment.Exit(1);
                }

                var objects = parsedArgs.Where(p => p.Key == "--path").Select(p => p.Value);
                var joinedList = parsedArgs.Where(p => p.Key == "--list").Select(p => p.Value).SelectMany(File.ReadAllLines).ToList();
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

        private static bool HandleCliStart(Dictionary<string, string> parsedArgs, string[] validObjects)
        {
            var tagsToAdd = parsedArgs.Where(p => p.Key == "--add").Select(p => p.Value).ToArray();
            var tagsToRemove = parsedArgs.Where(p => p.Key == "--remove").Select(p => p.Value).ToArray();
          
            if (tagsToAdd.Length == 0 && tagsToRemove.Length == 0)
            {
                var tagsCollection = FileUtils.GetObjectsTags(validObjects);
                System.Console.WriteLine("Tags for objects:");
                System.Console.WriteLine(string.Join(Environment.NewLine, tagsCollection.Select(kv => $"{kv.Key}: {string.Join(", ", kv.Value)}")));
                return false;
            }
            
            if (tagsToAdd.Length > 0)
            {
                Log("Adding tags: " + string.Join(", ", tagsToAdd));
                FileUtils.AddTags(validObjects, tagsToAdd);
            }

            if (tagsToRemove.Length > 0)
            {
                Log("Removing tags: " + string.Join(", ", tagsToRemove));
                FileUtils.RemoveTags(validObjects, tagsToRemove);
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
