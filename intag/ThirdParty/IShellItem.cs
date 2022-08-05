using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace intag.ThirdParty
{
	internal enum SICHINTF
	{
		SICHINT_DISPLAY = 0x00000000,
		SICHINT_CANONICAL = 0x10000000,
		SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000,
		SICHINT_ALLFIELDS = unchecked((int)0x80000000)
	}

	[ComImport,
	 Guid(ShellIIDGuid.IShellItem),
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellItem
	{
		// Not supported: IBindCtx.
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult BindToHandler(
			[In] IntPtr pbc,
			[In] ref Guid bhid,
			[In] ref Guid riid,
			[Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetDisplayName(
			[In] ShellNativeMethods.ShellItemDesignNameOptions sigdnName,
			out IntPtr ppszName);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAttributes([In] ShellNativeMethods.ShellFileGetAttributesOptions sfgaoMask, out ShellNativeMethods.ShellFileGetAttributesOptions psfgaoAttribs);

		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Compare(
			[In, MarshalAs(UnmanagedType.Interface)] IShellItem psi,
			[In] SICHINTF hint,
			out int piOrder);
	}

}