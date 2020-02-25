using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.Widgets.Internal
{
    public readonly partial struct WidgetID : System.IEquatable<WidgetID>
    {
        private readonly System.IntPtr _value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public WidgetID(System.IntPtr value) => _value = value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator WidgetID(System.IntPtr value) => new WidgetID(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(WidgetID value) => value._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is WidgetID other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(WidgetID other) => _value == other._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(WidgetID left, WidgetID right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(WidgetID left, WidgetID right) => !left.Equals(right);
    }
}