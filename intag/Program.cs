using System;
using System.IO;
using System.Windows.Forms;

namespace intag
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                RegUtils.Install();
                Environment.Exit(0);
            }
            if (args.Length == 1 && (args[0].Equals("--uninstall", StringComparison.CurrentCultureIgnoreCase) || args[0].Equals("-u", StringComparison.CurrentCultureIgnoreCase)))
            {
                RegUtils.Uninstall();
                Environment.Exit(0);
            }
            if (!Directory.Exists(args[0]) && !File.Exists(args[0]))
            {
                Environment.Exit(1);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args[0]));
        }
    }
}
