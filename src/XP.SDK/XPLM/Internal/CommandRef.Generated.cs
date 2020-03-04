using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// A command ref is an opaque identifier for an X-Plane command. Command
    /// references stay the same for the life of your plugin but not between
    /// executions of X-Plane. Command refs are used to execute commands, create
    /// commands, and create callbacks for particular commands.
    /// </para>
    /// <para>
    /// Note that a command is not "owned" by a particular plugin. Since many
    /// plugins may participate in a command's execution, the command does not go
    /// away if the plugin that created it is unloaded.
    /// </para>
    /// </summary>
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

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}