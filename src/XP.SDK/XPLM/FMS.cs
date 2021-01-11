using System;
using System.Runtime.InteropServices;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Provides access to X-Plane's Flight Management System (Flight Management Computer).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The FMS works based on an array of entries. Indices into the array are zero-based.
    /// Each entry is a nav-aid plus an altitude. The FMS tracks the currently displayed entry and the entry that it is flying to.
    /// </para>
    /// <para>
    /// The FMS must be programmed with contiguous entries, so clearing an entry at the end shortens the effective flight plan.
    /// There is a max of 100 waypoints in the flight plan.
    /// </para>
    /// </remarks>
    public static class FMS
    {
        /// <summary>
        /// The maximal number of entries.
        /// </summary>
        public const int MaxEntryCount = 100;
        
        /// <summary>
        /// Get the number of entries in the FMS.
        /// </summary>
        public static int EntryCount => NavigationAPI.CountFMSEntries();

        /// <summary>
        /// Gets or sets the index of the entry the pilot is viewing.
        /// </summary>
        public static int DisplayedEntry
        {
            get => NavigationAPI.GetDisplayedFMSEntry();
            set => NavigationAPI.SetDisplayedFMSEntry(value);
        }

        /// <summary>
        /// Gets or sets the index of the entry the FMS is flying to.
        /// </summary>
        public static int DestinationEntry
        {
            get => NavigationAPI.GetDestinationFMSEntry();
            set => NavigationAPI.SetDestinationFMSEntry(value);
        }

        /// <summary>
        /// Clears the given entry, potentially shortening the flight plan.
        /// </summary>
        public static void ClearEntry(int index) => NavigationAPI.ClearFMSEntry(index);

        /// <summary>
        /// Changes an entry in the FMS to have the destination navaid
        /// passed in and the altitude specified.  Use this only for airports, fixes,
        /// and radio-beacon navaids.  Currently of radio beacons, the FMS can only
        /// support VORs and NDBs.
        /// </summary>
        public static void SetEntry(int index, NavRef navRef, int altitude) => NavigationAPI.SetFMSEntryInfo(index, navRef, altitude);

        /// <summary>
        /// Changes an entry in the FMS to have the destination navaid
        /// passed in and the altitude specified.  Use this only for airports, fixes,
        /// and radio-beacon navaids.  Currently of radio beacons, the FMS can only
        /// support VORs and NDBs.
        /// </summary>
        public static void SetEntry(int index, NavAid navAid, int altitude) => NavigationAPI.SetFMSEntryInfo(index, navAid.NavRef, altitude);

        /// <summary>
        /// Changes the entry in the FMS to a lat/lon entry with the given coordinates.
        /// </summary>
        public static void SetEntry(int index, float latitude, float longitude, int altitude) => NavigationAPI.SetFMSEntryLatLon(index, latitude, longitude, altitude);

        /// <summary>
        /// This routine returns information about a given FMS entry. If the entry is
        /// an airport or navaid, a reference to a nav entry can be returned allowing
        /// you to find additional information (such as a frequency, ILS heading, name,
        /// etc.). Note that this reference can be <see cref="NavRef.NotFound"/> until the
        /// information has been looked up asynchronously, so after flightplan changes,
        /// it might take up to a second for this field to become populated. The other
        /// information is available immediately. For a lat/lon entry, the lat/lon is
        /// returned by this routine but the navaid cannot be looked up (and the
        /// reference will be <see cref="NavRef.NotFound"/>). FMS name entry buffers should be at
        /// least 256 chars in length.
        /// </summary>
        public static unsafe void GetEntry(int index, out string id, out NavType navType, out NavRef navRef, out float latitude, out float longitude, out int altitude)
        {
            /*
             * Due to a bug in X-Plane prior to 11.31, the navaid reference will not be set to XPLM_NAV_NOT_FOUND while no data is available,
             * and instead just remain the value of the variable that you passed the pointer to.
             * Therefore, always initialize the variable to XPLM_NAV_NOT_FOUND before passing the pointer to this function.
             */
            navRef = NavRef.NotFound;
            fixed (NavType* pNavType = &navType)
            {
                fixed (NavRef* pNavRef = &navRef)
                {
                    fixed (int* pAlt = &altitude)
                    {
                        fixed (float* pLat = &latitude, pLon = &longitude)
                        {
                            byte* idBuffer = stackalloc byte[256];
                            NavigationAPI.GetFMSEntryInfo(index, pNavType, idBuffer, pNavRef, pAlt, pLat, pLon);
                            id = Marshal.PtrToStringUTF8(new IntPtr(idBuffer));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This routine returns information about a given FMS entry. If the entry is
        /// an airport or navaid, a reference to a nav entry can be returned allowing
        /// you to find additional information (such as a frequency, ILS heading, name,
        /// etc.). Note that this reference can be <see cref="NavRef.NotFound"/> until the
        /// information has been looked up asynchronously, so after flightplan changes,
        /// it might take up to a second for this field to become populated. The other
        /// information is available immediately. For a lat/lon entry, the lat/lon is
        /// returned by this routine but the navaid cannot be looked up (and the
        /// reference will be <see cref="NavRef.NotFound"/>). FMS name entry buffers should be at
        /// least 256 chars in length.
        /// </summary>
        /// <exception cref="ArgumentException">The length of the <paramref name="id"/> is less than 256.</exception>
        public static unsafe void GetEntry(int index, in Span<byte> id, out NavType navType, out NavRef navRef, out float latitude, out float longitude, out int altitude)
        {
            if (id.Length < 256)
                throw new ArgumentException("The id buffer must be at least 256 bytes long.", nameof(id));
            
            /*
             * Due to a bug in X-Plane prior to 11.31, the navaid reference will not be set to XPLM_NAV_NOT_FOUND while no data is available,
             * and instead just remain the value of the variable that you passed the pointer to.
             * Therefore, always initialize the variable to XPLM_NAV_NOT_FOUND before passing the pointer to this function.
             */
            navRef = NavRef.NotFound;
            fixed (byte* pId = id)
            {
                fixed (NavType* pNavType = &navType)
                {
                    fixed (NavRef* pNavRef = &navRef)
                    {
                        fixed (int* pAlt = &altitude)
                        {
                            fixed (float* pLat = &latitude, pLon = &longitude)
                            {
                                NavigationAPI.GetFMSEntryInfo(index, pNavType, pId, pNavRef, pAlt, pLat, pLon);
                            }
                        }
                    }
                }
            }
        }
    }
}
