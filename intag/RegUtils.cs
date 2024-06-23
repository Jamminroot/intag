using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace intag
{
    public static class RegUtils
    {
        private static void InstallExtension(string ext)
        {
            var subKey = Registry.ClassesRoot.CreateSubKey($@"{ext}\shell\{Constants.RegistryName}\command")
                ?? throw new ApplicationException("Cannot create shell registry key");
            var subKeyIcon = Registry.ClassesRoot.OpenSubKey($@"{ext}\shell\{Constants.RegistryName}", true)
                ?? throw new ApplicationException("Cannot open shell registry key");

            var loc = Application.ExecutablePath;
            if (loc == null)
            {
                subKey.Close();
                subKeyIcon.Close();

                throw new ApplicationException("Cannot get assembly's location");
            }

            loc = $"\"{loc}\"";

            subKeyIcon.SetValue("Icon", loc);
            subKeyIcon.SetValue("MultiSelectModel", "Player");
            subKeyIcon.Close();
            subKey.SetValue("", loc+" --ui --path \"%1\"");
            subKey.Close();
        }

        private static bool TryGetSystemColor(string colorName, out System.Drawing.Color color)
        {
            color = System.Drawing.Color.Empty;
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM");
            if (key == null)
            {
                return false;
            }
            
            color = System.Drawing.Color.FromArgb((int)key.GetValue(colorName));
            return true;
        }
        
        public static bool TryGetSystemAccentColor(out System.Drawing.Color color)
        {
            return TryGetSystemColor("AccentColor", out color);
        }
        
        public static bool TryGetSystemColorizationColor(out System.Drawing.Color color)
        {
            return TryGetSystemColor("ColorizationColor", out color);
        }
        
        private static void InstallFiles() => InstallExtension("*");
        private static void InstallFolder() => InstallExtension("Folder");

        public static void Install()
        {
            InstallFolder();
            InstallFiles();
        }

        private static void UninstallExt(string ext)
        {
            Registry.ClassesRoot.DeleteSubKeyTree($@"{ext}\shell\{Constants.RegistryName}", false);
        }

        private static void UninstallFolder() => UninstallExt("Folder");
        private static void UninstallFiles() => UninstallExt("*");

        public static void Uninstall()
        {
            UninstallFolder();
            UninstallFiles();
        }
    }
}