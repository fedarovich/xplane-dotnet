using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// This is a unique ID for each menu you create.
    /// </para>
    /// </summary>
    public readonly partial struct MenuID : System.IEquatable<MenuID>
    {
        private readonly System.IntPtr _value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public MenuID(System.IntPtr value) => _value = value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MenuID(System.IntPtr value) => new MenuID(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(MenuID value) => value._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is MenuID other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(MenuID other) => _value == other._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(MenuID left, MenuID right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(MenuID left, MenuID right) => !left.Equals(right);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}