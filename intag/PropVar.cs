using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace intag
{
   [StructLayout(LayoutKind.Explicit)]
    public sealed class PropVar : IDisposable
    {
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromBooleanVector([In, MarshalAs(UnmanagedType.LPArray)] bool[] prgf, uint cElems, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromDoubleVector([In, Out] double[] prgn, uint cElems, [Out] PropVar propvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromFileTime([In] ref System.Runtime.InteropServices.ComTypes.FILETIME pftIn, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromFileTimeVector([In, Out] System.Runtime.InteropServices.ComTypes.FILETIME[] prgft, uint cElems, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt16Vector([In, Out] short[] prgn, uint cElems, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt32Vector([In, Out] int[] prgn, uint cElems, [Out] PropVar propVar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt64Vector([In, Out] long[] prgn, uint cElems, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromPropVariantVectorElem([In] PropVar propvarIn, uint iElem, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromStringVector([In, Out] string[] prgsz, uint cElems, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt16Vector([In, Out] ushort[] prgn, uint cElems, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt32Vector([In, Out] uint[] prgn, uint cElems, [Out] PropVar ppropvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt64Vector([In, Out] ulong[] prgn, uint cElems, [Out] PropVar ppropvar);

        [DllImport("Ole32.dll", PreserveSig = false)] // returns hresult
        internal static extern void PropVariantClear([In, Out] PropVar pvar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetBooleanElem([In] PropVar propVar, [In]uint iElem, [Out, MarshalAs(UnmanagedType.Bool)] out bool pfVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetDoubleElem([In] PropVar propVar, [In] uint iElem, [Out] out double pnVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern int PropVariantGetElementCount([In] PropVar propVar);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetFileTimeElem([In] PropVar propVar, [In] uint iElem, [Out, MarshalAs(UnmanagedType.Struct)] out System.Runtime.InteropServices.ComTypes.FILETIME pftVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt16Elem([In] PropVar propVar, [In] uint iElem, [Out] out short pnVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt32Elem([In] PropVar propVar, [In] uint iElem, [Out] out int pnVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt64Elem([In] PropVar propVar, [In] uint iElem, [Out] out long pnVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetStringElem([In] PropVar propVar, [In]  uint iElem, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt16Elem([In] PropVar propVar, [In] uint iElem, [Out] out ushort pnVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt32Elem([In] PropVar propVar, [In] uint iElem, [Out] out uint pnVal);

        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt64Elem([In] PropVar propVar, [In] uint iElem, [Out] out ulong pnVal);

        [DllImport("OleAut32.dll", PreserveSig = false)] // returns hresult
        internal static extern IntPtr SafeArrayAccessData(IntPtr psa);

        [DllImport("OleAut32.dll", PreserveSig = true)] // psa is actually returned, not hresult
        internal static extern IntPtr SafeArrayCreateVector(ushort vt, int lowerBound, uint cElems);

        [DllImport("OleAut32.dll", PreserveSig = true)] // retuns uint32
        internal static extern uint SafeArrayGetDim(IntPtr psa);

        // This decl for SafeArrayGetElement is only valid for cDims==1!
        [DllImport("OleAut32.dll", PreserveSig = false)] // returns hresult
        [return: MarshalAs(UnmanagedType.IUnknown)]
        internal static extern object SafeArrayGetElement(IntPtr psa, ref int rgIndices);

        [DllImport("OleAut32.dll", PreserveSig = false)] // returns hresult
        internal static extern int SafeArrayGetLBound(IntPtr psa, uint nDim);

        [DllImport("OleAut32.dll", PreserveSig = false)] // returns hresult
        internal static extern int SafeArrayGetUBound(IntPtr psa, uint nDim);

        [DllImport("OleAut32.dll", PreserveSig = false)] // returns hresult
        internal static extern void SafeArrayUnaccessData(IntPtr psa);

        [StructLayout(LayoutKind.Sequential)]
        private struct Blob
        {
            public int Number;
            public IntPtr Pointer;
        }

        // A static dictionary of delegates to get data from array's contained within PropVars
        private static Dictionary<Type, Action<PropVar, Array, uint>> _vectorActions = null;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static Dictionary<Type, Action<PropVar, Array, uint>> GenerateVectorActions()
        {
            Dictionary<Type, Action<PropVar, Array, uint>> cache = new Dictionary<Type, Action<PropVar, Array, uint>>
            {
                {
                    typeof(short),
                    (pv, array, i) =>
                    {
                        PropVariantGetInt16Elem(pv, i, out short val);
                        array.SetValue(val, i);
                    }
                },

                {
                    typeof(ushort),
                    (pv, array, i) =>
                    {
                        PropVariantGetUInt16Elem(pv, i, out ushort val);
                        array.SetValue(val, i);
                    }
                },

                {
                    typeof(int),
                    (pv, array, i) =>
                    {
                        PropVariantGetInt32Elem(pv, i, out int val);
                        array.SetValue(val, i);
                    }
                },

                {
                    typeof(uint),
                    (pv, array, i) =>
                    {
                        PropVariantGetUInt32Elem(pv, i, out uint val);
                        array.SetValue(val, i);
                    }
                },

                {
                    typeof(long),
                    (pv, array, i) =>
                    {
                        PropVariantGetInt64Elem(pv, i, out long val);
                        array.SetValue(val, i);
                    }
                },

                {
                    typeof(ulong),
                    (pv, array, i) =>
                    {
                        PropVariantGetUInt64Elem(pv, i, out ulong val);
                        array.SetValue(val, i);
                    }
                },

                {
                    typeof(DateTime),
                    (pv, array, i) =>
                    {
                        PropVariantGetFileTimeElem(pv, i, out System.Runtime.InteropServices.ComTypes.FILETIME val);

                        long fileTime = GetFileTimeAsLong(ref val);

                        array.SetValue(DateTime.FromFileTime(fileTime), i);
                    }
                },

                {
                    typeof(bool),
                    (pv, array, i) =>
                    {
                        PropVariantGetBooleanElem(pv, i, out bool val);
                        array.SetValue(val, i);
                    }
                },

                {
                    typeof(double),
                    (pv, array, i) =>
                    {
                        PropVariantGetDoubleElem(pv, i, out double val);
                        array.SetValue(val, i);
                    }
                },

                {
                    typeof(float),
                    (pv, array, i) => // float
                    {
                        float[] val = new float[1];
                        Marshal.Copy(pv._blob.Pointer, val, (int)i, 1);
                        array.SetValue(val[0], (int)i);
                    }
                },

                {
                    typeof(decimal),
                    (pv, array, i) =>
                    {
                        int[] val = new int[4];
                        for (int a = 0; a < val.Length; a++)
                        {
                            val[a] = Marshal.ReadInt32(pv._blob.Pointer,
                                (int)i * sizeof(decimal) + a * sizeof(int)); //index * size + offset quarter
                                  }
                        array.SetValue(new decimal(val), i);
                    }
                },

                {
                    typeof(string),
                    (pv, array, i) =>
                    {
                        string val = string.Empty;
                        PropVariantGetStringElem(pv, i, ref val);
                        array.SetValue(val, i);
                    }
                }
            };

            return cache;
        }

        /// <summary>Attempts to create a PropVar by finding an appropriate constructor.</summary>
        /// <param name="value">Object from which PropVar should be created.</param>
        public static PropVar FromObject(object value)
        {
            if (value == null)
            {
                return new PropVar();
            }
            else
            {
                Func<object, PropVar> func = GetDynamicConstructor(value.GetType());
                return func(value);
            }
        }

        // A dictionary and lock to contain compiled expression trees for constructors
        private static readonly Dictionary<Type, Func<object, PropVar>> _cache = new Dictionary<Type, Func<object, PropVar>>();

        private static readonly object _padlock = new object();

        // Retrieves a cached constructor expression. If no constructor has been cached, it attempts to find/add it. If it cannot be found an
        // exception is thrown. This method looks for a public constructor with the same parameter type as the object.
        private static Func<object, PropVar> GetDynamicConstructor(Type type)
        {
            lock (_padlock)
            {
                // initial check, if action is found, return it
                if (!_cache.TryGetValue(type, out var action))
                {
                    // iterates through all constructors
                    System.Reflection.ConstructorInfo constructor = typeof(PropVar)
                        .GetConstructor(new Type[] { type });

                    if (constructor == null)
                    {
                        // if the method was not found, throw.
                        throw new ArgumentException("This Value type is not supported.");
                    }
                    else // if the method was found, create an expression to call it.
                    {
                        // create parameters to action
                        ParameterExpression arg = Expression.Parameter(typeof(object), "arg");

                        // create an expression to invoke the constructor with an argument cast to the correct type
                        NewExpression create = Expression.New(constructor, Expression.Convert(arg, type));

                        // compiles expression into an action delegate
                        action = Expression.Lambda<Func<object, PropVar>>(create, arg).Compile();
                        _cache.Add(type, action);
                    }
                }
                return action;
            }
        }

        [FieldOffset(0)]
        private readonly decimal _decimal;

        // This is actually a VarEnum value, but the VarEnum type requires 4 bytes instead of the expected 2.
        [FieldOffset(0)]
        private ushort _valueType;

        // Reserved Fields
        //[FieldOffset(2)]
        //ushort _wReserved1;
        //[FieldOffset(4)]
        //ushort _wReserved2;
        //[FieldOffset(6)]
        //ushort _wReserved3;

        [FieldOffset(8)]
        private readonly Blob _blob;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        [FieldOffset(8)]
        private IntPtr _ptr;

        [FieldOffset(8)]
        private readonly int _int32;

        [FieldOffset(8)]
        private readonly uint _uint32;

        [FieldOffset(8)]
        private readonly byte _byte;

        [FieldOffset(8)]
        private readonly sbyte _sbyte;

        [FieldOffset(8)]
        private readonly short _short;

        [FieldOffset(8)]
        private readonly ushort _ushort;

        [FieldOffset(8)]
        private readonly long _long;

        [FieldOffset(8)]
        private readonly ulong _ulong;

        [FieldOffset(8)]
        private readonly double _double;

        [FieldOffset(8)]
        private readonly float _float;

        /// <summary>Default constrcutor</summary>
        public PropVar()
        {
            // left empty
        }

        /// <summary>Set a string value</summary>
        public PropVar(string value)
        {
            if (value == null)
            {
                throw new ArgumentException("String argument cannot be null or empty.", "value");
            }

            _valueType = (ushort)VarEnum.VT_LPWSTR;
            _ptr = Marshal.StringToCoTaskMemUni(value);
        }

        /// <summary>Set a string vector</summary>
        public PropVar(string[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromStringVector(value, (uint)value.Length, this);
        }

        /// <summary>Set a bool vector</summary>
        public PropVar(bool[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromBooleanVector(value, (uint)value.Length, this);
        }

        /// <summary>Set a short vector</summary>
        public PropVar(short[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromInt16Vector(value, (uint)value.Length, this);
        }

        /// <summary>Set a short vector</summary>
        public PropVar(ushort[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromUInt16Vector(value, (uint)value.Length, this);
        }

        /// <summary>Set an int vector</summary>
        public PropVar(int[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromInt32Vector(value, (uint)value.Length, this);
        }

        /// <summary>Set an uint vector</summary>
        public PropVar(uint[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromUInt32Vector(value, (uint)value.Length, this);
        }

        /// <summary>Set a long vector</summary>
        public PropVar(long[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromInt64Vector(value, (uint)value.Length, this);
        }

        /// <summary>Set a ulong vector</summary>
        public PropVar(ulong[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromUInt64Vector(value, (uint)value.Length, this);
        }

        /// <summary>&gt; Set a double vector</summary>
        public PropVar(double[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            InitPropVariantFromDoubleVector(value, (uint)value.Length, this);
        }

        /// <summary>Set a DateTime vector</summary>
        public PropVar(DateTime[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            System.Runtime.InteropServices.ComTypes.FILETIME[] fileTimeArr =
                new System.Runtime.InteropServices.ComTypes.FILETIME[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                fileTimeArr[i] = DateTimeToFileTime(value[i]);
            }

            InitPropVariantFromFileTimeVector(fileTimeArr, (uint)fileTimeArr.Length, this);
        }

        /// <summary>Set a bool value</summary>
        public PropVar(bool value)
        {
            _valueType = (ushort)VarEnum.VT_BOOL;
            _int32 = (value == true) ? -1 : 0;
        }

        /// <summary>Set a DateTime value</summary>
        public PropVar(DateTime value)
        {
            _valueType = (ushort)VarEnum.VT_FILETIME;

            System.Runtime.InteropServices.ComTypes.FILETIME ft = DateTimeToFileTime(value);
            InitPropVariantFromFileTime(ref ft, this);
        }

        /// <summary>Set a byte value</summary>
        public PropVar(byte value)
        {
            _valueType = (ushort)VarEnum.VT_UI1;
            _byte = value;
        }

        /// <summary>Set a sbyte value</summary>
        public PropVar(sbyte value)
        {
            _valueType = (ushort)VarEnum.VT_I1;
            _sbyte = value;
        }

        /// <summary>Set a short value</summary>
        public PropVar(short value)
        {
            _valueType = (ushort)VarEnum.VT_I2;
            _short = value;
        }

        /// <summary>Set an unsigned short value</summary>
        public PropVar(ushort value)
        {
            _valueType = (ushort)VarEnum.VT_UI2;
            _ushort = value;
        }

        /// <summary>Set an int value</summary>
        public PropVar(int value)
        {
            _valueType = (ushort)VarEnum.VT_I4;
            _int32 = value;
        }

        /// <summary>Set an unsigned int value</summary>
        public PropVar(uint value)
        {
            _valueType = (ushort)VarEnum.VT_UI4;
            _uint32 = value;
        }

        /// <summary>Set a decimal value</summary>
        public PropVar(decimal value)
        {
            _decimal = value;

            // It is critical that the value type be set after the decimal value, because they overlap. If valuetype is written first, its
            // value will be lost when _decimal is written.
            _valueType = (ushort)VarEnum.VT_DECIMAL;
        }

        /// <summary>Create a PropVar with a contained decimal array.</summary>
        /// <param name="value">Decimal array to wrap.</param>
        public PropVar(decimal[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            _valueType = (ushort)(VarEnum.VT_DECIMAL | VarEnum.VT_VECTOR);
            _int32 = value.Length;

            // allocate required memory for array with 128bit elements
            _blob.Pointer = Marshal.AllocCoTaskMem(value.Length * sizeof(decimal));
            for (int i = 0; i < value.Length; i++)
            {
                int[] bits = decimal.GetBits(value[i]);
                Marshal.Copy(bits, 0, _blob.Pointer, bits.Length);
            }
        }

        /// <summary>Create a PropVar containing a float type.</summary>
        public PropVar(float value)
        {
            _valueType = (ushort)VarEnum.VT_R4;

            _float = value;
        }

        /// <summary>Creates a PropVar containing a float[] array.</summary>
        public PropVar(float[] value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }

            _valueType = (ushort)(VarEnum.VT_R4 | VarEnum.VT_VECTOR);
            _int32 = value.Length;

            _blob.Pointer = Marshal.AllocCoTaskMem(value.Length * sizeof(float));

            Marshal.Copy(value, 0, _blob.Pointer, value.Length);
        }

        /// <summary>Set a long</summary>
        public PropVar(long value)
        {
            _long = value;
            _valueType = (ushort)VarEnum.VT_I8;
        }

        /// <summary>Set a ulong</summary>
        public PropVar(ulong value)
        {
            _valueType = (ushort)VarEnum.VT_UI8;
            _ulong = value;
        }

        /// <summary>Set a double</summary>
        public PropVar(double value)
        {
            _valueType = (ushort)VarEnum.VT_R8;
            _double = value;
        }

        /// <summary>Set an IUnknown value</summary>
        /// <param name="value">The new value to set.</param>
        internal void SetIUnknown(object value)
        {
            _valueType = (ushort)VarEnum.VT_UNKNOWN;
            _ptr = Marshal.GetIUnknownForObject(value);
        }

        /// <summary>Set a safe array value</summary>
        /// <param name="array">The new value to set.</param>
        internal void SetSafeArray(Array array)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            const ushort vtUnknown = 13;
            IntPtr psa = SafeArrayCreateVector(vtUnknown, 0, (uint)array.Length);

            IntPtr pvData = SafeArrayAccessData(psa);
            try // to remember to release lock on data
            {
                for (int i = 0; i < array.Length; ++i)
                {
                    object obj = array.GetValue(i);
                    IntPtr punk = (obj != null) ? Marshal.GetIUnknownForObject(obj) : IntPtr.Zero;
                    Marshal.WriteIntPtr(pvData, i * IntPtr.Size, punk);
                }
            }
            finally
            {
                SafeArrayUnaccessData(psa);
            }

            _valueType = (ushort)VarEnum.VT_ARRAY | (ushort)VarEnum.VT_UNKNOWN;
            _ptr = psa;
        }

        /// <summary>Gets or sets the variant type.</summary>
        public VarEnum VarType
        {
            get => (VarEnum)_valueType;
            set => _valueType = (ushort)value;
        }

        /// <summary>Checks if this has an empty or null value</summary>
        /// <returns></returns>
        public bool IsNullOrEmpty => (_valueType == (ushort)VarEnum.VT_EMPTY || _valueType == (ushort)VarEnum.VT_NULL);

        /// <summary>Gets the variant value.</summary>
        public object Value
        {
            get
            {
                switch ((VarEnum)_valueType)
                {
                    case VarEnum.VT_I1:
                        return _sbyte;

                    case VarEnum.VT_UI1:
                        return _byte;

                    case VarEnum.VT_I2:
                        return _short;

                    case VarEnum.VT_UI2:
                        return _ushort;

                    case VarEnum.VT_I4:
                    case VarEnum.VT_INT:
                        return _int32;

                    case VarEnum.VT_UI4:
                    case VarEnum.VT_UINT:
                        return _uint32;

                    case VarEnum.VT_I8:
                        return _long;

                    case VarEnum.VT_UI8:
                        return _ulong;

                    case VarEnum.VT_R4:
                        return _float;

                    case VarEnum.VT_R8:
                        return _double;

                    case VarEnum.VT_BOOL:
                        return _int32 == -1;

                    case VarEnum.VT_ERROR:
                        return _long;

                    case VarEnum.VT_CY:
                        return _decimal;

                    case VarEnum.VT_DATE:
                        return DateTime.FromOADate(_double);

                    case VarEnum.VT_FILETIME:
                        return DateTime.FromFileTime(_long);

                    case VarEnum.VT_BSTR:
                        return Marshal.PtrToStringBSTR(_ptr);

                    case VarEnum.VT_BLOB:
                        return GetBlobData();

                    case VarEnum.VT_LPSTR:
                        return Marshal.PtrToStringAnsi(_ptr);

                    case VarEnum.VT_LPWSTR:
                        return Marshal.PtrToStringUni(_ptr);

                    case VarEnum.VT_UNKNOWN:
                        return Marshal.GetObjectForIUnknown(_ptr);

                    case VarEnum.VT_DISPATCH:
                        return Marshal.GetObjectForIUnknown(_ptr);

                    case VarEnum.VT_DECIMAL:
                        return _decimal;

                    case VarEnum.VT_ARRAY | VarEnum.VT_UNKNOWN:
                        return CrackSingleDimSafeArray(_ptr);

                    case (VarEnum.VT_VECTOR | VarEnum.VT_LPWSTR):
                        return GetVector<string>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_I2):
                        return GetVector<short>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_UI2):
                        return GetVector<ushort>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_I4):
                        return GetVector<int>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_UI4):
                        return GetVector<uint>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_I8):
                        return GetVector<long>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_UI8):
                        return GetVector<ulong>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_R4):
                        return GetVector<float>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_R8):
                        return GetVector<double>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_BOOL):
                        return GetVector<bool>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_FILETIME):
                        return GetVector<DateTime>();

                    case (VarEnum.VT_VECTOR | VarEnum.VT_DECIMAL):
                        return GetVector<decimal>();

                    default:
                        // if the value cannot be marshaled
                        return null;
                }
            }
        }

        private static long GetFileTimeAsLong(ref System.Runtime.InteropServices.ComTypes.FILETIME val) => (((long)val.dwHighDateTime) << 32) + val.dwLowDateTime;

        private static System.Runtime.InteropServices.ComTypes.FILETIME DateTimeToFileTime(DateTime value)
        {
            long hFT = value.ToFileTime();
            System.Runtime.InteropServices.ComTypes.FILETIME ft =
                new System.Runtime.InteropServices.ComTypes.FILETIME
                {
                    dwLowDateTime = (int)(hFT & 0xFFFFFFFF),
                    dwHighDateTime = (int)(hFT >> 32)
                };
            return ft;
        }

        private object GetBlobData()
        {
            byte[] blobData = new byte[_int32];

            IntPtr pBlobData = _blob.Pointer;
            Marshal.Copy(pBlobData, blobData, 0, _int32);

            return blobData;
        }

        private Array GetVector<T>()
        {
            int count = PropVariantGetElementCount(this);
            if (count <= 0) { return null; }

            lock (_padlock)
            {
                if (_vectorActions == null)
                {
                    _vectorActions = GenerateVectorActions();
                }
            }

            if (!_vectorActions.TryGetValue(typeof(T), out var action))
            {
                throw new InvalidCastException("Cannot be cast to unsupported type.");
            }

            Array array = new T[count];
            for (uint i = 0; i < count; i++)
            {
                action(this, array, i);
            }

            return array;
        }

        private static Array CrackSingleDimSafeArray(IntPtr psa)
        {
            uint cDims = SafeArrayGetDim(psa);
            if (cDims != 1)
                throw new ArgumentException("Multi-dimensional SafeArrays not supported.", "psa");

            int lBound = SafeArrayGetLBound(psa, 1U);
            int uBound = SafeArrayGetUBound(psa, 1U);

            int n = uBound - lBound + 1; // uBound is inclusive

            object[] array = new object[n];
            for (int i = lBound; i <= uBound; ++i)
            {
                array[i] = SafeArrayGetElement(psa, ref i);
            }

            return array;
        }

        /// <summary>Disposes the object, calls the clear function.</summary>
        public void Dispose()
        {
            PropVariantClear(this);

            GC.SuppressFinalize(this);
        }

        /// <summary>Finalizer</summary>
        ~PropVar()
        {
            Dispose();
        }

        /// <summary>Provides an simple string representation of the contained data and type.</summary>
        /// <returns></returns>
        public override string ToString() => string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0}: {1}", Value, VarType.ToString());
    }
}