namespace intag
{
	public static partial class Constants
	{
		internal const string DesktopIni = "Desktop.ini";
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