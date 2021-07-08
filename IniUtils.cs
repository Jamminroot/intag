using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace intag
{
	public static class IniUtils
	{
		public static void AssignPropertyToFolder(string folder, string propertyValue, string oldPropertyValue)
		{
			if (string.IsNullOrWhiteSpace(propertyValue) && propertyValue == oldPropertyValue) { return; }
			var desktopIniFilepath = Path.Combine(folder, "Desktop.ini");
			var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".incomment");
			var contents = PrepareContentsForFolder(folder, propertyValue);
			var desktopIniExists = File.Exists(desktopIniFilepath);
			var f = new FileIOPermission(FileIOPermissionAccess.AllAccess, desktopIniFilepath);
			f.AddPathList(FileIOPermissionAccess.AllAccess, new FileInfo(desktopIniFilepath).DirectoryName);
			File.WriteAllText(tempFile, contents);
			if (!desktopIniExists)
			{
				try { File.Copy(tempFile, desktopIniFilepath, true); }
				catch (UnauthorizedAccessException)
				{
					//ignore - idk tf is it thrown, while we do get access later on 
				}
				return;
			}
			try
			{
				f.Demand();
				var fi = new FileInfo(desktopIniFilepath);
				var wasHidden = (fi.Attributes & FileAttributes.Hidden) > 0;
				var wasSystem = (fi.Attributes & FileAttributes.System) > 0;
				var wasReadonly = fi.IsReadOnly;
				if (File.Exists(desktopIniFilepath))
				{
					fi.IsReadOnly = false;
					fi.Attributes ^= FileAttributes.Hidden | FileAttributes.System;
				}
				try { File.Copy(tempFile, desktopIniFilepath, true); }
				catch (UnauthorizedAccessException)
				{
					//ignore - idk tf is that, but that fixes it?..
				}
				if (wasHidden) { fi.Attributes |= FileAttributes.Hidden; }
				if (wasSystem) { fi.Attributes |= FileAttributes.System; }
				if (wasReadonly) { fi.IsReadOnly = true; }
			}
			catch (SecurityException) { }
		}

		public static string GetFolderProperties(string path)
		{
			if (!File.Exists(path) || !IsCoorectSectionInDesktopIni(path)) return "";
			var lines = File.ReadAllLines(path);
			foreach (var line in lines)
			{
				if (!line.ToLower().StartsWith(Constants.MagicPropertyName.ToLower())) continue;
				var propertyValue = line.Substring(5).Trim('=', '\t', ' ').Substring(3).Trim();
				return propertyValue;
			}
			return "";
		}

		public static IEnumerable<string> GetNearbyPropertiesValues(string targetFolder)
		{
			var directoryInfo = new DirectoryInfo(targetFolder).Parent;
			if (directoryInfo == null) return null;
			var result = new HashSet<string>();
			foreach (var child in directoryInfo.GetDirectories())
			{
				var propertyValue = GetFolderProperties(Path.Combine(child.FullName, "Desktop.ini"));
				if (string.IsNullOrWhiteSpace(propertyValue)) continue;
				result.Add(propertyValue);
			}
			return result;
		}

		private static bool IsCoorectSectionInDesktopIni(string filePath)
		{
			return new Regex($@"\[{Constants.MagicGuid}\].*{Constants.MagicPropertyName}", RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(File.ReadAllText(filePath));
		}

		private static string PrepareContentsForFolder(string folder, string propertyValue)
		{
			var desktopIniFilepath = Path.Combine(folder, "Desktop.ini");
			if (!File.Exists(desktopIniFilepath) || string.IsNullOrWhiteSpace(File.ReadAllText(desktopIniFilepath))) return Constants.EmptyDesktopIniTemplate + propertyValue;
			var lines = new List<string>(File.ReadAllLines(desktopIniFilepath));
			var indexOfSection = -1;
			indexOfSection = lines.FindIndex(s => s.Contains(Constants.MagicGuid));
			if (indexOfSection < 0) return string.Join("\n", lines) + Constants.MagicSectionTemplate + propertyValue;
			var propertyIndex = lines.FindIndex(indexOfSection, s => s.ToLower().TrimStart().StartsWith(Constants.MagicPropertyName.ToLower()));
			if (propertyIndex >= 0) return lines[propertyIndex] = Constants.MagicPropertyTemplate + propertyValue;

			// prop not found
			lines.Insert(indexOfSection + 1, Constants.MagicSectionTemplate + propertyValue);
			return string.Join("\n", lines);
		}
	}
}