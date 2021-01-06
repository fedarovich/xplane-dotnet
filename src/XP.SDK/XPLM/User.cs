using System;
using System.Runtime.CompilerServices;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public static class User
    {
        /// <summary>
        /// Changes the user’s aircraft. Note that this will reinitialize the user to be on the nearest airport’s first runway.
        /// </summary>
        /// <param name="aircraftPath">A full path (hard drive and everything including the .acf extension) to the .acf file.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAircraft(in Utf8String aircraftPath)
        {
            PlanesAPI.SetUsersAircraft(aircraftPath);
        }

        /// <summary>
        /// Changes the user’s aircraft. Note that this will reinitialize the user to be on the nearest airport’s first runway.
        /// </summary>
        /// <param name="aircraftPath">A full path (hard drive and everything including the .acf extension) to the .acf file.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAircraft(in ReadOnlySpan<char> aircraftPath)
        {
            PlanesAPI.SetUsersAircraft(aircraftPath);
        }

        /// <summary>
        /// Places the user at a given airport.
        /// </summary>
        /// <param name="airportCode">The airport by its X-Plane airport ID (e.g. 'KBOS').</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlaceAtAirport(in Utf8String airportCode)
        {
            PlanesAPI.PlaceUserAtAirport(airportCode);
        }

        /// <summary>
        /// Places the user at a given airport.
        /// </summary>
        /// <param name="airportCode">The airport by its X-Plane airport ID (e.g. 'KBOS').</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlaceAtAirport(in ReadOnlySpan<char> airportCode)
        {
            PlanesAPI.PlaceUserAtAirport(airportCode);
        }

        /// <summary>
        /// Places the user at a specific location after performing any necessary scenery loads.
        /// </summary>
        /// <remarks>
        /// As with in-air starts initiated from the X-Plane user interface,
        /// the aircraft will always start with its engines running, regardless of the user’s preferences
        /// (i.e., regardless of what the dataref <c>sim/operation/prefs/startup_running</c> says).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlaceAtLocation(
            double latitudeDegrees, 
            double longitudeDegrees, 
            float elevationMetersMSL, 
            float headingDegreesTrue, 
            float speedMetersPerSecond)
        {
            PlanesAPI.PlaceUserAtLocation(
                latitudeDegrees,
                longitudeDegrees,
                elevationMetersMSL,
                headingDegreesTrue,
                speedMetersPerSecond);
        }
    }
}
