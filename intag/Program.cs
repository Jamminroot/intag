using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace intag
{
    internal static class Program
    {
        private const string BatchFilename = "batch.intag";
        private const int DefaultLaunchDelay = 300;

        [Conditional("LOGGING")]
        private static void Log(string message)
        {
            var fi = Path.Combine(new FileInfo(Application.ExecutablePath).Directory.FullName, "intag.log");
            File.AppendAllText( fi, $"{DateTime.Now:O}\t{message}{Environment.NewLine}");
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (args == null || args.Length == 0)
                {
                    if (UACHelper.UACHelper.IsElevated)
                    {
                        RegUtils.Install();
                        MessageBox.Show("InTag is installed.", "InTag", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UACHelper.UACHelper.StartElevated(new ProcessStartInfo(Assembly.GetExecutingAssembly().Location));
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
                        UACHelper.UACHelper.StartElevated(new ProcessStartInfo(Assembly.GetExecutingAssembly().Location, "-u"));
                    }
                    Environment.Exit(0);
                }

                var validObjects = args.Where(arg => Directory.Exists(arg) || File.Exists(arg)).ToArray();

                if (validObjects.Length == 0)
                {
                    MessageBox.Show("No existing files or folders are passed in a command line.", "InTag", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Environment.Exit(1);
                }

                var delay = args.Length > 1 ? int.Parse(args.Except(validObjects).FirstOrDefault() ?? DefaultLaunchDelay.ToString()) : DefaultLaunchDelay;

                using var mtx = new Mutex(false, "InTagMutex");
                //Batching
                mtx.WaitOne();

                if (File.Exists(BatchFilename))
                {
                    File.AppendAllText(BatchFilename, Environment.NewLine + validObjects[0]);
                    mtx.ReleaseMutex();
                    return;
                }
                File.WriteAllText(BatchFilename, validObjects[0]);
                mtx.ReleaseMutex();

                var ourName = Process.GetCurrentProcess().ProcessName;
                var start = DateTime.Now;
                var emergencyExit = false;
                do
                {
                    Thread.Sleep(delay);
                    if (DateTime.Now - start > TimeSpan.FromSeconds(5))
                    {
                        switch(MessageBox.Show("Timeout for waiting of another InTag process.", "InTag", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning))
                        {
                            case DialogResult.Abort:
                                if (File.Exists(BatchFilename)) File.Delete(BatchFilename);
                                return;
                            case DialogResult.Retry:
                                start = DateTime.Now;
                                break;
                            case DialogResult.Ignore:
                                emergencyExit = true;
                                break;
                        }
                    }
                } while(Process.GetProcessesByName(ourName).Length > 1 || emergencyExit);

                var batch = File.ReadAllLines(BatchFilename);
                File.Delete(BatchFilename);
                Log($"Launching with batch: [{string.Join(", ", batch)}]");
                Application.Run(new MainForm(batch));
            }
            catch(Win32Exception e) when (e.NativeErrorCode == 1223)
            {
                // user has canceled the elevation
            }
            catch (Exception e)
            {
                Log($"Caught exception:\n{e}");
                MessageBox.Show(e.Message, "InTag", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}