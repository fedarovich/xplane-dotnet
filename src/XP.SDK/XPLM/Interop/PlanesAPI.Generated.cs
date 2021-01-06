using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Interop
{
    public static partial class PlanesAPI
    {
        
        /// <summary>
        /// <para>
        /// This routine changes the user's aircraft.  Note that this will reinitialize
        /// the user to be on the nearest airport's first runway.  Pass in a full path
        /// (hard drive and everything including the .acf extension) to the .acf file.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetUsersAircraft", ExactSpelling = true)]
        public static extern unsafe void SetUsersAircraft(byte* inAircraftPath);

        
        /// <summary>
        /// <para>
        /// This routine changes the user's aircraft.  Note that this will reinitialize
        /// the user to be on the nearest airport's first runway.  Pass in a full path
        /// (hard drive and everything including the .acf extension) to the .acf file.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetUsersAircraft(in ReadOnlySpan<char> inAircraftPath)
        {
            int inAircraftPathUtf8Len = inAircraftPath.Length * 3 + 4;
            Span<byte> inAircraftPathUtf8 = inAircraftPathUtf8Len <= 4096 ? stackalloc byte[inAircraftPathUtf8Len] : new byte[inAircraftPathUtf8Len];
            Utils.ToUtf8(inAircraftPath, inAircraftPathUtf8);
            fixed (byte* inAircraftPathPtr = inAircraftPathUtf8)
                SetUsersAircraft(inAircraftPathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine changes the user's aircraft.  Note that this will reinitialize
        /// the user to be on the nearest airport's first runway.  Pass in a full path
        /// (hard drive and everything including the .acf extension) to the .acf file.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetUsersAircraft(in XP.SDK.Utf8String inAircraftPath)
        {
            fixed (byte* inAircraftPathPtr = inAircraftPath)
                SetUsersAircraft(inAircraftPathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine places the user at a given airport.  Specify the airport by
        /// its X-Plane airport ID (e.g. 'KBOS').
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMPlaceUserAtAirport", ExactSpelling = true)]
        public static extern unsafe void PlaceUserAtAirport(byte* inAirportCode);

        
        /// <summary>
        /// <para>
        /// This routine places the user at a given airport.  Specify the airport by
        /// its X-Plane airport ID (e.g. 'KBOS').
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void PlaceUserAtAirport(in ReadOnlySpan<char> inAirportCode)
        {
            int inAirportCodeUtf8Len = inAirportCode.Length * 3 + 4;
            Span<byte> inAirportCodeUtf8 = inAirportCodeUtf8Len <= 4096 ? stackalloc byte[inAirportCodeUtf8Len] : new byte[inAirportCodeUtf8Len];
            Utils.ToUtf8(inAirportCode, inAirportCodeUtf8);
            fixed (byte* inAirportCodePtr = inAirportCodeUtf8)
                PlaceUserAtAirport(inAirportCodePtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine places the user at a given airport.  Specify the airport by
        /// its X-Plane airport ID (e.g. 'KBOS').
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void PlaceUserAtAirport(in XP.SDK.Utf8String inAirportCode)
        {
            fixed (byte* inAirportCodePtr = inAirportCode)
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
        /// `sim/operation/prefs/startup_running` says).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMPlaceUserAtLocation", ExactSpelling = true)]
        public static extern void PlaceUserAtLocation(double latitudeDegrees, double longitudeDegrees, float elevationMetersMSL, float headingDegreesTrue, float speedMetersPerSecond);

        
        /// <summary>
        /// <para>
        /// This function returns the number of aircraft X-Plane is capable of having,
        /// as well as the number of aircraft that are currently active.  These numbers
        /// count the user's aircraft.  It can also return the plugin that is currently
        /// controlling aircraft.  In X-Plane 7, this routine reflects the number of
        /// aircraft the user has enabled in the rendering options window.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCountAircraft", ExactSpelling = true)]
        public static extern unsafe void CountAircraft(int* outTotalAircraft, int* outActiveAircraft, PluginID* outController);

        
        /// <summary>
        /// <para>
        /// This function returns the aircraft model for the Nth aircraft.  Indices are
        /// zero based, with zero being the user's aircraft.  The file name should be
        /// at least 256 chars in length; the path should be at least 512 chars in
        /// length.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetNthAircraftModel", ExactSpelling = true)]
        public static extern unsafe void GetNthAircraftModel(int inIndex, byte* outFileName, byte* outPath);

        
        /// <summary>
        /// <para>
        /// XPLMAcquirePlanes grants your plugin exclusive access to the aircraft.  It
        /// returns 1 if you gain access, 0 if you do not.
        /// </para>
        /// <para>
        /// inAircraft - pass in an array of pointers to strings specifying the planes
        /// you want loaded.  For any plane index you do not want loaded, pass a
        /// 0-length string.  Other strings should be full paths with the .acf
        /// extension.  NULL terminates this array, or pass NULL if there are no planes
        /// you want loaded.
        /// </para>
        /// <para>
        /// If you pass in a callback and do not receive access to the planes your
        /// callback will be called when the airplanes are available. If you do receive
        /// airplane access, your callback will not be called.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMAcquirePlanes", ExactSpelling = true)]
        public static extern unsafe int AcquirePlanes(byte** inAircraft, delegate* unmanaged[Cdecl]<void*, void> inCallback, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// Call this function to release access to the planes.  Note that if you are
        /// disabled, access to planes is released for you and you must reacquire it.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMReleasePlanes", ExactSpelling = true)]
        public static extern void ReleasePlanes();

        
        /// <summary>
        /// <para>
        /// This routine sets the number of active planes.  If you pass in a number
        /// higher than the total number of planes availables, only the total number of
        /// planes available is actually used.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetActiveAircraftCount", ExactSpelling = true)]
        public static extern void SetActiveAircraftCount(int inCount);

        
        /// <summary>
        /// <para>
        /// This routine loads an aircraft model.  It may only be called if you have
        /// exclusive access to the airplane APIs.  Pass in the path of the model with
        /// the .acf extension.  The index is zero based, but you may not pass in 0
        /// (use XPLMSetUsersAircraft to load the user's aircracft).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetAircraftModel", ExactSpelling = true)]
        public static extern unsafe void SetAircraftModel(int inIndex, byte* inAircraftPath);

        
        /// <summary>
        /// <para>
        /// This routine loads an aircraft model.  It may only be called if you have
        /// exclusive access to the airplane APIs.  Pass in the path of the model with
        /// the .acf extension.  The index is zero based, but you may not pass in 0
        /// (use XPLMSetUsersAircraft to load the user's aircracft).
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetAircraftModel(int inIndex, in ReadOnlySpan<char> inAircraftPath)
        {
            int inAircraftPathUtf8Len = inAircraftPath.Length * 3 + 4;
            Span<byte> inAircraftPathUtf8 = inAircraftPathUtf8Len <= 4096 ? stackalloc byte[inAircraftPathUtf8Len] : new byte[inAircraftPathUtf8Len];
            Utils.ToUtf8(inAircraftPath, inAircraftPathUtf8);
            fixed (byte* inAircraftPathPtr = inAircraftPathUtf8)
                SetAircraftModel(inIndex, inAircraftPathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine loads an aircraft model.  It may only be called if you have
        /// exclusive access to the airplane APIs.  Pass in the path of the model with
        /// the .acf extension.  The index is zero based, but you may not pass in 0
        /// (use XPLMSetUsersAircraft to load the user's aircracft).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetAircraftModel(int inIndex, in XP.SDK.Utf8String inAircraftPath)
        {
            fixed (byte* inAircraftPathPtr = inAircraftPath)
                SetAircraftModel(inIndex, inAircraftPathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine turns off X-Plane's AI for a given plane.  The plane will
        /// continue to draw and be a real plane in X-Plane, but will not move itself.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDisableAIForPlane", ExactSpelling = true)]
        public static extern void DisableAIForPlane(int inPlaneIndex);
    }
}