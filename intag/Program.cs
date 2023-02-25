using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace intag
{
	internal static class Program
	{
		private const string BatchFilename = "batch.intag";
		private const int DefaultLaunchDelay = 300;
		private static readonly Mutex Mtx = new();


		[Conditional("LOGGING")]
		private static void Log(string message)
		{
			var fi = Path.Combine(new FileInfo(Application.ExecutablePath).Directory.FullName, "intag.log");
			File.AppendAllText( fi, $"{DateTime.Now.ToString("O")}\t{message}{Environment.NewLine}");
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
		private static void Main(string[] args)
		{
			var held = false;
			try
			{
				if (args == null || args.Length == 0)
				{

					RegUtils.Install();
					Environment.Exit(0);
				}
				if (args.Length == 1 && (args[0].Equals("--uninstall", StringComparison.CurrentCultureIgnoreCase) ||
										 args[0].Equals("-u", StringComparison.CurrentCultureIgnoreCase)))
				{
					RegUtils.Uninstall();
					Environment.Exit(0);
				}
				if (!args.Any(arg => Directory.Exists(arg) || File.Exists(arg)))
				{
					Environment.Exit(1);
				}

				var delay = args.Length > 1 ? int.Parse(args.FirstOrDefault(arg => !Directory.Exists(arg) && !File.Exists(arg)) ?? DefaultLaunchDelay.ToString()) : DefaultLaunchDelay;
				var validObjects = args.Where(arg => Directory.Exists(arg) || File.Exists(arg)).ToArray();
				
				//Batching
				Mtx.WaitOne();
				held = true;
				if (File.Exists(BatchFilename))
				{
					File.AppendAllText(BatchFilename, Environment.NewLine + validObjects[0]);
					Mtx.ReleaseMutex();
					held = false;
					return;
				}
				File.WriteAllText(BatchFilename, validObjects[0]);
				Mtx.ReleaseMutex();
				held = false;
				Thread.Sleep(delay);
				var batch = File.ReadAllLines(BatchFilename);
				File.Delete(BatchFilename);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Log($"Launching with batch: [{string.Join(", ", batch)}]");
				Application.Run(new MainForm(batch));
			}
			catch (Exception e)
			{

				Log($"Caught exception:\n{e}");
				throw;
				//Ignored for now
			}
			finally
			{
				if (held) Mtx.ReleaseMutex();
			}
		}
	}
}