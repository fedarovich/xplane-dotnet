using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// <para>
    /// Use the magnetic variation (more properly, the "magnetic declination") API to find the offset of magnetic north from true north at a given latitude and longitude within the simulator.
    /// </para>
    /// <para>
    /// In the real world, the Earth’s magnetic field is irregular, such that true north (the direction along a meridian toward the north pole) does not necessarily match what a magnetic compass shows as north.
    /// </para>
    /// <para>
    /// Using this API ensures that you present the same offsets to users as X-Plane’s built-in instruments.
    /// </para>
    /// </summary>
    public static class MagneticVariation
    {
        /// <summary>
        /// <para>
        /// Returns X-Plane's simulated magnetic variation (declination) at the
        /// indication latitude and longitude.
        /// </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Get(double latitude, double longitude) => SceneryAPI.GetMagneticVariation(latitude, longitude);

        /// <summary>
        /// <para>
        /// Converts a heading in degrees relative to true north into a value relative
        /// to magnetic north at the user's current location.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float TrueToMagnetic(float headingDegreesTrue) => SceneryAPI.DegTrueToDegMagnetic(headingDegreesTrue);

        /// <summary>
        /// <para>
        /// Converts a heading in degrees relative to magnetic north at the user's
        /// current location into a value relative to true north.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float MagneticToTrue(float headingDegreesMagnetic) => SceneryAPI.DegMagneticToDegTrue(headingDegreesMagnetic);
    }
}
