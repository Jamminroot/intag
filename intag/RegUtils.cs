using System;
using System.Reflection;
using Microsoft.Win32;

namespace intag
{
	public static class RegUtils
	{
		private static void InstallExtension(string ext)
		{
			var subKeyIcon = Registry.ClassesRoot.CreateSubKey($@"{ext}\shell\{Constants.RegistryName}");
			if (subKeyIcon == null)
			{
				return;
			}
			
			var subKey = Registry.ClassesRoot.CreateSubKey($@"{ext}\shell\{Constants.RegistryName}\command");
			if (subKey == null)
			{
				return;
			}
			
			var loc = Assembly.GetEntryAssembly()?.Location;
			if (loc == null)
			{
				subKey.Close();
				Environment.Exit(1);
			}

			loc = $"\"{loc}\"";
			
			subKeyIcon.SetValue("Icon", loc);
			subKeyIcon.Close();
			subKey.SetValue("", loc+" \"%1\"");
			subKey.Close();  
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