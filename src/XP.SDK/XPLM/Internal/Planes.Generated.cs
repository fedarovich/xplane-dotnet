using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

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
            SetUsersAircraftPtr = Lib.GetExport("XPLMSetUsersAircraft");
            PlaceUserAtAirportPtr = Lib.GetExport("XPLMPlaceUserAtAirport");
            PlaceUserAtLocationPtr = Lib.GetExport("XPLMPlaceUserAtLocation");
            CountAircraftPtr = Lib.GetExport("XPLMCountAircraft");
            GetNthAircraftModelPtr = Lib.GetExport("XPLMGetNthAircraftModel");
            AcquirePlanesPtr = Lib.GetExport("XPLMAcquirePlanes");
            ReleasePlanesPtr = Lib.GetExport("XPLMReleasePlanes");
            SetActiveAircraftCountPtr = Lib.GetExport("XPLMSetActiveAircraftCount");
            SetAircraftModelPtr = Lib.GetExport("XPLMSetAircraftModel");
            DisableAIForPlanePtr = Lib.GetExport("XPLMDisableAIForPlane");
            DrawAircraftPtr = Lib.GetExport("XPLMDrawAircraft");
            ReinitUsersPlanePtr = Lib.GetExport("XPLMReinitUsersPlane");
        }

        
        /// <summary>
        /// <para>
        /// This routine changes the user's aircraft.  Note that this will reinitialize
        /// the user to be on the nearest airport's first runway.  Pass in a full path
        /// (hard drive and everything including the .acf extension) to the .acf file.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetUsersAircraft(byte* inAircraftPath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetUsersAircraftPtr);
            IL.Push(inAircraftPath);
            IL.Push(SetUsersAircraftPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine changes the user's aircraft.  Note that this will reinitialize
        /// the user to be on the nearest airport's first runway.  Pass in a full path
        /// (hard drive and everything including the .acf extension) to the .acf file.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetUsersAircraft(in ReadOnlySpan<char> inAircraftPath)
        {
            IL.DeclareLocals(false);
            Span<byte> inAircraftPathUtf8 = stackalloc byte[(inAircraftPath.Length << 1) | 1];
            var inAircraftPathPtr = Utils.ToUtf8Unsafe(inAircraftPath, inAircraftPathUtf8);
            SetUsersAircraft(inAircraftPathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine places the user at a given airport.  Specify the airport by
        /// its ICAO code (e.g. 'KBOS').
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void PlaceUserAtAirport(byte* inAirportCode)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(PlaceUserAtAirportPtr);
            IL.Push(inAirportCode);
            IL.Push(PlaceUserAtAirportPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine places the user at a given airport.  Specify the airport by
        /// its ICAO code (e.g. 'KBOS').
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void PlaceUserAtAirport(in ReadOnlySpan<char> inAirportCode)
        {
            IL.DeclareLocals(false);
            Span<byte> inAirportCodeUtf8 = stackalloc byte[(inAirportCode.Length << 1) | 1];
            var inAirportCodePtr = Utils.ToUtf8Unsafe(inAirportCode, inAirportCodeUtf8);
            PlaceUserAtAirport(inAirportCodePtr);
        }

        
        /// <summary>
        /// <para>
        /// Places the user at a specific location after performing any necessary
        /// scenery loads.
        /// </para>
        /// <para>
        /// As with in-air starts initiated from the X-Plane user interface, the
        /// aircraft will always start with its engines running, regardless of the
        /// user's preferences (i.e., regardless of what the dataref
        /// sim/operation/prefs/startup_running says).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void PlaceUserAtLocation(double latitudeDegrees, double longitudeDegrees, float elevationMetersMSL, float headingDegreesTrue, float speedMetersPerSecond)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(PlaceUserAtLocationPtr);
            IL.Push(latitudeDegrees);
            IL.Push(longitudeDegrees);
            IL.Push(elevationMetersMSL);
            IL.Push(headingDegreesTrue);
            IL.Push(speedMetersPerSecond);
            IL.Push(PlaceUserAtLocationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(double), typeof(double), typeof(float), typeof(float), typeof(float)));
        }

        
        /// <summary>
        /// <para>
        /// This function returns the number of aircraft X-Plane is capable of having,
        /// as well as the number of aircraft that are currently active.  These numbers
        /// count the user's aircraft.  It can also return the plugin that is currently
        /// controlling aircraft.  In X-Plane 7, this routine reflects the number of
        /// aircraft the user has enabled in the rendering options window.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CountAircraft(int* outTotalAircraft, int* outActiveAircraft, PluginID* outController)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CountAircraftPtr);
            IL.Push(outTotalAircraft);
            IL.Push(outActiveAircraft);
            IL.Push(outController);
            IL.Push(CountAircraftPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int*), typeof(int*), typeof(PluginID*)));
        }

        
        /// <summary>
        /// <para>
        /// This function returns the aircraft model for the Nth aircraft.  Indices are
        /// zero based, with zero being the user's aircraft.  The file name should be
        /// at least 256 chars in length; the path should be at least 512 chars in
        /// length.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetNthAircraftModel(int inIndex, byte* outFileName, byte* outPath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetNthAircraftModelPtr);
            IL.Push(inIndex);
            IL.Push(outFileName);
            IL.Push(outPath);
            IL.Push(GetNthAircraftModelPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(byte*), typeof(byte*)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe int AcquirePlanesPrivate(byte** inAircraft, IntPtr inCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(AcquirePlanesPtr);
            int result;
            IL.Push(inAircraft);
            IL.Push(inCallback);
            IL.Push(inRefcon);
            IL.Push(AcquirePlanesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte**), typeof(PlanesAvailableCallback), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// XPLMAcquirePlanes grants your plugin exclusive access to the aircraft.  It
        /// returns 1 if you gain access, 0 if you do not. inAircraft - pass in an
        /// array of pointers to strings specifying the planes you want loaded.  For
        /// any plane index you do not want loaded, pass a 0-length string.  Other
        /// strings should be full paths with the .acf extension.  NULL terminates this
        /// array, or pass NULL if there are no planes you want loaded. If you pass in
        /// a callback and do not receive access to the planes your callback will be
        /// called when the airplanes are available. If you do receive airplane access,
        /// your callback will not be called.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int AcquirePlanes(byte** inAircraft, PlanesAvailableCallback inCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            int result = AcquirePlanesPrivate(inAircraft, inCallbackPtr, inRefcon);
            GC.KeepAlive(inCallbackPtr);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Call this function to release access to the planes.  Note that if you are
        /// disabled, access to planes is released for you and you must reacquire it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ReleasePlanes()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ReleasePlanesPtr);
            IL.Push(ReleasePlanesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }

        
        /// <summary>
        /// <para>
        /// This routine sets the number of active planes.  If you pass in a number
        /// higher than the total number of planes availables, only the total number of
        /// planes available is actually used.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveAircraftCount(int inCount)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetActiveAircraftCountPtr);
            IL.Push(inCount);
            IL.Push(SetActiveAircraftCountPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine loads an aircraft model.  It may only be called if you  have
        /// exclusive access to the airplane APIs.  Pass in the path of the  model with
        /// the .acf extension.  The index is zero based, but you  may not pass in 0
        /// (use XPLMSetUsersAircraft to load the user's aircracft).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetAircraftModel(int inIndex, byte* inAircraftPath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetAircraftModelPtr);
            IL.Push(inIndex);
            IL.Push(inAircraftPath);
            IL.Push(SetAircraftModelPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine loads an aircraft model.  It may only be called if you  have
        /// exclusive access to the airplane APIs.  Pass in the path of the  model with
        /// the .acf extension.  The index is zero based, but you  may not pass in 0
        /// (use XPLMSetUsersAircraft to load the user's aircracft).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetAircraftModel(int inIndex, in ReadOnlySpan<char> inAircraftPath)
        {
            IL.DeclareLocals(false);
            Span<byte> inAircraftPathUtf8 = stackalloc byte[(inAircraftPath.Length << 1) | 1];
            var inAircraftPathPtr = Utils.ToUtf8Unsafe(inAircraftPath, inAircraftPathUtf8);
            SetAircraftModel(inIndex, inAircraftPathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine turns off X-Plane's AI for a given plane.  The plane will
        /// continue to draw and be a real plane in X-Plane, but will not  move itself.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DisableAIForPlane(int inPlaneIndex)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DisableAIForPlanePtr);
            IL.Push(inPlaneIndex);
            IL.Push(DisableAIForPlanePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine draws an aircraft.  It can only be called from a 3-d drawing
        /// callback.  Pass in the position of the plane in OpenGL local coordinates
        /// and the orientation of the plane.  A 1 for full drawing indicates that the
        /// whole plane must be drawn; a 0 indicates you only need the nav lights
        /// drawn. (This saves rendering time when planes are far away.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawAircraft(int inPlaneIndex, float inX, float inY, float inZ, float inPitch, float inRoll, float inYaw, int inFullDraw, PlaneDrawState* inDrawStateInfo)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DrawAircraftPtr);
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

        
        /// <summary>
        /// <para>
        /// This function recomputes the derived flight model data from the aircraft
        /// structure in memory.  If you have used the data access layer to modify the
        /// aircraft structure, use this routine to resynchronize X-Plane; since
        /// X-Plane works at least partly from derived values, the sim will not behave
        /// properly until this is called.
        /// </para>
        /// <para>
        /// WARNING: this routine does not necessarily place the airplane at the
        /// airport; use XPLMSetUsersAircraft to be compatible.  This routine is
        /// provided to do special experimentation with flight models without resetting
        /// flight.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ReinitUsersPlane()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ReinitUsersPlanePtr);
            IL.Push(ReinitUsersPlanePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }
    }
}