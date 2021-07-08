using System;
using System.Reflection;
using Microsoft.Win32;

namespace intag
{
	public static class RegUtils
	{
		public static void Install()
		{
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
			subKey.SetValue("", loc+" %1");
			subKey.Close();  
		}

		public static void Uninstall()
		{
			Registry.ClassesRoot.DeleteSubKeyTree($@"Folder\shell\{Constants.RegistryName}", false);
		}
	}
}