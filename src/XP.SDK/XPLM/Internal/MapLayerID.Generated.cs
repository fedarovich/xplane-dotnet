using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    public readonly partial struct MapLayerID : System.IEquatable<MapLayerID>
    {
        private readonly System.IntPtr _value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public MapLayerID(System.IntPtr value) => _value = value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MapLayerID(System.IntPtr value) => new MapLayerID(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(MapLayerID value) => value._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is MapLayerID other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(MapLayerID other) => _value == other._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(MapLayerID left, MapLayerID right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(MapLayerID left, MapLayerID right) => !left.Equals(right);
    }
}