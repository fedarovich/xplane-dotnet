using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// An opaque IDs used to identify a hot key.
    /// </para>
    /// </summary>
    public readonly partial struct HotKeyID : System.IEquatable<HotKeyID>
    {
        private readonly System.IntPtr _value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public HotKeyID(System.IntPtr value) => _value = value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HotKeyID(System.IntPtr value) => new HotKeyID(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(HotKeyID value) => value._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is HotKeyID other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(HotKeyID other) => _value == other._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(HotKeyID left, HotKeyID right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HotKeyID left, HotKeyID right) => !left.Equals(right);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}