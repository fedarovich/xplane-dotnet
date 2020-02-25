using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    public readonly partial struct InstanceRef : System.IEquatable<InstanceRef>
    {
        private readonly System.IntPtr _value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public InstanceRef(System.IntPtr value) => _value = value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator InstanceRef(System.IntPtr value) => new InstanceRef(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(InstanceRef value) => value._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is InstanceRef other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(InstanceRef other) => _value == other._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(InstanceRef left, InstanceRef right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(InstanceRef left, InstanceRef right) => !left.Equals(right);
    }
}