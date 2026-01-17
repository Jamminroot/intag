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
        static Guid _shellItem2Guid = new ("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
        static Guid _propStoreGuid =  new ("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");

        // Helper to create PropertyKey for a given property ID
        private static PropertyKey GetPropertyKey(int propertyId)
        {
            return new PropertyKey(Constants.MagicGuid.Trim('{', '}'), propertyId);
        }

        public static void AssignTags(Dictionary<string, SortedSet<string>> objectTags)
        {
            foreach (var pair in objectTags)
            {
                var obj = pair.Key;
                var tags = pair.Value;
                if (File.Exists(obj))
                {
                    AssignTagsToFile(obj, tags);
                } else if (Directory.Exists(obj))
                {
                    AssignTagsToFolder(obj, tags);
                }
            }
        }
        
        public static void AddTags(string[] objects, string[] tags)
        {
            var objectTags = new Dictionary<string, SortedSet<string>>();
            foreach (var obj in objects)
            {
                if (File.Exists(obj))
                {
                    var fileTags = GetFileTags(obj);
                    fileTags.UnionWith(tags);
                    objectTags[obj] = fileTags;
                }
                else if (Directory.Exists(obj))
                {
                    var folderTags = GetFolderTags(obj);
                    folderTags.UnionWith(tags);
                    objectTags[obj] = folderTags;
                }
            }
            AssignTags(objectTags);
        }
       
        public static void RemoveTags(string[] objects, string[] tags)
        {
            var objectTags = new Dictionary<string, SortedSet<string>>();
            foreach (var obj in objects)
            {
                if (File.Exists(obj))
                {
                    var fileTags = GetFileTags(obj);
                    fileTags.ExceptWith(tags);
                    objectTags[obj] = fileTags;
                }
                else if (Directory.Exists(obj))
                {
                    var folderTags = GetFolderTags(obj);
                    folderTags.ExceptWith(tags);
                    objectTags[obj] = folderTags;
                }
            }
            AssignTags(objectTags);
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
        public static void AssignTagsToFolder(string folder, SortedSet<string> tags)
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

        public static void AssignTagsToFile(string file, SortedSet<string> tags)
        {
            AssignPropertyToFile(file, Constants.PID_KEYWORDS, tags.ToArray());
        }

        // Generic method to assign any property value to a file
        public static void AssignPropertyToFile(string file, int propertyId, object value)
        {
            if (!File.Exists(file)) return;
            try
            {
                if (SHCreateItemFromParsingName(file, IntPtr.Zero, ref _shellItem2Guid, out var shellItem) != 0)
                    throw new SystemException("Failed to create shell item");
                if (shellItem == null) return;

                var propKey = GetPropertyKey(propertyId);

                // Create PropVar based on value type
                PropVar propVar;
                if (value is string[] stringArray)
                    propVar = new PropVar(stringArray);
                else if (value is string stringValue)
                    propVar = new PropVar(stringValue);
                else
                    throw new ArgumentException($"Unsupported property value type: {value?.GetType()}");

                using (propVar)
                {
                    if (shellItem.GetPropertyStore(GetPropertyStoreOptions.ReadWrite, ref _propStoreGuid, out var propStore) != 0)
                        throw new SystemException("Failed to get property store");
                    propStore.SetValue(ref propKey, propVar);
                    if (propStore.Commit() != HRes.Ok)
                        throw new SystemException("Failed to save property");

                    Marshal.ReleaseComObject(propStore);
                }
                Marshal.ReleaseComObject(shellItem);
            }
            catch
            {
                // ignored
            }
        }

        // Convenience method for setting single-value string properties
        public static void AssignStringPropertyToFile(string file, int propertyId, string value)
        {
            AssignPropertyToFile(file, propertyId, value ?? string.Empty);
        }

        public static SortedSet<string> GetFolderTags(string folder)
        {
            var path = Path.Combine(folder, Constants.DesktopIni);
            if (!File.Exists(path) || !IsCorrectSectionPresentInDesktopIni(path)) return new SortedSet<string>();
            var enc = EncodingUtils.DetectTextEncoding(path, out var str);
            var lines = str.Split(new []{'\n'});
            foreach (var line in lines)
            {
                if (!line.StartsWith(Constants.MagicPropertyName, StringComparison.OrdinalIgnoreCase)) continue;
                var propertyValue = line.Substring(5).Trim('=', '\t', ' ').Substring(3).Trim();
                return new SortedSet<string>(propertyValue.Split(';')
                                                          .Select(s => s.Trim())
                                                          .Where(s => !string.IsNullOrWhiteSpace(s)));
            }
            return new SortedSet<string>();
        }

        public static string GetRawFolderProperties(string path)
        {
            if (!File.Exists(path) || !IsCorrectSectionPresentInDesktopIni(path)) return "";
            var enc = EncodingUtils.DetectTextEncoding(path, out var str);
            var lines = str.Split(new []{'\n'});
            foreach (var line in lines)
            {
                if (!line.StartsWith(Constants.MagicPropertyName, StringComparison.OrdinalIgnoreCase)) continue;
                var propertyValue = line.Substring(5).Trim('=', '\t', ' ').Substring(3).Trim();
                return propertyValue;
            }
            return "";
        }

        public static Dictionary<string, SortedSet<string>> GetObjectsTags(string[] objs)
        {
            var tags = new Dictionary<string, SortedSet<string>>();
            foreach (var obj in objs)
            {
                if (File.Exists(obj))
                {
                    tags[obj] = GetFileTags(obj);
                }
                else if (Directory.Exists(obj))
                {
                    tags[obj] = GetFolderTags(obj);
                }
            }
            return tags;
        }

        public static SortedSet<string> GetNearbyTags(string targetObject)
        {
            var directoryInfo = new DirectoryInfo(targetObject).Parent;
            if (directoryInfo == null) return null;
            var result = new SortedSet<string>();
            foreach (var child in directoryInfo.GetDirectories())
            {
                var propertyValue = GetFolderTags(child.FullName);
                if (propertyValue.Count==0) continue;
                result.UnionWith(propertyValue);
            }
            foreach (var file in directoryInfo.GetFiles())
            {
                var fileTags = GetFileTags(file.FullName);
                if (fileTags.Count==0) continue;
                result.UnionWith(fileTags);
            }
            return result;
        }

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PSGetPropertyKeyFromName([In, MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName, out PropertyKey propkey);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            IntPtr pbc,
            ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem2 shellItem);

        private static SortedSet<string> GetFileTags(string file)
        {
            var result = GetFileProperty(file, Constants.PID_KEYWORDS);
            if (result is string[] tags && tags.Length > 0)
                return new SortedSet<string>(tags);
            return new SortedSet<string>();
        }

        // Generic method to read any property from a file
        public static object GetFileProperty(string file, int propertyId)
        {
            if (!File.Exists(file)) return null;
            try
            {
                if (SHCreateItemFromParsingName(file, IntPtr.Zero, ref _shellItem2Guid, out var shellItem) != 0)
                    throw new SystemException("Failed to create shell item");
                if (shellItem == null) return null;

                var propKey = GetPropertyKey(propertyId);
                using var propVar = new PropVar();

                if (shellItem.GetPropertyStore(GetPropertyStoreOptions.BestEffort, ref _propStoreGuid, out var propStore) != 0)
                    throw new SystemException("Failed to get property store");
                propStore.GetValue(ref propKey, propVar);

                Marshal.ReleaseComObject(propStore);
                Marshal.ReleaseComObject(shellItem);

                return propVar.Value;
            }
            catch
            {
                // ignored
            }
            return null;
        }

        // Convenience method for reading single-value string properties
        public static string GetFileStringProperty(string file, int propertyId)
        {
            var result = GetFileProperty(file, propertyId);
            return result as string ?? string.Empty;
        }

        // Get all supported properties for a file
        public static Dictionary<int, object> GetFileProperties(string file)
        {
            var properties = new Dictionary<int, object>();
            foreach (var pid in new[] { Constants.PID_TITLE, Constants.PID_SUBJECT, Constants.PID_AUTHOR, Constants.PID_KEYWORDS, Constants.PID_COMMENTS })
            {
                var value = GetFileProperty(file, pid);
                if (value != null)
                    properties[pid] = value;
            }
            return properties;
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