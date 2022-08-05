using System;
using System.Runtime.InteropServices;
using System.Text;

namespace intag.ThirdParty
{
	internal static class CoreNativeMethods
	{
		// Various helpers for forcing binding to proper version of Comctl32 (v6).
		[DllImport("kernel32.dll", SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
		internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string fileName);
		
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern int LoadString(
			IntPtr instanceHandle,
			int id,
			StringBuilder buffer,
			int bufferSize);
	}
}