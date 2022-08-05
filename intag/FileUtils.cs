using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using intag.ThirdParty;

namespace intag
{
	public static class FileUtils
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
			//TODO: Encoding detection for new files - maybe in %APPDATA% or registry?.. 
			var enc = Encoding.Default;
			if (fi.Exists)
			{
				enc = EncodingUtils.DetectTextEncoding(desktopIniFilepath, out _);
				fi.Attributes ^= FileAttributes.Hidden | FileAttributes.System;
			}
			File.WriteAllText(desktopIniFilepath, contents, enc);
			fi.Attributes |= FileAttributes.Hidden;
			fi.Attributes |= FileAttributes.System;
			fi.IsReadOnly = true;                  
		}

		public static HashSet<string> GetFolderProperties(string path)
		{
			if (!File.Exists(path) || !IsCorrectSectionPresentInDesktopIni(path)) return new HashSet<string>();
			var enc = EncodingUtils.DetectTextEncoding(path, out var str);
			var lines = str.Split(new []{'\n'});
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
			var enc = EncodingUtils.DetectTextEncoding(path, out var str);
			var lines = str.Split(new []{'\n'});
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
			foreach (var file in directoryInfo.GetFiles())
			{
				var fileTags = GetFileTags(file.FullName);
				result.UnionWith(fileTags);
			}
			return result.ToList();
		}

		private static string[] GetFileTags(string file)
		{
			if (!File.Exists(file)) return new string[]{};
			var sf = ShellFile.FromFilePath(file);
			return sf.Properties.GetProperty(Constants.CanonicalTagViewAggregateName)?.ValueAsObject as string[] ?? new string[] {};
			//return new List<string>();//props.ValueAsObject as string)?.Split(';').ToList() ?? new List<string>();
		}
		
		private static bool IsCorrectSectionPresentInDesktopIni(string filePath)
		{
			var enc = EncodingUtils.DetectTextEncoding(filePath, out _);
			return new Regex($@"\[{Constants.MagicGuid}\].*{Constants.MagicPropertyName}", RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(File.ReadAllText(filePath, enc));
		}

		private static string PrepareContentsForFolder(string folder, string propertyValue)
		{
			return Constants.EmptyDesktopIniTemplate + propertyValue;
		}
	}
}