using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class DataAccess
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
        static DataAccess()
        {
            const string libraryName = "XPLM";
            FindDataRefPtr = FunctionResolver.Resolve(libraryName, "XPLMFindDataRef");
            CanWriteDataRefPtr = FunctionResolver.Resolve(libraryName, "XPLMCanWriteDataRef");
            IsDataRefGoodPtr = FunctionResolver.Resolve(libraryName, "XPLMIsDataRefGood");
            GetDataRefTypesPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDataRefTypes");
            GetDataiPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDatai");
            SetDataiPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDatai");
            GetDatafPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDataf");
            SetDatafPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDataf");
            GetDatadPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDatad");
            SetDatadPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDatad");
            GetDataviPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDatavi");
            SetDataviPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDatavi");
            GetDatavfPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDatavf");
            SetDatavfPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDatavf");
            GetDatabPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDatab");
            SetDatabPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDatab");
            RegisterDataAccessorPtr = FunctionResolver.Resolve(libraryName, "XPLMRegisterDataAccessor");
            UnregisterDataAccessorPtr = FunctionResolver.Resolve(libraryName, "XPLMUnregisterDataAccessor");
            ShareDataPtr = FunctionResolver.Resolve(libraryName, "XPLMShareData");
            UnshareDataPtr = FunctionResolver.Resolve(libraryName, "XPLMUnshareData");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe DataRef FindDataRef(byte *inDataRefName)
        {
            IL.DeclareLocals(false);
            DataRef result;
            IL.Push(inDataRefName);
            IL.Push(FindDataRefPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(DataRef), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CanWriteDataRef(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inDataRef);
            IL.Push(CanWriteDataRefPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsDataRefGood(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inDataRef);
            IL.Push(IsDataRefGoodPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static DataTypeID GetDataRefTypes(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            DataTypeID result;
            IL.Push(inDataRef);
            IL.Push(GetDataRefTypesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(DataTypeID), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetDatai(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inDataRef);
            IL.Push(GetDataiPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDatai(DataRef inDataRef, int inValue)
        {
            IL.DeclareLocals(false);
            IL.Push(inDataRef);
            IL.Push(inValue);
            IL.Push(SetDataiPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float GetDataf(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            float result;
            IL.Push(inDataRef);
            IL.Push(GetDatafPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDataf(DataRef inDataRef, float inValue)
        {
            IL.DeclareLocals(false);
            IL.Push(inDataRef);
            IL.Push(inValue);
            IL.Push(SetDatafPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(float)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static double GetDatad(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            double result;
            IL.Push(inDataRef);
            IL.Push(GetDatadPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(double), typeof(DataRef)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDatad(DataRef inDataRef, double inValue)
        {
            IL.DeclareLocals(false);
            IL.Push(inDataRef);
            IL.Push(inValue);
            IL.Push(SetDatadPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(double)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDatavi(DataRef inDataRef, int *outValues, int inOffset, int inMax)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inDataRef);
            IL.Push(outValues);
            IL.Push(inOffset);
            IL.Push(inMax);
            IL.Push(GetDataviPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef), typeof(int *), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetDatavi(DataRef inDataRef, int *inValues, int inoffset, int inCount)
        {
            IL.DeclareLocals(false);
            IL.Push(inDataRef);
            IL.Push(inValues);
            IL.Push(inoffset);
            IL.Push(inCount);
            IL.Push(SetDataviPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(int *), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDatavf(DataRef inDataRef, float *outValues, int inOffset, int inMax)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inDataRef);
            IL.Push(outValues);
            IL.Push(inOffset);
            IL.Push(inMax);
            IL.Push(GetDatavfPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef), typeof(float *), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetDatavf(DataRef inDataRef, float *inValues, int inoffset, int inCount)
        {
            IL.DeclareLocals(false);
            IL.Push(inDataRef);
            IL.Push(inValues);
            IL.Push(inoffset);
            IL.Push(inCount);
            IL.Push(SetDatavfPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(float *), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetDatab(DataRef inDataRef, void *outValue, int inOffset, int inMaxBytes)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inDataRef);
            IL.Push(outValue);
            IL.Push(inOffset);
            IL.Push(inMaxBytes);
            IL.Push(GetDatabPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(DataRef), typeof(void *), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetDatab(DataRef inDataRef, void *inValue, int inOffset, int inLength)
        {
            IL.DeclareLocals(false);
            IL.Push(inDataRef);
            IL.Push(inValue);
            IL.Push(inOffset);
            IL.Push(inLength);
            IL.Push(SetDatabPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef), typeof(void *), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe DataRef RegisterDataAccessor(byte *inDataName, DataTypeID inDataType, int inIsWritable, GetDataiCallback inReadInt, SetDataiCallback inWriteInt, GetDatafCallback inReadFloat, SetDatafCallback inWriteFloat, GetDatadCallback inReadDouble, SetDatadCallback inWriteDouble, GetDataviCallback inReadIntArray, SetDataviCallback inWriteIntArray, GetDatavfCallback inReadFloatArray, SetDatavfCallback inWriteFloatArray, GetDatabCallback inReadData, SetDatabCallback inWriteData, void *inReadRefcon, void *inWriteRefcon)
        {
            IL.DeclareLocals(false);
            DataRef result;
            IntPtr inReadIntPtr = Marshal.GetFunctionPointerForDelegate(inReadInt);
            IntPtr inWriteIntPtr = Marshal.GetFunctionPointerForDelegate(inWriteInt);
            IntPtr inReadFloatPtr = Marshal.GetFunctionPointerForDelegate(inReadFloat);
            IntPtr inWriteFloatPtr = Marshal.GetFunctionPointerForDelegate(inWriteFloat);
            IntPtr inReadDoublePtr = Marshal.GetFunctionPointerForDelegate(inReadDouble);
            IntPtr inWriteDoublePtr = Marshal.GetFunctionPointerForDelegate(inWriteDouble);
            IntPtr inReadIntArrayPtr = Marshal.GetFunctionPointerForDelegate(inReadIntArray);
            IntPtr inWriteIntArrayPtr = Marshal.GetFunctionPointerForDelegate(inWriteIntArray);
            IntPtr inReadFloatArrayPtr = Marshal.GetFunctionPointerForDelegate(inReadFloatArray);
            IntPtr inWriteFloatArrayPtr = Marshal.GetFunctionPointerForDelegate(inWriteFloatArray);
            IntPtr inReadDataPtr = Marshal.GetFunctionPointerForDelegate(inReadData);
            IntPtr inWriteDataPtr = Marshal.GetFunctionPointerForDelegate(inWriteData);
            IL.Push(inDataName);
            IL.Push(inDataType);
            IL.Push(inIsWritable);
            IL.Push(inReadIntPtr);
            IL.Push(inWriteIntPtr);
            IL.Push(inReadFloatPtr);
            IL.Push(inWriteFloatPtr);
            IL.Push(inReadDoublePtr);
            IL.Push(inWriteDoublePtr);
            IL.Push(inReadIntArrayPtr);
            IL.Push(inWriteIntArrayPtr);
            IL.Push(inReadFloatArrayPtr);
            IL.Push(inWriteFloatArrayPtr);
            IL.Push(inReadDataPtr);
            IL.Push(inWriteDataPtr);
            IL.Push(inReadRefcon);
            IL.Push(inWriteRefcon);
            IL.Push(RegisterDataAccessorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(DataRef), typeof(byte *), typeof(DataTypeID), typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(void *), typeof(void *)));
            IL.Pop(out result);
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

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterDataAccessor(DataRef inDataRef)
        {
            IL.DeclareLocals(false);
            IL.Push(inDataRef);
            IL.Push(UnregisterDataAccessorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(DataRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int ShareData(byte *inDataName, DataTypeID inDataType, DataChangedCallback inNotificationFunc, void *inNotificationRefcon)
        {
            IL.DeclareLocals(false);
            int result;
            IntPtr inNotificationFuncPtr = Marshal.GetFunctionPointerForDelegate(inNotificationFunc);
            IL.Push(inDataName);
            IL.Push(inDataType);
            IL.Push(inNotificationFuncPtr);
            IL.Push(inNotificationRefcon);
            IL.Push(ShareDataPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte *), typeof(DataTypeID), typeof(IntPtr), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inNotificationFunc);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int UnshareData(byte *inDataName, DataTypeID inDataType, DataChangedCallback inNotificationFunc, void *inNotificationRefcon)
        {
            IL.DeclareLocals(false);
            int result;
            IntPtr inNotificationFuncPtr = Marshal.GetFunctionPointerForDelegate(inNotificationFunc);
            IL.Push(inDataName);
            IL.Push(inDataType);
            IL.Push(inNotificationFuncPtr);
            IL.Push(inNotificationRefcon);
            IL.Push(UnshareDataPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte *), typeof(DataTypeID), typeof(IntPtr), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inNotificationFunc);
            return result;
        }
    }
}