using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    public readonly partial struct CommandRef : System.IEquatable<CommandRef>
    {
        private readonly System.IntPtr _value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public CommandRef(System.IntPtr value) => _value = value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CommandRef(System.IntPtr value) => new CommandRef(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(CommandRef value) => value._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is CommandRef other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CommandRef other) => _value == other._value;
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CommandRef left, CommandRef right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CommandRef left, CommandRef right) => !left.Equals(right);
    }
}