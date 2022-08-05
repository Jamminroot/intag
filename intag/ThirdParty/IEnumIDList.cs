using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace intag.ThirdParty
{

	[ComImport,
	 Guid(ShellIIDGuid.IEnumIDList),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IEnumIDList
	{
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Next(uint celt, out IntPtr rgelt, out uint pceltFetched);

		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Skip([In] uint celt);

		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Reset();

		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
	}
}