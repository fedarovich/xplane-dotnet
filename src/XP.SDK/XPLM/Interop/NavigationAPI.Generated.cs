using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Interop
{
    public static partial class NavigationAPI
    {
        
        /// <summary>
        /// <para>
        /// This returns the very first navaid in the database.  Use this to traverse
        /// the entire database.  Returns XPLM_NAV_NOT_FOUND if the nav database is
        /// empty.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetFirstNavAid", ExactSpelling = true)]
        public static extern NavRef GetFirstNavAid();

        
        /// <summary>
        /// <para>
        /// Given a valid nav aid ref, this routine returns the next navaid.  It
        /// returns XPLM_NAV_NOT_FOUND if the nav aid passed in was invalid or if the
        /// navaid passed in was the last one in the database.  Use this routine to
        /// iterate across all like-typed navaids or the entire database.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetNextNavAid", ExactSpelling = true)]
        public static extern NavRef GetNextNavAid(NavRef inNavAidRef);

        
        /// <summary>
        /// <para>
        /// This routine returns the ref of the first navaid of the given type in the
        /// database or XPLM_NAV_NOT_FOUND if there are no navaids of that type in the
        /// database.  You must pass exactly one nav aid type to this routine.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMFindFirstNavAidOfType", ExactSpelling = true)]
        public static extern NavRef FindFirstNavAidOfType(NavType inType);

        
        /// <summary>
        /// <para>
        /// This routine returns the ref of the last navaid of the given type in the
        /// database or XPLM_NAV_NOT_FOUND if there are no navaids of that type in the
        /// database.  You must pass exactly one nav aid type to this routine.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMFindLastNavAidOfType", ExactSpelling = true)]
        public static extern NavRef FindLastNavAidOfType(NavType inType);

        
        /// <summary>
        /// <para>
        /// This routine provides a number of searching capabilities for the nav
        /// database. XPLMFindNavAid will search through every nav aid whose type is
        /// within inType (multiple types may be added together) and return any
        /// nav-aids found based on the following rules:
        /// </para>
        /// <para>
        /// * If inLat and inLon are not NULL, the navaid nearest to that lat/lon will
        /// be returned, otherwise the last navaid found will be returned.
        /// </para>
        /// <para>
        /// * If inFrequency is not NULL, then any navaids considered must match this
        /// frequency.  Note that this will screen out radio beacons that do not have
        /// frequency data published (like inner markers) but not fixes and airports.
        /// </para>
        /// <para>
        /// * If inNameFragment is not NULL, only navaids that contain the fragment in
        /// their name will be returned.
        /// </para>
        /// <para>
        /// * If inIDFragment is not NULL, only navaids that contain the fragment in
        /// their IDs will be returned.
        /// </para>
        /// <para>
        /// This routine provides a simple way to do a number of useful searches:
        /// * Find the nearest navaid on this frequency.
        /// * Find the nearest airport.
        /// * Find the VOR whose ID is "KBOS".
        /// * Find the nearest airport whose name contains "Chicago".
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMFindNavAid", ExactSpelling = true)]
        public static extern unsafe NavRef FindNavAid(byte* inNameFragment, byte* inIDFragment, float* inLat, float* inLon, int* inFrequency, NavType inType);

        
        /// <summary>
        /// <para>
        /// This routine provides a number of searching capabilities for the nav
        /// database. XPLMFindNavAid will search through every nav aid whose type is
        /// within inType (multiple types may be added together) and return any
        /// nav-aids found based on the following rules:
        /// </para>
        /// <para>
        /// * If inLat and inLon are not NULL, the navaid nearest to that lat/lon will
        /// be returned, otherwise the last navaid found will be returned.
        /// </para>
        /// <para>
        /// * If inFrequency is not NULL, then any navaids considered must match this
        /// frequency.  Note that this will screen out radio beacons that do not have
        /// frequency data published (like inner markers) but not fixes and airports.
        /// </para>
        /// <para>
        /// * If inNameFragment is not NULL, only navaids that contain the fragment in
        /// their name will be returned.
        /// </para>
        /// <para>
        /// * If inIDFragment is not NULL, only navaids that contain the fragment in
        /// their IDs will be returned.
        /// </para>
        /// <para>
        /// This routine provides a simple way to do a number of useful searches:
        /// * Find the nearest navaid on this frequency.
        /// * Find the nearest airport.
        /// * Find the VOR whose ID is "KBOS".
        /// * Find the nearest airport whose name contains "Chicago".
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe NavRef FindNavAid(in XP.SDK.Utf8String inNameFragment, in XP.SDK.Utf8String inIDFragment, float* inLat, float* inLon, int* inFrequency, NavType inType)
        {
            fixed (byte* inNameFragmentPtr = inNameFragment, inIDFragmentPtr = inIDFragment)
                return FindNavAid(inNameFragmentPtr, inIDFragmentPtr, inLat, inLon, inFrequency, inType);
        }

        
        /// <summary>
        /// <para>
        /// This routine provides a number of searching capabilities for the nav
        /// database. XPLMFindNavAid will search through every nav aid whose type is
        /// within inType (multiple types may be added together) and return any
        /// nav-aids found based on the following rules:
        /// </para>
        /// <para>
        /// * If inLat and inLon are not NULL, the navaid nearest to that lat/lon will
        /// be returned, otherwise the last navaid found will be returned.
        /// </para>
        /// <para>
        /// * If inFrequency is not NULL, then any navaids considered must match this
        /// frequency.  Note that this will screen out radio beacons that do not have
        /// frequency data published (like inner markers) but not fixes and airports.
        /// </para>
        /// <para>
        /// * If inNameFragment is not NULL, only navaids that contain the fragment in
        /// their name will be returned.
        /// </para>
        /// <para>
        /// * If inIDFragment is not NULL, only navaids that contain the fragment in
        /// their IDs will be returned.
        /// </para>
        /// <para>
        /// This routine provides a simple way to do a number of useful searches:
        /// * Find the nearest navaid on this frequency.
        /// * Find the nearest airport.
        /// * Find the VOR whose ID is "KBOS".
        /// * Find the nearest airport whose name contains "Chicago".
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        public static unsafe NavRef FindNavAid(in ReadOnlySpan<char> inNameFragment, in ReadOnlySpan<char> inIDFragment, float* inLat, float* inLon, int* inFrequency, NavType inType)
        {
            int inNameFragmentUtf8Len = inNameFragment.Length * 3 + 4;
            Span<byte> inNameFragmentUtf8 = inNameFragmentUtf8Len <= 4096 ? stackalloc byte[inNameFragmentUtf8Len] : GC.AllocateUninitializedArray<byte>(inNameFragmentUtf8Len);
            var inNameFragmentUtf8Str = Utf8String.FromUtf16Unsafe(inNameFragment, inNameFragmentUtf8);
            int inIDFragmentUtf8Len = inIDFragment.Length * 3 + 4;
            Span<byte> inIDFragmentUtf8 = inIDFragmentUtf8Len <= 4096 ? stackalloc byte[inIDFragmentUtf8Len] : GC.AllocateUninitializedArray<byte>(inIDFragmentUtf8Len);
            var inIDFragmentUtf8Str = Utf8String.FromUtf16Unsafe(inIDFragment, inIDFragmentUtf8);
            return FindNavAid(inNameFragmentUtf8Str, inIDFragmentUtf8Str, inLat, inLon, inFrequency, inType);
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetNavAidInfo", ExactSpelling = true)]
        public static extern unsafe void GetNavAidInfo(NavRef inRef, NavType* outType, float* outLatitude, float* outLongitude, float* outHeight, int* outFrequency, float* outHeading, byte* outID, byte* outName, byte* outReg);

        
        /// <summary>
        /// <para>
        /// This routine returns the number of entries in the FMS.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCountFMSEntries", ExactSpelling = true)]
        public static extern int CountFMSEntries();

        
        /// <summary>
        /// <para>
        /// This routine returns the index of the entry the pilot is viewing.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDisplayedFMSEntry", ExactSpelling = true)]
        public static extern int GetDisplayedFMSEntry();

        
        /// <summary>
        /// <para>
        /// This routine returns the index of the entry the FMS is flying to.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetDestinationFMSEntry", ExactSpelling = true)]
        public static extern int GetDestinationFMSEntry();

        
        /// <summary>
        /// <para>
        /// This routine changes which entry the FMS is showing to the index specified.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetDisplayedFMSEntry", ExactSpelling = true)]
        public static extern void SetDisplayedFMSEntry(int inIndex);

        
        /// <summary>
        /// <para>
        /// This routine changes which entry the FMS is flying the aircraft toward.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetDestinationFMSEntry", ExactSpelling = true)]
        public static extern void SetDestinationFMSEntry(int inIndex);

        
        /// <summary>
        /// <para>
        /// This routine returns information about a given FMS entry. If the entry is
        /// an airport or navaid, a reference to a nav entry can be returned allowing
        /// you to find additional information (such as a frequency, ILS heading, name,
        /// etc.). Note that this reference can be XPLM_NAV_NOT_FOUND until the
        /// information has been looked up asynchronously, so after flightplan changes,
        /// it might take up to a second for this field to become populated. The other
        /// information is available immediately. For a lat/lon entry, the lat/lon is
        /// returned by this routine but the navaid cannot be looked up (and the
        /// reference will be XPLM_NAV_NOT_FOUND). FMS name entry buffers should be at
        /// least 256 chars in length.
        /// </para>
        /// <para>
        /// WARNING: Due to a bug in X-Plane prior to 11.31, the navaid reference will
        /// not be set to XPLM_NAV_NOT_FOUND while no data is available, and instead
        /// just remain the value of the variable that you passed the pointer to.
        /// Therefore, always initialize the variable to XPLM_NAV_NOT_FOUND before
        /// passing the pointer to this function.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetFMSEntryInfo", ExactSpelling = true)]
        public static extern unsafe void GetFMSEntryInfo(int inIndex, NavType* outType, byte* outID, NavRef* outRef, int* outAltitude, float* outLat, float* outLon);

        
        /// <summary>
        /// <para>
        /// This routine changes an entry in the FMS to have the destination navaid
        /// passed in and the altitude specified.  Use this only for airports, fixes,
        /// and radio-beacon navaids.  Currently of radio beacons, the FMS can only
        /// support VORs and NDBs. Use the routines below to clear or fly to a lat/lon.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetFMSEntryInfo", ExactSpelling = true)]
        public static extern void SetFMSEntryInfo(int inIndex, NavRef inRef, int inAltitude);

        
        /// <summary>
        /// <para>
        /// This routine changes the entry in the FMS to a lat/lon entry with the given
        /// coordinates.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetFMSEntryLatLon", ExactSpelling = true)]
        public static extern void SetFMSEntryLatLon(int inIndex, float inLat, float inLon, int inAltitude);

        
        /// <summary>
        /// <para>
        /// This routine clears the given entry, potentially shortening the flight
        /// plan.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMClearFMSEntry", ExactSpelling = true)]
        public static extern void ClearFMSEntry(int inIndex);

        
        /// <summary>
        /// <para>
        /// This routine returns the type of the currently selected GPS destination,
        /// one of fix, airport, VOR or NDB.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetGPSDestinationType", ExactSpelling = true)]
        public static extern NavType GetGPSDestinationType();

        
        /// <summary>
        /// <para>
        /// This routine returns the current GPS destination.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetGPSDestination", ExactSpelling = true)]
        public static extern NavRef GetGPSDestination();
    }
}