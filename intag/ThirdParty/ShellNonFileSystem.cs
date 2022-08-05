namespace intag.ThirdParty
{
	public class ShellNonFileSystemItem : ShellObject
	{
		internal ShellNonFileSystemItem(IShellItem2 shellItem) => nativeShellItem = shellItem;
	}
}