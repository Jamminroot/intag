namespace intag.ThirdParty
{
	public sealed class ShellSearchConnector : ShellSearchCollection
	{
		internal ShellSearchConnector() => CoreHelpers.ThrowIfNotWin7();

		internal ShellSearchConnector(IShellItem2 shellItem)
			: this() => nativeShellItem = shellItem;

		/// <summary>Indicates whether this feature is supported on the current platform.</summary>
		public new static bool IsPlatformSupported =>
			// We need Windows 7 onwards ...
			CoreHelpers.RunningOnWin7;
	}
}