using System;
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
            var fi = Path.Combine(new FileInfo(Application.ExecutablePath).Directory.FullName, "intag.log");
            File.AppendAllText( fi, $"{DateTime.Now:O}\t{message}{Environment.NewLine}");
        }

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
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
                    Environment.Exit(0);
                }
                if (args.Length == 1 && (args[0].Equals("--uninstall", StringComparison.CurrentCultureIgnoreCase) ||
                                         args[0].Equals("-u", StringComparison.CurrentCultureIgnoreCase)))
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
                    Environment.Exit(0);
                }

                var validObjects = args.Where(arg => Directory.Exists(arg) || File.Exists(arg)).ToArray();
                var delay = args.Length > 1 ? int.Parse(args.Except(validObjects).FirstOrDefault() ?? DefaultLaunchDelay.ToString()) : DefaultLaunchDelay;
                
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
                    return;
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
    }
}
