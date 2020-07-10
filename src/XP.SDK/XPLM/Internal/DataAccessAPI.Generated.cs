using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class DataAccessAPI
    {
        private static IntPtr FindDataRefPtr;
        private static IntPtr CanWriteDataRefPtr;
        private static IntPtr IsDataRefGoodPtr;
        private static IntPtr GetDataRefTypesPtr;
        private static IntPtr GetDataiPtr;
        private static IntPtr SetDataiPtr;
        private static IntPtr GetDatafPtr;
        private static IntPtr SetDatafPtr;
        private static IntPtr GetDatadPtr;
        private static IntPtr SetDatadPtr;
        private static IntPtr GetDataviPtr;
        private static IntPtr SetDataviPtr;
        private static IntPtr GetDatavfPtr;
        private static IntPtr SetDatavfPtr;
        private static IntPtr GetDatabPtr;
        private static IntPtr SetDatabPtr;
        private static IntPtr RegisterDataAccessorPtr;
        private static IntPtr UnregisterDataAccessorPtr;
        private static IntPtr ShareDataPtr;
        private static IntPtr UnshareDataPtr;

        static DataAccessAPI()
        {
            FindDataRefPtr = Lib.GetExport("XPLMFindDataRef");
            CanWriteDataRefPtr = Lib.GetExport("XPLMCanWriteDataRef");
            IsDataRefGoodPtr = Lib.GetExport("XPLMIsDataRefGood");
            GetDataRefTypesPtr = Lib.GetExport("XPLMGetDataRefTypes");
            GetDataiPtr = Lib.GetExport("XPLMGetDatai");
            SetDataiPtr = Lib.GetExport("XPLMSetDatai");
            GetDatafPtr = Lib.GetExport("XPLMGetDataf");
            SetDatafPtr = Lib.GetExport("XPLMSetDataf");
            GetDatadPtr = Lib.GetExport("XPLMGetDatad");
            SetDatadPtr = Lib.GetExport("XPLMSetDatad");
            GetDataviPtr = Lib.GetExport("XPLMGetDatavi");
            SetDataviPtr = Lib.GetExport("XPLMSetDatavi");
            GetDatavfPtr = Lib.GetExport("XPLMGetDatavf");
            SetDatavfPtr = Lib.GetExport("XPLMSetDatavf");
            GetDatabPtr = Lib.GetExport("XPLMGetDatab");
            SetDatabPtr = Lib.GetExport("XPLMSetDatab");
            RegisterDataAccessorPtr = Lib.GetExport("XPLMRegisterDataAccessor");
            UnregisterDataAccessorPtr = Lib.GetExport("XPLMUnregisterDataAccessor");
            ShareDataPtr = Lib.GetExport("XPLMShareData");
            UnshareDataPtr = Lib.GetExport("XPLMUnshareData");
        }

        
        /// <summary>
        /// <para>
        /// Given a c-style string that names the data ref, this routine looks up the
        /// actual opaque XPLMDataRef that you use to read and write the data. The
        /// string names for datarefs are published on the X-Plane SDK web site.
        /// </para>
        /// <para>
        /// This function returns NULL if the data ref cannot be found.
        /// </para>
        /// <para>
        /// NOTE: this function is relatively expensive; save the XPLMDataRef this
        /// function returns for future use. Do not look up your data ref by string
        /// every time you need to read or write it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe DataRef FindDataRef(byte* inDataRefName)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(FindDataRefPtr);
            DataRef result;
            IL.Push(inDataRefName);
            IL.Push(FindDataRefPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(DataRef), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Given a c-style string that names the data ref, this routine looks up the
        /// actual opaque XPLMDataRef that you use to read and write the data. The
        /// string names for datarefs are published on the X-Plane SDK web site.
        /// </para>
        /// <para>
        /// This function returns NULL if the data ref cannot be found.
        /// </para>
        /// <para>
        /// NOTE: this function is relatively expensive; save the XPLMDataRef this
        /// function returns for future use. Do not look up your data ref by string
        /// every time you need to read or write it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe DataRef FindDataRef(in ReadOnlySpan<char> inDataRefName)
        {
            IL.DeclareLocals(false);
            Span<byte> inDataRefNameUtf8 = stackalloc byte[(inDataRefName.Length << 1) | 1];
            var inDataRefNamePtr = Utils.ToUtf8Unsafe(inDataRefName, inDataRefNameUtf8);
            return FindDataRef(inDataRefNamePtr);
        }

        
        /// <summary>
        /// <para>
        /// Given a data ref, this routine returns true if you can successfully set the
        /// data, false otherwise. Some datarefs are read-only.
        /// </para>
        /// <para>
        /// NOTE: even if a dataref is marked writable, it may not act writable.  This
        /// can happen for datarefs that X-Plane writes to on every frame of
        /// simulation.  In some cases, the dataref is writable but you have to set a
        /// separate "override" dataref to 1 to stop X-Plane from writing it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CanWriteDataRef(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CanWriteDataRefPtr);
            int result;
            IL.Push(inDataRef);
            IL.Push(CanWriteDataRefPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This function returns true if the passed in handle is a valid dataref that
        /// is not orphaned.
        /// </para>
        /// <para>
        /// Note: there is normally no need to call this function; datarefs returned by
        /// XPLMFindDataRef remain valid (but possibly orphaned) unless there is a
        /// complete plugin reload (in which case your plugin is reloaded anyway).
        /// Orphaned datarefs can be safely read and return 0. Therefore you never need
        /// to call XPLMIsDataRefGood to 'check' the safety of a dataref.
        /// (XPLMIsDatarefGood performs some slow checking of the handle validity, so
        /// it has a perormance cost.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsDataRefGood(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(IsDataRefGoodPtr);
            int result;
            IL.Push(inDataRef);
            IL.Push(IsDataRefGoodPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the types of the data ref for accessor use. If a data
        /// ref is available in multiple data types, the bit-wise OR of these types
        /// will be returned.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static DataTypeID GetDataRefTypes(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDataRefTypesPtr);
            DataTypeID result;
            IL.Push(inDataRef);
            IL.Push(GetDataRefTypesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(DataTypeID), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Read an integer data ref and return its value. The return value is the
        /// dataref value or 0 if the dataref is NULL or the plugin is disabled.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetDatai(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDataiPtr);
            int result;
            IL.Push(inDataRef);
            IL.Push(GetDataiPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Write a new value to an integer data ref. This routine is a no-op if the
        /// plugin publishing the dataref is disabled, the dataref is NULL, or the
        /// dataref is not writable.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDatai(DataRef inDataRef, int inValue)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetDataiPtr);
            IL.Push(inDataRef);
            IL.Push(inValue);
            IL.Push(SetDataiPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// Read a single precision floating point dataref and return its value. The
        /// return value is the dataref value or 0.0 if the dataref is NULL or the
        /// plugin is disabled.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float GetDataf(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDatafPtr);
            float result;
            IL.Push(inDataRef);
            IL.Push(GetDatafPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Write a new value to a single precision floating point data ref. This
        /// routine is a no-op if the plugin publishing the dataref is disabled, the
        /// dataref is NULL, or the dataref is not writable.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDataf(DataRef inDataRef, float inValue)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetDatafPtr);
            IL.Push(inDataRef);
            IL.Push(inValue);
            IL.Push(SetDatafPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(float)));
        }

        
        /// <summary>
        /// <para>
        /// Read a double precision floating point dataref and return its value. The
        /// return value is the dataref value or 0.0 if the dataref is NULL or the
        /// plugin is disabled.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static double GetDatad(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDatadPtr);
            double result;
            IL.Push(inDataRef);
            IL.Push(GetDatadPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(double), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Write a new value to a double precision floating point data ref. This
        /// routine is a no-op if the plugin publishing the dataref is disabled, the
        /// dataref is NULL, or the dataref is not writable.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDatad(DataRef inDataRef, double inValue)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetDatadPtr);
            IL.Push(inDataRef);
            IL.Push(inValue);
            IL.Push(SetDatadPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(double)));
        }

        
        /// <summary>
        /// <para>
        /// Read a part of an integer array dataref. If you pass NULL for outValues,
        /// the routine will return the size of the array, ignoring inOffset and inMax.
        /// </para>
        /// <para>
        /// If outValues is not NULL, then up to inMax values are copied from the
        /// dataref into outValues, starting at inOffset in the dataref. If inMax +
        /// inOffset is larger than the size of the dataref, less than inMax values
        /// will be copied. The number of values copied is returned.
        /// </para>
        /// <para>
        /// Note: the semantics of array datarefs are entirely implemented by the
        /// plugin (or X-Plane) that provides the dataref, not the SDK itself; the
        /// above description is how these datarefs are intended to work, but a rogue
        /// plugin may have different behavior.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDatavi(DataRef inDataRef, int* outValues, int inOffset, int inMax)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDataviPtr);
            int result;
            IL.Push(inDataRef);
            IL.Push(outValues);
            IL.Push(inOffset);
            IL.Push(inMax);
            IL.Push(GetDataviPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef), typeof(int*), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Write part or all of an integer array dataref. The values passed by
        /// inValues are written into the dataref starting at inOffset. Up to inCount
        /// values are written; however if the values would write "off the end" of the
        /// dataref array, then fewer values are written.
        /// </para>
        /// <para>
        /// Note: the semantics of array datarefs are entirely implemented by the
        /// plugin (or X-Plane) that provides the dataref, not the SDK itself; the
        /// above description is how these datarefs are intended to work, but a rogue
        /// plugin may have different behavior.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetDatavi(DataRef inDataRef, int* inValues, int inoffset, int inCount)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetDataviPtr);
            IL.Push(inDataRef);
            IL.Push(inValues);
            IL.Push(inoffset);
            IL.Push(inCount);
            IL.Push(SetDataviPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(int*), typeof(int), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// Read a part of a single precision floating point array dataref. If you pass
        /// NULL for outVaules, the routine will return the size of the array, ignoring
        /// inOffset and inMax.
        /// </para>
        /// <para>
        /// If outValues is not NULL, then up to inMax values are copied from the
        /// dataref into outValues, starting at inOffset in the dataref. If inMax +
        /// inOffset is larger than the size of the dataref, less than inMax values
        /// will be copied. The number of values copied is returned.
        /// </para>
        /// <para>
        /// Note: the semantics of array datarefs are entirely implemented by the
        /// plugin (or X-Plane) that provides the dataref, not the SDK itself; the
        /// above description is how these datarefs are intended to work, but a rogue
        /// plugin may have different behavior.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDatavf(DataRef inDataRef, float* outValues, int inOffset, int inMax)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDatavfPtr);
            int result;
            IL.Push(inDataRef);
            IL.Push(outValues);
            IL.Push(inOffset);
            IL.Push(inMax);
            IL.Push(GetDatavfPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef), typeof(float*), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Write part or all of a single precision floating point array dataref. The
        /// values passed by inValues are written into the dataref starting at
        /// inOffset. Up to inCount values are written; however if the values would
        /// write "off the end" of the dataref array, then fewer values are written.
        /// </para>
        /// <para>
        /// Note: the semantics of array datarefs are entirely implemented by the
        /// plugin (or X-Plane) that provides the dataref, not the SDK itself; the
        /// above description is how these datarefs are intended to work, but a rogue
        /// plugin may have different behavior.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetDatavf(DataRef inDataRef, float* inValues, int inoffset, int inCount)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetDatavfPtr);
            IL.Push(inDataRef);
            IL.Push(inValues);
            IL.Push(inoffset);
            IL.Push(inCount);
            IL.Push(SetDatavfPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(float*), typeof(int), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// Read a part of a byte array dataref. If you pass NULL for outVaules, the
        /// routine will return the size of the array, ignoring inOffset and inMax.
        /// </para>
        /// <para>
        /// If outValues is not NULL, then up to inMax values are copied from the
        /// dataref into outValues, starting at inOffset in the dataref. If inMax +
        /// inOffset is larger than the size of the dataref, less than inMax values
        /// will be copied. The number of values copied is returned.
        /// </para>
        /// <para>
        /// Note: the semantics of array datarefs are entirely implemented by the
        /// plugin (or X-Plane) that provides the dataref, not the SDK itself; the
        /// above description is how these datarefs are intended to work, but a rogue
        /// plugin may have different behavior.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDatab(DataRef inDataRef, void* outValue, int inOffset, int inMaxBytes)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetDatabPtr);
            int result;
            IL.Push(inDataRef);
            IL.Push(outValue);
            IL.Push(inOffset);
            IL.Push(inMaxBytes);
            IL.Push(GetDatabPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef), typeof(void*), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Write part or all of a byte array dataref. The values passed by inValues
        /// are written into the dataref starting at inOffset. Up to inCount values are
        /// written; however if the values would write "off the end" of the dataref
        /// array, then fewer values are written.
        /// </para>
        /// <para>
        /// Note: the semantics of array datarefs are entirely implemented by the
        /// plugin (or X-Plane) that provides the dataref, not the SDK itself; the
        /// above description is how these datarefs are intended to work, but a rogue
        /// plugin may have different behavior.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetDatab(DataRef inDataRef, void* inValue, int inOffset, int inLength)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetDatabPtr);
            IL.Push(inDataRef);
            IL.Push(inValue);
            IL.Push(inOffset);
            IL.Push(inLength);
            IL.Push(SetDatabPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(void*), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe DataRef RegisterDataAccessorPrivate(byte* inDataName, DataTypeID inDataType, int inIsWritable, IntPtr inReadInt, IntPtr inWriteInt, IntPtr inReadFloat, IntPtr inWriteFloat, IntPtr inReadDouble, IntPtr inWriteDouble, IntPtr inReadIntArray, IntPtr inWriteIntArray, IntPtr inReadFloatArray, IntPtr inWriteFloatArray, IntPtr inReadData, IntPtr inWriteData, void* inReadRefcon, void* inWriteRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(RegisterDataAccessorPtr);
            DataRef result;
            IL.Push(inDataName);
            IL.Push(inDataType);
            IL.Push(inIsWritable);
            IL.Push(inReadInt);
            IL.Push(inWriteInt);
            IL.Push(inReadFloat);
            IL.Push(inWriteFloat);
            IL.Push(inReadDouble);
            IL.Push(inWriteDouble);
            IL.Push(inReadIntArray);
            IL.Push(inWriteIntArray);
            IL.Push(inReadFloatArray);
            IL.Push(inWriteFloatArray);
            IL.Push(inReadData);
            IL.Push(inWriteData);
            IL.Push(inReadRefcon);
            IL.Push(inWriteRefcon);
            IL.Push(RegisterDataAccessorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(DataRef), typeof(byte*), typeof(DataTypeID), typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(void*), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine creates a new item of data that can be read and written. Pass
        /// in the data's full name for searching, the type(s) of the data for
        /// accessing, and whether the data can be written to. For each data type you
        /// support, pass in a read accessor function and a write accessor function if
        /// necessary. Pass NULL for data types you do not support or write accessors
        /// if you are read-only.
        /// </para>
        /// <para>
        /// You are returned a data ref for the new item of data created. You can use
        /// this data ref to unregister your data later or read or write from it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe DataRef RegisterDataAccessor(byte* inDataName, DataTypeID inDataType, int inIsWritable, GetDataiCallback inReadInt, SetDataiCallback inWriteInt, GetDatafCallback inReadFloat, SetDatafCallback inWriteFloat, GetDatadCallback inReadDouble, SetDatadCallback inWriteDouble, GetDataviCallback inReadIntArray, SetDataviCallback inWriteIntArray, GetDatavfCallback inReadFloatArray, SetDatavfCallback inWriteFloatArray, GetDatabCallback inReadData, SetDatabCallback inWriteData, void* inReadRefcon, void* inWriteRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inReadIntPtr = inReadInt != null ? Marshal.GetFunctionPointerForDelegate(inReadInt) : default;
            IntPtr inWriteIntPtr = inWriteInt != null ? Marshal.GetFunctionPointerForDelegate(inWriteInt) : default;
            IntPtr inReadFloatPtr = inReadFloat != null ? Marshal.GetFunctionPointerForDelegate(inReadFloat) : default;
            IntPtr inWriteFloatPtr = inWriteFloat != null ? Marshal.GetFunctionPointerForDelegate(inWriteFloat) : default;
            IntPtr inReadDoublePtr = inReadDouble != null ? Marshal.GetFunctionPointerForDelegate(inReadDouble) : default;
            IntPtr inWriteDoublePtr = inWriteDouble != null ? Marshal.GetFunctionPointerForDelegate(inWriteDouble) : default;
            IntPtr inReadIntArrayPtr = inReadIntArray != null ? Marshal.GetFunctionPointerForDelegate(inReadIntArray) : default;
            IntPtr inWriteIntArrayPtr = inWriteIntArray != null ? Marshal.GetFunctionPointerForDelegate(inWriteIntArray) : default;
            IntPtr inReadFloatArrayPtr = inReadFloatArray != null ? Marshal.GetFunctionPointerForDelegate(inReadFloatArray) : default;
            IntPtr inWriteFloatArrayPtr = inWriteFloatArray != null ? Marshal.GetFunctionPointerForDelegate(inWriteFloatArray) : default;
            IntPtr inReadDataPtr = inReadData != null ? Marshal.GetFunctionPointerForDelegate(inReadData) : default;
            IntPtr inWriteDataPtr = inWriteData != null ? Marshal.GetFunctionPointerForDelegate(inWriteData) : default;
            DataRef result = RegisterDataAccessorPrivate(inDataName, inDataType, inIsWritable, inReadIntPtr, inWriteIntPtr, inReadFloatPtr, inWriteFloatPtr, inReadDoublePtr, inWriteDoublePtr, inReadIntArrayPtr, inWriteIntArrayPtr, inReadFloatArrayPtr, inWriteFloatArrayPtr, inReadDataPtr, inWriteDataPtr, inReadRefcon, inWriteRefcon);
            GC.KeepAlive(inWriteData);
            GC.KeepAlive(inReadData);
            GC.KeepAlive(inWriteFloatArray);
            GC.KeepAlive(inReadFloatArray);
            GC.KeepAlive(inWriteIntArray);
            GC.KeepAlive(inReadIntArray);
            GC.KeepAlive(inWriteDouble);
            GC.KeepAlive(inReadDouble);
            GC.KeepAlive(inWriteFloat);
            GC.KeepAlive(inReadFloat);
            GC.KeepAlive(inWriteInt);
            GC.KeepAlive(inReadInt);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine creates a new item of data that can be read and written. Pass
        /// in the data's full name for searching, the type(s) of the data for
        /// accessing, and whether the data can be written to. For each data type you
        /// support, pass in a read accessor function and a write accessor function if
        /// necessary. Pass NULL for data types you do not support or write accessors
        /// if you are read-only.
        /// </para>
        /// <para>
        /// You are returned a data ref for the new item of data created. You can use
        /// this data ref to unregister your data later or read or write from it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe DataRef RegisterDataAccessor(in ReadOnlySpan<char> inDataName, DataTypeID inDataType, int inIsWritable, GetDataiCallback inReadInt, SetDataiCallback inWriteInt, GetDatafCallback inReadFloat, SetDatafCallback inWriteFloat, GetDatadCallback inReadDouble, SetDatadCallback inWriteDouble, GetDataviCallback inReadIntArray, SetDataviCallback inWriteIntArray, GetDatavfCallback inReadFloatArray, SetDatavfCallback inWriteFloatArray, GetDatabCallback inReadData, SetDatabCallback inWriteData, void* inReadRefcon, void* inWriteRefcon)
        {
            IL.DeclareLocals(false);
            Span<byte> inDataNameUtf8 = stackalloc byte[(inDataName.Length << 1) | 1];
            var inDataNamePtr = Utils.ToUtf8Unsafe(inDataName, inDataNameUtf8);
            return RegisterDataAccessor(inDataNamePtr, inDataType, inIsWritable, inReadInt, inWriteInt, inReadFloat, inWriteFloat, inReadDouble, inWriteDouble, inReadIntArray, inWriteIntArray, inReadFloatArray, inWriteFloatArray, inReadData, inWriteData, inReadRefcon, inWriteRefcon);
        }

        
        /// <summary>
        /// <para>
        /// Use this routine to unregister any data accessors you may have registered.
        /// You unregister a data ref by the XPLMDataRef you get back from
        /// registration. Once you unregister a data ref, your function pointer will
        /// not be called anymore.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterDataAccessor(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(UnregisterDataAccessorPtr);
            IL.Push(inDataRef);
            IL.Push(UnregisterDataAccessorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe int ShareDataPrivate(byte* inDataName, DataTypeID inDataType, IntPtr inNotificationFunc, void* inNotificationRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ShareDataPtr);
            int result;
            IL.Push(inDataName);
            IL.Push(inDataType);
            IL.Push(inNotificationFunc);
            IL.Push(inNotificationRefcon);
            IL.Push(ShareDataPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte*), typeof(DataTypeID), typeof(IntPtr), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine connects a plug-in to shared data, creating the shared data if
        /// necessary. inDataName is a standard path for the data ref, and inDataType
        /// specifies the type. This function will create the data if it does not
        /// exist. If the data already exists but the type does not match, an error is
        /// returned, so it is important that plug-in authors collaborate to establish
        /// public standards for shared data.
        /// </para>
        /// <para>
        /// If a notificationFunc is passed in and is not NULL, that notification
        /// function will be called whenever the data is modified. The notification
        /// refcon will be passed to it. This allows your plug-in to know which shared
        /// data was changed if multiple shared data are handled by one callback, or if
        /// the plug-in does not use global variables.
        /// </para>
        /// <para>
        /// A one is returned for successfully creating or finding the shared data; a
        /// zero if the data already exists but is of the wrong type.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int ShareData(byte* inDataName, DataTypeID inDataType, DataChangedCallback inNotificationFunc, void* inNotificationRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inNotificationFuncPtr = inNotificationFunc != null ? Marshal.GetFunctionPointerForDelegate(inNotificationFunc) : default;
            int result = ShareDataPrivate(inDataName, inDataType, inNotificationFuncPtr, inNotificationRefcon);
            GC.KeepAlive(inNotificationFunc);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine connects a plug-in to shared data, creating the shared data if
        /// necessary. inDataName is a standard path for the data ref, and inDataType
        /// specifies the type. This function will create the data if it does not
        /// exist. If the data already exists but the type does not match, an error is
        /// returned, so it is important that plug-in authors collaborate to establish
        /// public standards for shared data.
        /// </para>
        /// <para>
        /// If a notificationFunc is passed in and is not NULL, that notification
        /// function will be called whenever the data is modified. The notification
        /// refcon will be passed to it. This allows your plug-in to know which shared
        /// data was changed if multiple shared data are handled by one callback, or if
        /// the plug-in does not use global variables.
        /// </para>
        /// <para>
        /// A one is returned for successfully creating or finding the shared data; a
        /// zero if the data already exists but is of the wrong type.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int ShareData(in ReadOnlySpan<char> inDataName, DataTypeID inDataType, DataChangedCallback inNotificationFunc, void* inNotificationRefcon)
        {
            IL.DeclareLocals(false);
            Span<byte> inDataNameUtf8 = stackalloc byte[(inDataName.Length << 1) | 1];
            var inDataNamePtr = Utils.ToUtf8Unsafe(inDataName, inDataNameUtf8);
            return ShareData(inDataNamePtr, inDataType, inNotificationFunc, inNotificationRefcon);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe int UnshareDataPrivate(byte* inDataName, DataTypeID inDataType, IntPtr inNotificationFunc, void* inNotificationRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(UnshareDataPtr);
            int result;
            IL.Push(inDataName);
            IL.Push(inDataType);
            IL.Push(inNotificationFunc);
            IL.Push(inNotificationRefcon);
            IL.Push(UnshareDataPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte*), typeof(DataTypeID), typeof(IntPtr), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine removes your notification function for shared data. Call it
        /// when done with the data to stop receiving change notifications. Arguments
        /// must match XPLMShareData. The actual memory will not necessarily be freed,
        /// since other plug-ins could be using it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int UnshareData(byte* inDataName, DataTypeID inDataType, DataChangedCallback inNotificationFunc, void* inNotificationRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inNotificationFuncPtr = inNotificationFunc != null ? Marshal.GetFunctionPointerForDelegate(inNotificationFunc) : default;
            int result = UnshareDataPrivate(inDataName, inDataType, inNotificationFuncPtr, inNotificationRefcon);
            GC.KeepAlive(inNotificationFunc);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine removes your notification function for shared data. Call it
        /// when done with the data to stop receiving change notifications. Arguments
        /// must match XPLMShareData. The actual memory will not necessarily be freed,
        /// since other plug-ins could be using it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int UnshareData(in ReadOnlySpan<char> inDataName, DataTypeID inDataType, DataChangedCallback inNotificationFunc, void* inNotificationRefcon)
        {
            IL.DeclareLocals(false);
            Span<byte> inDataNameUtf8 = stackalloc byte[(inDataName.Length << 1) | 1];
            var inDataNamePtr = Utils.ToUtf8Unsafe(inDataName, inDataNameUtf8);
            return UnshareData(inDataNamePtr, inDataType, inNotificationFunc, inNotificationRefcon);
        }
    }
}