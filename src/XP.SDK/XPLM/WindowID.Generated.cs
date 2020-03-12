using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// This is an opaque identifier for a window.  You use it to control your
    /// window. When you create a window (via either XPLMCreateWindow() or
    /// XPLMCreateWindowEx()), you will specify callbacks to handle drawing, mouse
    /// interaction, etc.
    /// </para>
    /// </summary>
    public readonly partial struct WindowID : System.IEquatable<WindowID>
    {
        private readonly System.IntPtr _value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public WindowID(System.IntPtr value) => _value = value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator WindowID(System.IntPtr value) => new WindowID(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(WindowID value) => value._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is WindowID other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(WindowID other) => _value == other._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(WindowID left, WindowID right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(WindowID left, WindowID right) => !left.Equals(right);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}