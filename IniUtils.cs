using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace intag
{
	public static class IniUtils
	{
		public static void AssignPropertyToFolder(string folder, HashSet<string> propertyValue)
		{
			var desktopIniFilepath = Path.Combine(folder, Constants.DesktopIni);
			var contents = PrepareContentsForFolder(folder, string.Join(";", propertyValue));
			
			var fi = new FileInfo(desktopIniFilepath);
			var f = new FileIOPermission(FileIOPermissionAccess.AllAccess, desktopIniFilepath);
			f.AddPathList(FileIOPermissionAccess.AllAccess, new FileInfo(desktopIniFilepath).DirectoryName);
			if (File.Exists(desktopIniFilepath))
			{
				fi.IsReadOnly = false;
			}
			f.Demand();
			fi.Attributes ^= FileAttributes.Hidden | FileAttributes.System;
			File.WriteAllText(desktopIniFilepath, contents);
			fi.Attributes |= FileAttributes.Hidden;
			fi.Attributes |= FileAttributes.System;
			fi.IsReadOnly = true;                  
		}

		public static HashSet<string> GetFolderProperties(string path)
		{
			if (!File.Exists(path) || !IsCorrectSectionPresentInDesktopIni(path)) return new HashSet<string>();
			var lines = File.ReadAllLines(path);
			foreach (var line in lines)
			{
				if (!line.ToLower().StartsWith(Constants.MagicPropertyName.ToLower())) continue;
				var propertyValue = line.Substring(5).Trim('=', '\t', ' ').Substring(3).Trim();
				return propertyValue.Split(';').Select(s=>s.Trim()).Where(s=>!string.IsNullOrWhiteSpace(s)).ToHashSet();
			}
			return new HashSet<string>();
		}
		
		public static string GetRawFolderProperties(string path)
		{
			if (!File.Exists(path) || !IsCorrectSectionPresentInDesktopIni(path)) return "";
			var lines = File.ReadAllLines(path);
			foreach (var line in lines)
			{
				if (!line.ToLower().StartsWith(Constants.MagicPropertyName.ToLower())) continue;
				var propertyValue = line.Substring(5).Trim('=', '\t', ' ').Substring(3).Trim();
				return propertyValue;
			}
			return "";
		}
		
		public static List<string> GetNearbyPropertiesValues(string targetFolder)
		{
			var directoryInfo = new DirectoryInfo(targetFolder).Parent;
			if (directoryInfo == null) return null;
			var result = new HashSet<string>();
			foreach (var child in directoryInfo.GetDirectories())
			{
				var propertyValue = GetFolderProperties(Path.Combine(child.FullName, Constants.DesktopIni));
				if (propertyValue.Count==0) continue;
				result.UnionWith(propertyValue);
			}
			return result.ToList();
		}

		private static bool IsCorrectSectionPresentInDesktopIni(string filePath)
		{
			return new Regex($@"\[{Constants.MagicGuid}\].*{Constants.MagicPropertyName}", RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(File.ReadAllText(filePath));
		}

		private static string PrepareContentsForFolder(string folder, string propertyValue)
		{
			return Constants.EmptyDesktopIniTemplate + propertyValue;
			var desktopIniFilepath = Path.Combine(folder, Constants.DesktopIni);
			if (!File.Exists(desktopIniFilepath) || string.IsNullOrWhiteSpace(File.ReadAllText(desktopIniFilepath))) ;
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