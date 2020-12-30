using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public readonly struct NavAid
    {
        private readonly int _navRefPlusOne;

        public NavAid(NavRef navRef)
        {
            _navRefPlusOne = (int) navRef + 1;
        }

        public NavRef NavRef => _navRefPlusOne - 1;

        public unsafe NavType Type
        {
            get
            {
                NavType type;
                NavigationAPI.GetNavAidInfo(NavRef,
                    &type,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);
                return type;
            }
        }

        public unsafe (float lat, float lon) Coordinates
        {
            get
            {
                (float lat, float lon) coords;
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    &coords.lat,
                    &coords.lon,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);
                return coords;
            }
        }

        public unsafe float Height
        {
            get
            {
                float height;
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    null,
                    null,
                    &height,
                    null,
                    null,
                    null,
                    null,
                    null);
                return height;
            }
        }

        public unsafe int Frequency
        {
            get
            {
                int frequency;
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    null,
                    null,
                    null,
                    &frequency,
                    null,
                    null,
                    null,
                    null);
                return frequency;
            }
        }

        public unsafe float Heading
        {
            get
            {
                float heading;
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    null,
                    null,
                    null,
                    null,
                    &heading,
                    null,
                    null,
                    null);
                return heading;
            }
        }

        public unsafe string Id
        {
            get
            {
                byte* buffer = stackalloc byte[32];
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    buffer,
                    null,
                    null);
                return Marshal.PtrToStringUTF8(new IntPtr(buffer));
            }
        }

        public unsafe string Name
        {
            get
            {
                byte* buffer = stackalloc byte[256];
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    buffer,
                    null);
                return Marshal.PtrToStringUTF8(new IntPtr(buffer));
            }
        }

        public unsafe bool IsInLocalRegion
        {
            get
            {
                byte reg;
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    &reg);
                return reg != 0;
            }
        }

        public static IEnumerable<NavAid> Enumerate()
        {
            var navRef = NavigationAPI.GetFirstNavAid();
            while (navRef != NavRef.NotFound)
            {
                yield return new NavAid(navRef);
                navRef = NavigationAPI.GetNextNavAid(navRef);
            }
        }

        public static IEnumerable<NavAid> Enumerate(NavType type)
        {
            int typeInt = (int)type;
            if ((type ^ (type - 1)) != 0)
                throw new ArgumentException("Type parameter must contain exactly on nav aid type.", nameof(type));

            var first = NavigationAPI.FindFirstNavAidOfType(type);
            if (first == NavRef.NotFound)
                yield break;

            yield return new NavAid(first);

            var current = first;
            var last = NavigationAPI.FindLastNavAidOfType(type);
            while (current != last)
            {
                current = NavigationAPI.GetNextNavAid(current);
                yield return new NavAid(current);
            }
        }

        public static unsafe NavAid Find(
            NavType type,
            in ReadOnlySpan<char> nameFragment = default,
            in ReadOnlySpan<char> idFragment = default,
            (float lat, float lon)? coordinates = null,
            int? frequency = null)
        {
            float* pLat = null, pLon = null;
            int* pFreq = null;
            if (coordinates != null)
            {
                var (lat, lon) = coordinates.Value;
                pLat = &lat;
                pLon = &lon;
            }
            if (frequency != null)
            {
                var freq = frequency.Value;
                pFreq = &freq;
            }

            var navRef = NavigationAPI.FindNavAid(
                nameFragment,
                idFragment,
                pLat,
                pLon,
                pFreq,
                type);
            return new NavAid(navRef);
        }

        public static NavAid GetGPSDestination()
        {
            return new NavAid(NavigationAPI.GetGPSDestination());
        }
    }
}
