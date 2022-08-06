using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		public static void AssignTagsToObject(string obj, HashSet<string> tags)
		{
			if (File.Exists(obj))
			{
				AssignTagsToFile(obj, tags);
			} else if (Directory.Exists(obj))
			{
				AssignTagsToFolder(obj, tags);
			}
		}

		private static void Unhide(FileInfo fi)
		{
			var f = new FileIOPermission(FileIOPermissionAccess.AllAccess, fi.FullName);
			f.AddPathList(FileIOPermissionAccess.AllAccess, fi.FullName);
			if (fi.Exists) { fi.IsReadOnly = false; }
			f.Demand();
			if (fi.Exists)
			{
				fi.Attributes ^= FileAttributes.Hidden | FileAttributes.System;
			}
		}
		private static void Hide(FileInfo fi)
		{
			fi.Attributes |= FileAttributes.Hidden;
			fi.Attributes |= FileAttributes.System;
			fi.IsReadOnly = true; 
		}
		public static void AssignTagsToFolder(string folder, HashSet<string> tags)
		{
			var desktopIniFilepath = Path.Combine(folder, Constants.DesktopIni);
			var contents = PrepareContentsForFolder(folder, string.Join(";", tags));
			var fi = new FileInfo(desktopIniFilepath);
			Unhide(fi);
			//TODO: Encoding detection for new files - maybe in %APPDATA% or registry?.. 
			var enc = Encoding.Default;
			if (File.Exists(desktopIniFilepath))
			{
				enc = EncodingUtils.DetectTextEncoding(desktopIniFilepath, out _);
			}
			File.WriteAllText(desktopIniFilepath, contents, enc);
			Hide(fi);
		}

		public static void AssignTagsToFile(string file, HashSet<string> tags)
		{
			if (!File.Exists(file)) return;
			var sf = ShellFile.FromFilePath(file);
			var prop = sf.Properties.GetProperty<string[]>(Constants.CanonicalKeywordsName);
			prop.Value = tags.ToArray();
		}
		
		public static HashSet<string> GetFolderTags(string folder)
		{
			var path = Path.Combine(folder, Constants.DesktopIni);
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

		public static HashSet<string> GetObjectTags(string obj)
		{
			if (File.Exists(obj))
			{
				return GetFileTags(obj).ToHashSet();
			} 
			else if (Directory.Exists(obj))
			{
				return GetFolderTags(obj).ToHashSet();
			}
			return new HashSet<string>();
		}
		
		public static List<string> GetNearbyTags(string targetObject)
		{
			var directoryInfo= File.Exists(targetObject) ? new DirectoryInfo(targetObject).Parent : new DirectoryInfo(targetObject);
			if (directoryInfo == null) return null;
			var result = new HashSet<string>();
			foreach (var child in directoryInfo.GetDirectories())
			{
				var propertyValue = GetFolderTags(child.FullName);
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
			using (var sf = ShellFile.FromFilePath(file))
			{
				return sf.Properties.GetProperty(Constants.CanonicalKeywordsName)?.ValueAsObject as string[] ?? new string[] {};	
			}
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