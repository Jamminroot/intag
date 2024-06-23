
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace intag
{
    static class ConsoleWindow
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        private const int ATTACH_PARENT_PROCESS = -1;

        public static void Allocate()
        {
            if (!AttachConsole(ATTACH_PARENT_PROCESS))
            {
                AllocConsole();
            }

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetError(new StreamWriter(Console.OpenStandardError()) { AutoFlush = true });
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        }
    }
}