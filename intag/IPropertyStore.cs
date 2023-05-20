using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace intag
{
	/// <summary>A property store</summary>
	[ComImport]
	[Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyStore
	{
		/// <summary>Gets the number of properties contained in the property store.</summary>
		/// <param name="propertyCount"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HRes GetCount([Out] out uint propertyCount);

		/// <summary>Get a property key located at a specific index.</summary>
		/// <param name="propertyIndex"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HRes GetAt([In] uint propertyIndex, out PropertyKey key);

		/// <summary>Gets the value of a property from the store</summary>
		/// <param name="key"></param>
		/// <param name="pv"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HRes GetValue([In] ref PropertyKey key, [Out] PropVar pv);

		/// <summary>Sets the value of a property in the store</summary>
		/// <param name="key"></param>
		/// <param name="pv"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
		HRes SetValue([In] ref PropertyKey key, [In] PropVar pv);

		/// <summary>Commits the changes.</summary>
		/// <returns></returns>
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HRes Commit();
	}
}