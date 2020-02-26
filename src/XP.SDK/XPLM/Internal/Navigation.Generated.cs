using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Navigation
    {
        private static IntPtr GetFirstNavAidPtr;
        private static IntPtr GetNextNavAidPtr;
        private static IntPtr FindFirstNavAidOfTypePtr;
        private static IntPtr FindLastNavAidOfTypePtr;
        private static IntPtr FindNavAidPtr;
        private static IntPtr GetNavAidInfoPtr;
        private static IntPtr CountFMSEntriesPtr;
        private static IntPtr GetDisplayedFMSEntryPtr;
        private static IntPtr GetDestinationFMSEntryPtr;
        private static IntPtr SetDisplayedFMSEntryPtr;
        private static IntPtr SetDestinationFMSEntryPtr;
        private static IntPtr GetFMSEntryInfoPtr;
        private static IntPtr SetFMSEntryInfoPtr;
        private static IntPtr SetFMSEntryLatLonPtr;
        private static IntPtr ClearFMSEntryPtr;
        private static IntPtr GetGPSDestinationTypePtr;
        private static IntPtr GetGPSDestinationPtr;

        static Navigation()
        {
            const string libraryName = "XPLM";
            GetFirstNavAidPtr = FunctionResolver.Resolve(libraryName, "XPLMGetFirstNavAid");
            GetNextNavAidPtr = FunctionResolver.Resolve(libraryName, "XPLMGetNextNavAid");
            FindFirstNavAidOfTypePtr = FunctionResolver.Resolve(libraryName, "XPLMFindFirstNavAidOfType");
            FindLastNavAidOfTypePtr = FunctionResolver.Resolve(libraryName, "XPLMFindLastNavAidOfType");
            FindNavAidPtr = FunctionResolver.Resolve(libraryName, "XPLMFindNavAid");
            GetNavAidInfoPtr = FunctionResolver.Resolve(libraryName, "XPLMGetNavAidInfo");
            CountFMSEntriesPtr = FunctionResolver.Resolve(libraryName, "XPLMCountFMSEntries");
            GetDisplayedFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDisplayedFMSEntry");
            GetDestinationFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMGetDestinationFMSEntry");
            SetDisplayedFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDisplayedFMSEntry");
            SetDestinationFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMSetDestinationFMSEntry");
            GetFMSEntryInfoPtr = FunctionResolver.Resolve(libraryName, "XPLMGetFMSEntryInfo");
            SetFMSEntryInfoPtr = FunctionResolver.Resolve(libraryName, "XPLMSetFMSEntryInfo");
            SetFMSEntryLatLonPtr = FunctionResolver.Resolve(libraryName, "XPLMSetFMSEntryLatLon");
            ClearFMSEntryPtr = FunctionResolver.Resolve(libraryName, "XPLMClearFMSEntry");
            GetGPSDestinationTypePtr = FunctionResolver.Resolve(libraryName, "XPLMGetGPSDestinationType");
            GetGPSDestinationPtr = FunctionResolver.Resolve(libraryName, "XPLMGetGPSDestination");
        }

        
        /// <summary>
        /// <para>
        /// This returns the very first navaid in the database.  Use this to traverse
        /// the entire database.  Returns XPLM_NAV_NOT_FOUND if the nav database is
        /// empty.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef GetFirstNavAid()
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(GetFirstNavAidPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Given a nav aid ref, this routine returns the next navaid.  It returns
        /// XPLM_NAV_NOT_FOUND if the nav aid passed in was invalid or if the navaid
        /// passed in was the last one in the database.  Use this routine to iterate
        /// across all like-typed navaids or the entire database.
        /// </para>
        /// <para>
        /// WARNING: due to a bug in the SDK, when fix loading is disabled in the
        /// rendering settings screen, calling this routine with the last airport
        /// returns a bogus nav aid.  Using this nav aid can crash X-Plane.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef GetNextNavAid(NavRef inNavAidRef)
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(inNavAidRef);
            IL.Push(GetNextNavAidPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef), typeof(NavRef)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the ref of the first navaid of the given type in the
        /// database or XPLM_NAV_NOT_FOUND if there are no navaids of that type in the
        /// database.  You must pass exactly one nav aid type to this routine.
        /// </para>
        /// <para>
        /// WARNING: due to a bug in the SDK, when fix loading is disabled in the
        /// rendering settings screen, calling this routine with fixes returns a bogus
        /// nav aid.  Using this nav aid can crash X-Plane.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef FindFirstNavAidOfType(NavType inType)
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(inType);
            IL.Push(FindFirstNavAidOfTypePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef), typeof(NavType)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the ref of the last navaid of the given type in the
        /// database or XPLM_NAV_NOT_FOUND if there are no navaids of that type in the
        /// database.  You must pass exactly one nav aid type to this routine.
        /// </para>
        /// <para>
        /// WARNING: due to a bug in the SDK, when fix loading is disabled in the
        /// rendering settings screen, calling this routine with fixes returns a bogus
        /// nav aid.  Using this nav aid can crash X-Plane.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef FindLastNavAidOfType(NavType inType)
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(inType);
            IL.Push(FindLastNavAidOfTypePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef), typeof(NavType)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine provides a number of searching capabilities for the nav
        /// database. XPLMFindNavAid will search through every nav aid whose type is
        /// within inType (multiple types may be added together) and return any
        /// nav-aids found based  on the following rules:
        /// </para>
        /// <para>
        /// If inLat and inLon are not NULL, the navaid nearest to that lat/lon will be
        /// returned, otherwise the last navaid found will be returned.
        /// </para>
        /// <para>
        /// If inFrequency is not NULL, then any navaids considered must match this
        /// frequency.  Note that this will screen out radio beacons that do not have
        /// frequency data published (like inner markers) but not fixes and airports.
        /// </para>
        /// <para>
        /// If inNameFragment is not NULL, only navaids that contain the fragment in
        /// their name will be returned.
        /// </para>
        /// <para>
        /// If inIDFragment is not NULL, only navaids that contain the fragment in
        /// their IDs will be returned.
        /// </para>
        /// <para>
        /// This routine provides a simple way to do a number of useful searches:
        /// </para>
        /// <para>
        /// Find the nearest navaid on this frequency. Find the nearest airport. Find
        /// the VOR whose ID is "KBOS". Find the nearest airport whose name contains
        /// "Chicago".
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe NavRef FindNavAid(byte* inNameFragment, byte* inIDFragment, float* inLat, float* inLon, int* inFrequency, NavType inType)
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(inNameFragment);
            IL.Push(inIDFragment);
            IL.Push(inLat);
            IL.Push(inLon);
            IL.Push(inFrequency);
            IL.Push(inType);
            IL.Push(FindNavAidPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef), typeof(byte*), typeof(byte*), typeof(float*), typeof(float*), typeof(int*), typeof(NavType)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine provides a number of searching capabilities for the nav
        /// database. XPLMFindNavAid will search through every nav aid whose type is
        /// within inType (multiple types may be added together) and return any
        /// nav-aids found based  on the following rules:
        /// </para>
        /// <para>
        /// If inLat and inLon are not NULL, the navaid nearest to that lat/lon will be
        /// returned, otherwise the last navaid found will be returned.
        /// </para>
        /// <para>
        /// If inFrequency is not NULL, then any navaids considered must match this
        /// frequency.  Note that this will screen out radio beacons that do not have
        /// frequency data published (like inner markers) but not fixes and airports.
        /// </para>
        /// <para>
        /// If inNameFragment is not NULL, only navaids that contain the fragment in
        /// their name will be returned.
        /// </para>
        /// <para>
        /// If inIDFragment is not NULL, only navaids that contain the fragment in
        /// their IDs will be returned.
        /// </para>
        /// <para>
        /// This routine provides a simple way to do a number of useful searches:
        /// </para>
        /// <para>
        /// Find the nearest navaid on this frequency. Find the nearest airport. Find
        /// the VOR whose ID is "KBOS". Find the nearest airport whose name contains
        /// "Chicago".
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe NavRef FindNavAid(in ReadOnlySpan<char> inNameFragment, in ReadOnlySpan<char> inIDFragment, float* inLat, float* inLon, int* inFrequency, NavType inType)
        {
            IL.DeclareLocals(false);
            Span<byte> inNameFragmentUtf8 = stackalloc byte[(inNameFragment.Length << 1) | 1];
            var inNameFragmentPtr = Utils.ToUtf8Unsafe(inNameFragment, inNameFragmentUtf8);
            Span<byte> inIDFragmentUtf8 = stackalloc byte[(inIDFragment.Length << 1) | 1];
            var inIDFragmentPtr = Utils.ToUtf8Unsafe(inIDFragment, inIDFragmentUtf8);
            return FindNavAid(inNameFragmentPtr, inIDFragmentPtr, inLat, inLon, inFrequency, inType);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns information about a navaid.  Any non-null field is
        /// filled out with information if it is available.
        /// </para>
        /// <para>
        /// Frequencies are in the nav.dat convention as described in the X-Plane nav
        /// database FAQ: NDB frequencies are exact, all others are multiplied by 100.
        /// </para>
        /// <para>
        /// The buffer for IDs should be at least 6 chars and the buffer for names
        /// should be at least 41 chars, but since these values are likely to go up, I
        /// recommend passing at least 32 chars for IDs and 256 chars for names when
        /// possible.
        /// </para>
        /// <para>
        /// The outReg parameter tells if the navaid is within the local "region" of
        /// loaded DSFs.  (This information may not be particularly useful to plugins.)
        /// The parameter is a single byte value 1 for true or 0 for false, not a C
        /// string.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetNavAidInfo(NavRef inRef, NavType* outType, float* outLatitude, float* outLongitude, float* outHeight, int* outFrequency, float* outHeading, byte* outID, byte* outName, byte* outReg)
        {
            IL.DeclareLocals(false);
            IL.Push(inRef);
            IL.Push(outType);
            IL.Push(outLatitude);
            IL.Push(outLongitude);
            IL.Push(outHeight);
            IL.Push(outFrequency);
            IL.Push(outHeading);
            IL.Push(outID);
            IL.Push(outName);
            IL.Push(outReg);
            IL.Push(GetNavAidInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(NavRef), typeof(NavType*), typeof(float*), typeof(float*), typeof(float*), typeof(int*), typeof(float*), typeof(byte*), typeof(byte*), typeof(byte*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the number of entries in the FMS.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountFMSEntries()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(CountFMSEntriesPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the index of the entry the pilot is viewing.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetDisplayedFMSEntry()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(GetDisplayedFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the index of the entry the FMS is flying to.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetDestinationFMSEntry()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(GetDestinationFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine changes which entry the FMS is showing to the index specified.
        /// *
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDisplayedFMSEntry(int inIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(SetDisplayedFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine changes which entry the FMS is flying the aircraft toward.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetDestinationFMSEntry(int inIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(SetDestinationFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns information about a given FMS entry.  A reference to a
        /// navaid can be returned allowing you to find additional information (such as
        /// a frequency, ILS heading, name, etc.).  Some information is available
        /// immediately.  For a lat/lon entry, the lat/lon is returned by this routine
        /// but the navaid cannot be looked up (and the reference will be
        /// XPLM_NAV_NOT_FOUND. FMS name entry buffers should be at least 256 chars in
        /// length.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetFMSEntryInfo(int inIndex, NavType* outType, byte* outID, NavRef* outRef, int* outAltitude, float* outLat, float* outLon)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(outType);
            IL.Push(outID);
            IL.Push(outRef);
            IL.Push(outAltitude);
            IL.Push(outLat);
            IL.Push(outLon);
            IL.Push(GetFMSEntryInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(NavType*), typeof(byte*), typeof(NavRef*), typeof(int*), typeof(float*), typeof(float*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine changes an entry in the FMS to have the destination navaid
        /// passed in and the altitude specified.  Use this only for airports, fixes,
        /// and radio-beacon navaids.  Currently of radio beacons, the FMS can only
        /// support VORs and NDBs. Use the routines below to clear or fly to a lat/lon.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetFMSEntryInfo(int inIndex, NavRef inRef, int inAltitude)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(inRef);
            IL.Push(inAltitude);
            IL.Push(SetFMSEntryInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(NavRef), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine changes the entry in the FMS to a lat/lon entry with the given
        /// coordinates.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetFMSEntryLatLon(int inIndex, float inLat, float inLon, int inAltitude)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(inLat);
            IL.Push(inLon);
            IL.Push(inAltitude);
            IL.Push(SetFMSEntryLatLonPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int), typeof(float), typeof(float), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine clears the given entry, potentially shortening the flight
        /// plan.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ClearFMSEntry(int inIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inIndex);
            IL.Push(ClearFMSEntryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the type of the currently selected GPS destination,
        /// one of fix, airport, VOR or NDB.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavType GetGPSDestinationType()
        {
            IL.DeclareLocals(false);
            NavType result;
            IL.Push(GetGPSDestinationTypePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavType)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the current GPS destination.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static NavRef GetGPSDestination()
        {
            IL.DeclareLocals(false);
            NavRef result;
            IL.Push(GetGPSDestinationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(NavRef)));
            IL.Pop(out result);
            return result;
        }
    }
}