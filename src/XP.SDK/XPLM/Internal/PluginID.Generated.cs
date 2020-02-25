using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    public readonly partial struct PluginID : System.IEquatable<PluginID>
    {
        private readonly int _value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public PluginID(int value) => _value = value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PluginID(int value) => new PluginID(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int (PluginID value) => value._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is PluginID other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(PluginID other) => _value == other._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PluginID left, PluginID right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PluginID left, PluginID right) => !left.Equals(right);
    }
}