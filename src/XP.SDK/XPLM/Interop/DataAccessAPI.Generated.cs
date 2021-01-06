using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Interop
{
    public static partial class DataAccessAPI
    {
        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMFindDataRef", ExactSpelling = true)]
        public static extern unsafe DataRef FindDataRef(byte* inDataRefName);

        
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
        public static unsafe DataRef FindDataRef(in XP.SDK.Utf8String inDataRefName)
        {
            fixed (byte* inDataRefNamePtr = inDataRefName)
                return FindDataRef(inDataRefNamePtr);
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
        [SkipLocalsInitAttribute]
        public static unsafe DataRef FindDataRef(in ReadOnlySpan<char> inDataRefName)
        {
            int inDataRefNameUtf8Len = inDataRefName.Length * 3 + 4;
            Span<byte> inDataRefNameUtf8 = inDataRefNameUtf8Len <= 4096 ? stackalloc byte[inDataRefNameUtf8Len] : GC.AllocateUninitializedArray<byte>(inDataRefNameUtf8Len);
            var inDataRefNameUtf8Str = Utf8String.FromUtf16Unsafe(inDataRefName, inDataRefNameUtf8);
            return FindDataRef(inDataRefNameUtf8Str);
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCanWriteDataRef", ExactSpelling = true)]
        public static extern int CanWriteDataRef(DataRef inDataRef);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMIsDataRefGood", ExactSpelling = true)]
        public static extern int IsDataRefGood(DataRef inDataRef);

        
        /// <summary>
        /// <para>
        /// This routine returns the types of the data ref for accessor use. If a data
        /// ref is available in multiple data types, the bit-wise OR of these types
        /// will be returned.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDataRefTypes", ExactSpelling = true)]
        public static extern DataTypeID GetDataRefTypes(DataRef inDataRef);

        
        /// <summary>
        /// <para>
        /// Read an integer data ref and return its value. The return value is the
        /// dataref value or 0 if the dataref is NULL or the plugin is disabled.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDatai", ExactSpelling = true)]
        public static extern int GetDatai(DataRef inDataRef);

        
        /// <summary>
        /// <para>
        /// Write a new value to an integer data ref. This routine is a no-op if the
        /// plugin publishing the dataref is disabled, the dataref is NULL, or the
        /// dataref is not writable.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetDatai", ExactSpelling = true)]
        public static extern void SetDatai(DataRef inDataRef, int inValue);

        
        /// <summary>
        /// <para>
        /// Read a single precision floating point dataref and return its value. The
        /// return value is the dataref value or 0.0 if the dataref is NULL or the
        /// plugin is disabled.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDataf", ExactSpelling = true)]
        public static extern float GetDataf(DataRef inDataRef);

        
        /// <summary>
        /// <para>
        /// Write a new value to a single precision floating point data ref. This
        /// routine is a no-op if the plugin publishing the dataref is disabled, the
        /// dataref is NULL, or the dataref is not writable.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetDataf", ExactSpelling = true)]
        public static extern void SetDataf(DataRef inDataRef, float inValue);

        
        /// <summary>
        /// <para>
        /// Read a double precision floating point dataref and return its value. The
        /// return value is the dataref value or 0.0 if the dataref is NULL or the
        /// plugin is disabled.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDatad", ExactSpelling = true)]
        public static extern double GetDatad(DataRef inDataRef);

        
        /// <summary>
        /// <para>
        /// Write a new value to a double precision floating point data ref. This
        /// routine is a no-op if the plugin publishing the dataref is disabled, the
        /// dataref is NULL, or the dataref is not writable.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetDatad", ExactSpelling = true)]
        public static extern void SetDatad(DataRef inDataRef, double inValue);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDatavi", ExactSpelling = true)]
        public static extern unsafe int GetDatavi(DataRef inDataRef, int* outValues, int inOffset, int inMax);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetDatavi", ExactSpelling = true)]
        public static extern unsafe void SetDatavi(DataRef inDataRef, int* inValues, int inoffset, int inCount);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDatavf", ExactSpelling = true)]
        public static extern unsafe int GetDatavf(DataRef inDataRef, float* outValues, int inOffset, int inMax);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetDatavf", ExactSpelling = true)]
        public static extern unsafe void SetDatavf(DataRef inDataRef, float* inValues, int inoffset, int inCount);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDatab", ExactSpelling = true)]
        public static extern unsafe int GetDatab(DataRef inDataRef, void* outValue, int inOffset, int inMaxBytes);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetDatab", ExactSpelling = true)]
        public static extern unsafe void SetDatab(DataRef inDataRef, void* inValue, int inOffset, int inLength);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMRegisterDataAccessor", ExactSpelling = true)]
        public static extern unsafe DataRef RegisterDataAccessor(byte* inDataName, DataTypeID inDataType, int inIsWritable, delegate* unmanaged<void*, int> inReadInt, delegate* unmanaged<void*, int, void> inWriteInt, delegate* unmanaged<void*, float> inReadFloat, delegate* unmanaged<void*, float, void> inWriteFloat, delegate* unmanaged<void*, double> inReadDouble, delegate* unmanaged<void*, double, void> inWriteDouble, delegate* unmanaged<void*, int*, int, int, int> inReadIntArray, delegate* unmanaged<void*, int*, int, int, void> inWriteIntArray, delegate* unmanaged<void*, float*, int, int, int> inReadFloatArray, delegate* unmanaged<void*, float*, int, int, void> inWriteFloatArray, delegate* unmanaged<void*, void*, int, int, int> inReadData, delegate* unmanaged<void*, void*, int, int, void> inWriteData, void* inReadRefcon, void* inWriteRefcon);

        
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
        public static unsafe DataRef RegisterDataAccessor(in XP.SDK.Utf8String inDataName, DataTypeID inDataType, int inIsWritable, delegate* unmanaged<void*, int> inReadInt, delegate* unmanaged<void*, int, void> inWriteInt, delegate* unmanaged<void*, float> inReadFloat, delegate* unmanaged<void*, float, void> inWriteFloat, delegate* unmanaged<void*, double> inReadDouble, delegate* unmanaged<void*, double, void> inWriteDouble, delegate* unmanaged<void*, int*, int, int, int> inReadIntArray, delegate* unmanaged<void*, int*, int, int, void> inWriteIntArray, delegate* unmanaged<void*, float*, int, int, int> inReadFloatArray, delegate* unmanaged<void*, float*, int, int, void> inWriteFloatArray, delegate* unmanaged<void*, void*, int, int, int> inReadData, delegate* unmanaged<void*, void*, int, int, void> inWriteData, void* inReadRefcon, void* inWriteRefcon)
        {
            fixed (byte* inDataNamePtr = inDataName)
                return RegisterDataAccessor(inDataNamePtr, inDataType, inIsWritable, inReadInt, inWriteInt, inReadFloat, inWriteFloat, inReadDouble, inWriteDouble, inReadIntArray, inWriteIntArray, inReadFloatArray, inWriteFloatArray, inReadData, inWriteData, inReadRefcon, inWriteRefcon);
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
        [SkipLocalsInitAttribute]
        public static unsafe DataRef RegisterDataAccessor(in ReadOnlySpan<char> inDataName, DataTypeID inDataType, int inIsWritable, delegate* unmanaged<void*, int> inReadInt, delegate* unmanaged<void*, int, void> inWriteInt, delegate* unmanaged<void*, float> inReadFloat, delegate* unmanaged<void*, float, void> inWriteFloat, delegate* unmanaged<void*, double> inReadDouble, delegate* unmanaged<void*, double, void> inWriteDouble, delegate* unmanaged<void*, int*, int, int, int> inReadIntArray, delegate* unmanaged<void*, int*, int, int, void> inWriteIntArray, delegate* unmanaged<void*, float*, int, int, int> inReadFloatArray, delegate* unmanaged<void*, float*, int, int, void> inWriteFloatArray, delegate* unmanaged<void*, void*, int, int, int> inReadData, delegate* unmanaged<void*, void*, int, int, void> inWriteData, void* inReadRefcon, void* inWriteRefcon)
        {
            int inDataNameUtf8Len = inDataName.Length * 3 + 4;
            Span<byte> inDataNameUtf8 = inDataNameUtf8Len <= 4096 ? stackalloc byte[inDataNameUtf8Len] : GC.AllocateUninitializedArray<byte>(inDataNameUtf8Len);
            var inDataNameUtf8Str = Utf8String.FromUtf16Unsafe(inDataName, inDataNameUtf8);
            return RegisterDataAccessor(inDataNameUtf8Str, inDataType, inIsWritable, inReadInt, inWriteInt, inReadFloat, inWriteFloat, inReadDouble, inWriteDouble, inReadIntArray, inWriteIntArray, inReadFloatArray, inWriteFloatArray, inReadData, inWriteData, inReadRefcon, inWriteRefcon);
        }

        
        /// <summary>
        /// <para>
        /// Use this routine to unregister any data accessors you may have registered.
        /// You unregister a data ref by the XPLMDataRef you get back from
        /// registration. Once you unregister a data ref, your function pointer will
        /// not be called anymore.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMUnregisterDataAccessor", ExactSpelling = true)]
        public static extern void UnregisterDataAccessor(DataRef inDataRef);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMShareData", ExactSpelling = true)]
        public static extern unsafe int ShareData(byte* inDataName, DataTypeID inDataType, delegate* unmanaged<void*, void> inNotificationFunc, void* inNotificationRefcon);

        
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
        public static unsafe int ShareData(in XP.SDK.Utf8String inDataName, DataTypeID inDataType, delegate* unmanaged<void*, void> inNotificationFunc, void* inNotificationRefcon)
        {
            fixed (byte* inDataNamePtr = inDataName)
                return ShareData(inDataNamePtr, inDataType, inNotificationFunc, inNotificationRefcon);
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
        [SkipLocalsInitAttribute]
        public static unsafe int ShareData(in ReadOnlySpan<char> inDataName, DataTypeID inDataType, delegate* unmanaged<void*, void> inNotificationFunc, void* inNotificationRefcon)
        {
            int inDataNameUtf8Len = inDataName.Length * 3 + 4;
            Span<byte> inDataNameUtf8 = inDataNameUtf8Len <= 4096 ? stackalloc byte[inDataNameUtf8Len] : GC.AllocateUninitializedArray<byte>(inDataNameUtf8Len);
            var inDataNameUtf8Str = Utf8String.FromUtf16Unsafe(inDataName, inDataNameUtf8);
            return ShareData(inDataNameUtf8Str, inDataType, inNotificationFunc, inNotificationRefcon);
        }

        
        /// <summary>
        /// <para>
        /// This routine removes your notification function for shared data. Call it
        /// when done with the data to stop receiving change notifications. Arguments
        /// must match XPLMShareData. The actual memory will not necessarily be freed,
        /// since other plug-ins could be using it.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMUnshareData", ExactSpelling = true)]
        public static extern unsafe int UnshareData(byte* inDataName, DataTypeID inDataType, delegate* unmanaged<void*, void> inNotificationFunc, void* inNotificationRefcon);

        
        /// <summary>
        /// <para>
        /// This routine removes your notification function for shared data. Call it
        /// when done with the data to stop receiving change notifications. Arguments
        /// must match XPLMShareData. The actual memory will not necessarily be freed,
        /// since other plug-ins could be using it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int UnshareData(in XP.SDK.Utf8String inDataName, DataTypeID inDataType, delegate* unmanaged<void*, void> inNotificationFunc, void* inNotificationRefcon)
        {
            fixed (byte* inDataNamePtr = inDataName)
                return UnshareData(inDataNamePtr, inDataType, inNotificationFunc, inNotificationRefcon);
        }

        
        /// <summary>
        /// <para>
        /// This routine removes your notification function for shared data. Call it
        /// when done with the data to stop receiving change notifications. Arguments
        /// must match XPLMShareData. The actual memory will not necessarily be freed,
        /// since other plug-ins could be using it.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        public static unsafe int UnshareData(in ReadOnlySpan<char> inDataName, DataTypeID inDataType, delegate* unmanaged<void*, void> inNotificationFunc, void* inNotificationRefcon)
        {
            int inDataNameUtf8Len = inDataName.Length * 3 + 4;
            Span<byte> inDataNameUtf8 = inDataNameUtf8Len <= 4096 ? stackalloc byte[inDataNameUtf8Len] : GC.AllocateUninitializedArray<byte>(inDataNameUtf8Len);
            var inDataNameUtf8Str = Utf8String.FromUtf16Unsafe(inDataName, inDataNameUtf8);
            return UnshareData(inDataNameUtf8Str, inDataType, inNotificationFunc, inNotificationRefcon);
        }
    }
}