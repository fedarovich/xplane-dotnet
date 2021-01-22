#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Provides information about a navigational aid.
    /// </summary>
    public readonly struct NavAid
    {
        private readonly int _navRefPlusOne;

        /// <summary>
        /// Initialized a new instance of the <see cref="NavAid"/>.
        /// </summary>
        /// <param name="navRef"></param>
        public NavAid(NavRef navRef)
        {
            _navRefPlusOne = (int) navRef + 1;
        }

        /// <summary>
        /// Gets the <see cref="NavRef"/> for this navaid.
        /// </summary>
        public NavRef NavRef => _navRefPlusOne - 1;

        /// <summary>
        /// Gets the type of the navaid.
        /// </summary>
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

        /// <summary>
        /// Gets the coordinates of the navaid.
        /// </summary>
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

        /// <summary>
        /// Gets the height of the navaid.
        /// </summary>
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

        /// <summary>
        /// Gets the frequency of the navaid.
        /// </summary>
        /// <remarks>
        /// Frequencies are in the nav.dat convention as described in the X-Plane nav
        /// database FAQ: NDB frequencies are exact, all others are multiplied by 100.
        /// </remarks>
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

        /// <summary>
        /// Gets the heading of the navaid.
        /// </summary>
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

        /// <summary>
        /// Gets the ID of the navaid.
        /// </summary>
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
                return Marshal.PtrToStringUTF8(new IntPtr(buffer))!;
            }
        }

        /// <summary>
        /// Gets the name of the navaid.
        /// </summary>
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
                return Marshal.PtrToStringUTF8(new IntPtr(buffer))!;
            }
        }

        /// <summary>
        /// Get the value indicating whether the navaid
        /// is within the local "region" of  loaded DSFs.
        /// </summary>
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

        /// <summary>
        /// Gets the Id of the navaid.
        /// </summary>
        public unsafe Utf8String GetId()
        {
            var buffer = GC.AllocateUninitializedArray<byte>(32);
            fixed (byte* pBuffer = buffer)
            {
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    pBuffer,
                    null,
                    null);
            }

            return new Utf8String(buffer);
        }

        /// <summary>
        /// Gets the name of the navaid.
        /// </summary>
        public unsafe Utf8String GetName()
        {
            var buffer = GC.AllocateUninitializedArray<byte>(256);
            fixed (byte* pBuffer = buffer)
            {
                NavigationAPI.GetNavAidInfo(NavRef,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    pBuffer,
                    null);
            }

            return new Utf8String(buffer);
        }

        /// <summary>
        /// Enumerates the navaid database.
        /// </summary>
        public static IEnumerable<NavAid> Enumerate()
        {
            var navRef = NavigationAPI.GetFirstNavAid();
            while (navRef != NavRef.NotFound)
            {
                yield return new NavAid(navRef);
                navRef = NavigationAPI.GetNextNavAid(navRef);
            }
        }

        /// <summary>
        /// Enumerates the navigational aids of the specified
        /// <paramref name="type"/> in the navaid database.
        /// </summary>
        /// <param name="type">The navigational aid type. Exactly one type must be specified.</param>
        /// <exception cref="ArgumentException">No type or more than one type is passed.</exception>
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

        /// <summary>
        /// <para>
        /// This routine provides a number of searching capabilities for the nav
        /// database. It will search through every nav aid whose type is
        /// within <paramref name="type"/> (multiple types may be added together) and return any
        /// nav-aids found based on the following rules:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// If <paramref name="coordinates"/> are not <see langword="null"/>,
        /// the navaid nearest to that lat/lon will be returned,
        /// otherwise the last navaid found will be returned.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If <paramref name="frequency"/> is not <see langword="null"/>,
        /// then any navaids considered must match this frequency.
        /// Note that this will screen out radio beacons that do not have
        /// frequency data published (like inner markers) but not fixes and airports.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If <paramref name="nameFragment"/> is not empty,
        /// only navaids that contain the fragment in their name will be returned.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If <paramref name="idFragment"/> is not empty, only navaids that contain the fragment in
        /// their IDs will be returned.
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// <para>This routine provides a simple way to do a number of useful searches:</para>
        /// <list type="bullet">
        /// <item><description>Find the nearest navaid on this frequency.</description></item>
        /// <item><description>Find the nearest airport.</description></item>
        /// <item><description>Find the VOR whose ID is "KBOS".</description></item>
        /// <item><description>Find the nearest airport whose name contains "Chicago".</description></item>
        /// </list>
        /// </remarks>
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

        /// <summary>
        /// <para>
        /// This routine provides a number of searching capabilities for the nav
        /// database. It will search through every nav aid whose type is
        /// within <paramref name="type"/> (multiple types may be added together) and return any
        /// nav-aids found based on the following rules:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// If <paramref name="coordinates"/> are not <see langword="null"/>,
        /// the navaid nearest to that lat/lon will be returned,
        /// otherwise the last navaid found will be returned.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If <paramref name="frequency"/> is not <see langword="null"/>,
        /// then any navaids considered must match this frequency.
        /// Note that this will screen out radio beacons that do not have
        /// frequency data published (like inner markers) but not fixes and airports.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If <paramref name="nameFragment"/> is not empty,
        /// only navaids that contain the fragment in their name will be returned.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If <paramref name="idFragment"/> is not empty, only navaids that contain the fragment in
        /// their IDs will be returned.
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// <para>This routine provides a simple way to do a number of useful searches:</para>
        /// <list type="bullet">
        /// <item><description>Find the nearest navaid on this frequency.</description></item>
        /// <item><description>Find the nearest airport.</description></item>
        /// <item><description>Find the VOR whose ID is "KBOS".</description></item>
        /// <item><description>Find the nearest airport whose name contains "Chicago".</description></item>
        /// </list>
        /// </remarks>
        public static unsafe NavAid Find(
            NavType type,
            in Utf8String nameFragment = default,
            in Utf8String idFragment = default,
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

        /// <summary>
        /// Returns the current GPS destination.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static NavAid GetGPSDestination()
        {
            return new NavAid(NavigationAPI.GetGPSDestination());
        }
    }
}
