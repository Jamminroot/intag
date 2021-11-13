using System;
using System.Reflection;
using Microsoft.Win32;

namespace intag
{
	public static class RegUtils
	{
		public static void Install()
		{
			var subKeyIcon = Registry.ClassesRoot.CreateSubKey($@"Folder\shell\{Constants.RegistryName}");
			if (subKeyIcon == null)
			{
				return;
			}
			
			var subKey = Registry.ClassesRoot.CreateSubKey($@"Folder\shell\{Constants.RegistryName}\command");
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

		public static void Uninstall()
		{
			Registry.ClassesRoot.DeleteSubKeyTree($@"Folder\shell\{Constants.RegistryName}", false);
		}
	}
}