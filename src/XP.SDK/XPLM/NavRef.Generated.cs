using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// XPLMNavRef is an iterator into the navigation database.  The navigation
    /// database is essentially an array, but it is not necessarily densely
    /// populated. The only assumption you can safely make is that like-typed
    /// nav-aids are  grouped together.
    /// </para>
    /// <para>
    /// Use XPLMNavRef to refer to a nav-aid.
    /// </para>
    /// <para>
    /// XPLM_NAV_NOT_FOUND is returned by functions that return an XPLMNavRef when
    /// the iterator must be invalid.
    /// </para>
    /// </summary>
    public readonly partial struct NavRef : System.IEquatable<NavRef>
    {
        private readonly int _value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public NavRef(int value) => _value = value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NavRef(int value) => new NavRef(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(NavRef value) => value._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is NavRef other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(NavRef other) => _value == other._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(NavRef left, NavRef right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(NavRef left, NavRef right) => !left.Equals(right);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}