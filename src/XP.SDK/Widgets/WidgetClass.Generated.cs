using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.Widgets
{
    
    /// <summary>
    /// <para>
    /// Widget classes define predefined widget types. A widget class basically
    /// specifies from a library the widget function to be used for the widget.
    /// Most widgets can be made right from classes.
    /// </para>
    /// </summary>
    public readonly partial struct WidgetClass : System.IEquatable<WidgetClass>
    {
        private readonly int _value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public WidgetClass(int value) => _value = value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator WidgetClass(int value) => new WidgetClass(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(WidgetClass value) => value._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is WidgetClass other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(WidgetClass other) => _value == other._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(WidgetClass left, WidgetClass right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(WidgetClass left, WidgetClass right) => !left.Equals(right);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}