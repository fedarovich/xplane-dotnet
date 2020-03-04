using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// This is an opaque identifier for a flight loop callback. You can use this
    /// identifier to easily track and remove your callbacks, or to use the new
    /// flight loop APIs.
    /// </para>
    /// </summary>
    public readonly partial struct FlightLoopID : System.IEquatable<FlightLoopID>
    {
        private readonly System.IntPtr _value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public FlightLoopID(System.IntPtr value) => _value = value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FlightLoopID(System.IntPtr value) => new FlightLoopID(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(FlightLoopID value) => value._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is FlightLoopID other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(FlightLoopID other) => _value == other._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(FlightLoopID left, FlightLoopID right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(FlightLoopID left, FlightLoopID right) => !left.Equals(right);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}