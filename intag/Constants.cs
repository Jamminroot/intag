using System.Collections.Generic;

namespace intag
{
	public static class Constants
	{
		// Property IDs for SummaryInformation FMTID
		internal const int PID_TITLE = 2;
		internal const int PID_SUBJECT = 3;
		internal const int PID_AUTHOR = 4;
		internal const int PID_KEYWORDS = 5;
		internal const int PID_COMMENTS = 6;

		// Property name to ID mapping (lowercase keys for case-insensitive lookup)
		internal static readonly Dictionary<string, int> PropertyNameToId = new()
		{
			{ "title", PID_TITLE },
			{ "subject", PID_SUBJECT },
			{ "author", PID_AUTHOR },
			{ "keywords", PID_KEYWORDS },
			{ "tags", PID_KEYWORDS },  // alias for keywords
			{ "comments", PID_COMMENTS },
		};

		// Properties that support multiple values (string arrays)
		internal static readonly HashSet<int> MultiValueProperties = new() { PID_KEYWORDS };

		// Property ID to display name mapping
		internal static readonly Dictionary<int, string> PropertyIdToDisplayName = new()
		{
			{ PID_TITLE, "Title" },
			{ PID_SUBJECT, "Subject" },
			{ PID_AUTHOR, "Author" },
			{ PID_KEYWORDS, "Keywords" },
			{ PID_COMMENTS, "Comments" },
		};

		// Helper to get property ID from name (case-insensitive)
		internal static int GetPropertyId(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
				return PID_KEYWORDS;  // default
			return PropertyNameToId.TryGetValue(propertyName.ToLowerInvariant(), out var id)
				? id
				: PID_KEYWORDS;
		}

		// Helper to check if property supports multiple values
		internal static bool IsMultiValueProperty(int propertyId) => MultiValueProperties.Contains(propertyId);

		internal const string MagicPropertyName = "Prop5";  // Legacy, for keywords
		internal const string RegistryName = "InTag";
		internal const string DesktopIni = "Desktop.ini";
		//internal const string CanonicalTagViewAggregateName = "System.Photo.TagViewAggregate";
		internal const string CanonicalKeywordsName = "System.Keywords";
		internal const string MagicGuid = "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}";

		internal const string MagicPropertyTemplate = MagicPropertyName + "=31, ";

		internal const string MagicSectionTemplate = @"
[" + MagicGuid + @"]
" + MagicPropertyTemplate;

		internal const string EmptyDesktopIniTemplate = @"[ViewState]
Mode=
Vid=
FolderType=Generic
" + MagicSectionTemplate;
	}
}