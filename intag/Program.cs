using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace intag
{
	internal static class Program
	{
		private const string BatchFilename = "batch.intag";
		private static readonly Mutex Mtx = new();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
		private static void Main(string[] args)
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

			var delay = args.Length > 1 ? int.Parse(args.FirstOrDefault(arg => !Directory.Exists(arg) || !File.Exists(arg)) ?? "300") : 300;
			var validObjects = args.Where(arg => Directory.Exists(arg) || File.Exists(arg)).ToArray();
			var held = false;
			 
			//Batching
			try
			{
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
				Application.Run(new MainForm(batch));
			}
			catch (Exception e)
			{
				//Ignored for now
			}
			finally
			{
				if (held) Mtx.ReleaseMutex();
			}
		}
	}
}