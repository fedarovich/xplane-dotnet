using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.Widgets.Internal
{
    
    /// <summary>
    /// <para>
    /// A Widget ID is an opaque unique non-zero handle identifying your widget.
    /// Use 0 to specify "no widget". This type is defined as wide enough to hold a
    /// pointer. You receive a widget ID when you create a new widget and then use
    /// that widget ID to further refer to the widget.
    /// </para>
    /// </summary>
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