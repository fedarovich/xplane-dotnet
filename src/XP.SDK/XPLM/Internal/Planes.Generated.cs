using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Planes
    {
        private static IntPtr SetUsersAircraftPtr;
        private static IntPtr PlaceUserAtAirportPtr;
        private static IntPtr PlaceUserAtLocationPtr;
        private static IntPtr CountAircraftPtr;
        private static IntPtr GetNthAircraftModelPtr;
        private static IntPtr AcquirePlanesPtr;
        private static IntPtr ReleasePlanesPtr;
        private static IntPtr SetActiveAircraftCountPtr;
        private static IntPtr SetAircraftModelPtr;
        private static IntPtr DisableAIForPlanePtr;
        private static IntPtr DrawAircraftPtr;
        private static IntPtr ReinitUsersPlanePtr;
        static Planes()
        {
            const string libraryName = "XPLM";
            SetUsersAircraftPtr = FunctionResolver.Resolve(libraryName, "XPLMSetUsersAircraft");
            PlaceUserAtAirportPtr = FunctionResolver.Resolve(libraryName, "XPLMPlaceUserAtAirport");
            PlaceUserAtLocationPtr = FunctionResolver.Resolve(libraryName, "XPLMPlaceUserAtLocation");
            CountAircraftPtr = FunctionResolver.Resolve(libraryName, "XPLMCountAircraft");
            GetNthAircraftModelPtr = FunctionResolver.Resolve(libraryName, "XPLMGetNthAircraftModel");
            AcquirePlanesPtr = FunctionResolver.Resolve(libraryName, "XPLMAcquirePlanes");
            ReleasePlanesPtr = FunctionResolver.Resolve(libraryName, "XPLMReleasePlanes");
            SetActiveAircraftCountPtr = FunctionResolver.Resolve(libraryName, "XPLMSetActiveAircraftCount");
            SetAircraftModelPtr = FunctionResolver.Resolve(libraryName, "XPLMSetAircraftModel");
            DisableAIForPlanePtr = FunctionResolver.Resolve(libraryName, "XPLMDisableAIForPlane");
            DrawAircraftPtr = FunctionResolver.Resolve(libraryName, "XPLMDrawAircraft");
            ReinitUsersPlanePtr = FunctionResolver.Resolve(libraryName, "XPLMReinitUsersPlane");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetUsersAircraft(byte *inAircraftPath)
        {
            IL.DeclareLocals(false);
            IL.Push(inAircraftPath);
            IL.Push(SetUsersAircraftPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void PlaceUserAtAirport(byte *inAirportCode)
        {
            IL.DeclareLocals(false);
            IL.Push(inAirportCode);
            IL.Push(PlaceUserAtAirportPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void PlaceUserAtLocation(double latitudeDegrees, double longitudeDegrees, float elevationMetersMSL, float headingDegreesTrue, float speedMetersPerSecond)
        {
            IL.DeclareLocals(false);
            IL.Push(latitudeDegrees);
            IL.Push(longitudeDegrees);
            IL.Push(elevationMetersMSL);
            IL.Push(headingDegreesTrue);
            IL.Push(speedMetersPerSecond);
            IL.Push(PlaceUserAtLocationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(double), typeof(double), typeof(float), typeof(float), typeof(float)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CountAircraft(int *outTotalAircraft, int *outActiveAircraft, PluginID*outController)
        {
            IL.DeclareLocals(false);
            IL.Push(outTotalAircraft);
            IL.Push(outActiveAircraft);
            IL.Push(outController);
            IL.Push(CountAircraftPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int *), typeof(int *), typeof(PluginID*)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetNthAircraftModel(int inIndex, byte *outFileName, byte *outPath)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(outFileName);
            IL.Push(outPath);
            IL.Push(GetNthAircraftModelPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(byte *), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AcquirePlanes(byte **inAircraft, PlanesAvailableCallback inCallback, void *inRefcon)
        {
            IL.DeclareLocals(false);
            int result;
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inAircraft);
            IL.Push(inCallbackPtr);
            IL.Push(inRefcon);
            IL.Push(AcquirePlanesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte **), typeof(IntPtr), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inCallback);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ReleasePlanes()
        {
            IL.DeclareLocals(false);
            IL.Push(ReleasePlanesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveAircraftCount(int inCount)
        {
            IL.DeclareLocals(false);
            IL.Push(inCount);
            IL.Push(SetActiveAircraftCountPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetAircraftModel(int inIndex, byte *inAircraftPath)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(inAircraftPath);
            IL.Push(SetAircraftModelPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DisableAIForPlane(int inPlaneIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inPlaneIndex);
            IL.Push(DisableAIForPlanePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawAircraft(int inPlaneIndex, float inX, float inY, float inZ, float inPitch, float inRoll, float inYaw, int inFullDraw, PlaneDrawState*inDrawStateInfo)
        {
            IL.DeclareLocals(false);
            IL.Push(inPlaneIndex);
            IL.Push(inX);
            IL.Push(inY);
            IL.Push(inZ);
            IL.Push(inPitch);
            IL.Push(inRoll);
            IL.Push(inYaw);
            IL.Push(inFullDraw);
            IL.Push(inDrawStateInfo);
            IL.Push(DrawAircraftPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(int), typeof(PlaneDrawState*)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ReinitUsersPlane()
        {
            IL.DeclareLocals(false);
            IL.Push(ReinitUsersPlanePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }
    }
}