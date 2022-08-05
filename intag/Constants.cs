namespace intag
{
	public static class Constants
	{
		internal const string MagicPropertyName = "Prop5";
		internal const string RegistryName = "InTag";
		internal const string DesktopIni = "Desktop.ini";
		internal const string CanonicalTagViewAggregateName = "System.Photo.TagViewAggregate";
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