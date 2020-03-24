using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public static class FMS
    {
        public static int EntryCount => NavigationAPI.CountFMSEntries();

        public static int DisplayedEntry
        {
            get => NavigationAPI.GetDisplayedFMSEntry();
            set => NavigationAPI.SetDisplayedFMSEntry(value);
        }

        public static int DestinationEntry
        {
            get => NavigationAPI.GetDestinationFMSEntry();
            set => NavigationAPI.SetDestinationFMSEntry(value);
        }

        public static void ClearEntry(int index) => NavigationAPI.ClearFMSEntry(index);

        public static void SetEntry(int index, NavRef navRef, int altitude) => NavigationAPI.SetFMSEntryInfo(index, navRef, altitude);

        public static void SetEntry(int index, NavAid navAid, int altitude) => NavigationAPI.SetFMSEntryInfo(index, navAid.NavRef, altitude);

        public static void SetEntry(int index, float latitude, float longitude, int altitude) => NavigationAPI.SetFMSEntryLatLon(index, latitude, longitude, altitude);

        public static unsafe void GetEntry(int index, out string id, out NavType navType, out NavRef navRef,
            out float latitude, out float longitude, out int altitude)
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
    }
}
