using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Scenery
    {
        private static IntPtr CreateProbePtr;
        private static IntPtr DestroyProbePtr;
        private static IntPtr ProbeTerrainXYZPtr;
        private static IntPtr GetMagneticVariationPtr;
        private static IntPtr DegTrueToDegMagneticPtr;
        private static IntPtr DegMagneticToDegTruePtr;
        private static IntPtr LoadObjectPtr;
        private static IntPtr LoadObjectAsyncPtr;
        private static IntPtr DrawObjectsPtr;
        private static IntPtr UnloadObjectPtr;
        private static IntPtr LookupObjectsPtr;
        static Scenery()
        {
            const string libraryName = "XPLM";
            CreateProbePtr = FunctionResolver.Resolve(libraryName, "XPLMCreateProbe");
            DestroyProbePtr = FunctionResolver.Resolve(libraryName, "XPLMDestroyProbe");
            ProbeTerrainXYZPtr = FunctionResolver.Resolve(libraryName, "XPLMProbeTerrainXYZ");
            GetMagneticVariationPtr = FunctionResolver.Resolve(libraryName, "XPLMGetMagneticVariation");
            DegTrueToDegMagneticPtr = FunctionResolver.Resolve(libraryName, "XPLMDegTrueToDegMagnetic");
            DegMagneticToDegTruePtr = FunctionResolver.Resolve(libraryName, "XPLMDegMagneticToDegTrue");
            LoadObjectPtr = FunctionResolver.Resolve(libraryName, "XPLMLoadObject");
            LoadObjectAsyncPtr = FunctionResolver.Resolve(libraryName, "XPLMLoadObjectAsync");
            DrawObjectsPtr = FunctionResolver.Resolve(libraryName, "XPLMDrawObjects");
            UnloadObjectPtr = FunctionResolver.Resolve(libraryName, "XPLMUnloadObject");
            LookupObjectsPtr = FunctionResolver.Resolve(libraryName, "XPLMLookupObjects");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ProbeRef CreateProbe(ProbeType inProbeType)
        {
            IL.DeclareLocals(false);
            ProbeRef result;
            IL.Push(inProbeType);
            IL.Push(CreateProbePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ProbeRef), typeof(ProbeType)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyProbe(ProbeRef inProbe)
        {
            IL.DeclareLocals(false);
            IL.Push(inProbe);
            IL.Push(DestroyProbePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ProbeRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe ProbeResult ProbeTerrainXYZ(ProbeRef inProbe, float inX, float inY, float inZ, ProbeInfo*outInfo)
        {
            IL.DeclareLocals(false);
            ProbeResult result;
            IL.Push(inProbe);
            IL.Push(inX);
            IL.Push(inY);
            IL.Push(inZ);
            IL.Push(outInfo);
            IL.Push(ProbeTerrainXYZPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ProbeResult), typeof(ProbeRef), typeof(float), typeof(float), typeof(float), typeof(ProbeInfo*)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float GetMagneticVariation(double latitude, double longitude)
        {
            IL.DeclareLocals(false);
            float result;
            IL.Push(latitude);
            IL.Push(longitude);
            IL.Push(GetMagneticVariationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(double), typeof(double)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float DegTrueToDegMagnetic(float headingDegreesTrue)
        {
            IL.DeclareLocals(false);
            float result;
            IL.Push(headingDegreesTrue);
            IL.Push(DegTrueToDegMagneticPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(float)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float DegMagneticToDegTrue(float headingDegreesMagnetic)
        {
            IL.DeclareLocals(false);
            float result;
            IL.Push(headingDegreesMagnetic);
            IL.Push(DegMagneticToDegTruePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(float)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe ObjectRef LoadObject(byte *inPath)
        {
            IL.DeclareLocals(false);
            ObjectRef result;
            IL.Push(inPath);
            IL.Push(LoadObjectPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ObjectRef), typeof(byte *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void LoadObjectAsync(byte *inPath, ObjectLoadedCallback inCallback, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inPath);
            IL.Push(inCallbackPtr);
            IL.Push(inRefcon);
            IL.Push(LoadObjectAsyncPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte *), typeof(IntPtr), typeof(void *)));
            GC.KeepAlive(inCallback);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawObjects(ObjectRef inObject, int inCount, DrawInfo*inLocations, int lighting, int earth_relative)
        {
            IL.DeclareLocals(false);
            IL.Push(inObject);
            IL.Push(inCount);
            IL.Push(inLocations);
            IL.Push(lighting);
            IL.Push(earth_relative);
            IL.Push(DrawObjectsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ObjectRef), typeof(int), typeof(DrawInfo*), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void UnloadObject(ObjectRef inObject)
        {
            IL.DeclareLocals(false);
            IL.Push(inObject);
            IL.Push(UnloadObjectPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ObjectRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LookupObjects(byte *inPath, float inLatitude, float inLongitude, LibraryEnumeratorCallback enumerator, void *@ref)
        {
            IL.DeclareLocals(false);
            int result;
            IntPtr enumeratorPtr = Marshal.GetFunctionPointerForDelegate(enumerator);
            IL.Push(inPath);
            IL.Push(inLatitude);
            IL.Push(inLongitude);
            IL.Push(enumeratorPtr);
            IL.Push(@ref);
            IL.Push(LookupObjectsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte *), typeof(float), typeof(float), typeof(IntPtr), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(enumerator);
            return result;
        }
    }
}