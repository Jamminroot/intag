using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;

namespace intag
{
	public static class FileUtils
	{
		static PropertyKey _propertyKey = new ("f29f85e0-4ff9-1068-ab91-08002b27b3d9", 5);
		static Guid _shellItem2Guid = new ("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
		static Guid _propStoreGuid =  new ("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");

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
			try
			{
				if (SHCreateItemFromParsingName(file, IntPtr.Zero, ref _shellItem2Guid, out var shellItem) != 0) throw new SystemException("Failed to create shell item");
				if (PSGetPropertyKeyFromName(Constants.CanonicalKeywordsName, out var propKey) != 0) throw new SystemException("Failed to create property key");
				if (shellItem == null) return;
				using var propVar = new PropVar(tags.ToArray());
				if(shellItem.GetPropertyStore(GetPropertyStoreOptions.ReadWrite, ref _propStoreGuid, out var propStore)!=0) throw new SystemException("Failed to get property store");
				propStore.SetValue(ref _propertyKey, propVar);
				if (propStore.Commit() != HRes.Ok) throw new SystemException("Failed to save tags");
				
				Marshal.ReleaseComObject(propStore);
				Marshal.ReleaseComObject(shellItem);
			}
			catch
			{
				// ignored
			}
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

		[DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int PSGetPropertyKeyFromName([In, MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName, out PropertyKey propkey);
			
			
		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            // The following parameter is not used - binding context.
            IntPtr pbc,
            ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem2 shellItem);
        
		private static HashSet<string> GetFileTags(string file)
		{
			if (!File.Exists(file)) return new HashSet<string>();
			try
			{
				if (SHCreateItemFromParsingName(file, IntPtr.Zero, ref _shellItem2Guid, out var shellItem) != 0) throw new SystemException("Failed to create shell item");
				if (PSGetPropertyKeyFromName(Constants.CanonicalKeywordsName, out var propKey) != 0) throw new SystemException("Failed to create property key");
				if (shellItem == null) return new HashSet<string>();
				using var propVar = new PropVar();
				shellItem.GetProperty(ref propKey, propVar);
				if(shellItem.GetPropertyStore(GetPropertyStoreOptions.BestEffort, ref _propStoreGuid, out var propStore)!=0) throw new SystemException("Failed to get property store");
				propStore.GetValue(ref _propertyKey, propVar);
				Marshal.ReleaseComObject(propStore);
				Marshal.ReleaseComObject(shellItem);
				if (propVar.Value is string[] { Length: > 0 } propValues) return new HashSet<string>(propValues);
				
			}
			catch
			{
				// ignored
			}
			return new HashSet<string>();
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