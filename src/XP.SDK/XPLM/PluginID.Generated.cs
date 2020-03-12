using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// Each plug-in is identified by a unique integer ID.  This ID can be used to
    /// disable or enable a plug-in, or discover what plug-in is 'running' at the
    /// time.  A plug-in ID is unique within the currently running instance of
    /// X-Plane unless plug-ins are reloaded.  Plug-ins may receive a different
    /// unique ID each time they are loaded.
    /// </para>
    /// <para>
    /// For persistent identification of plug-ins, use XPLMFindPluginBySignature in
    /// XPLMUtiltiies.h
    /// </para>
    /// <para>
    /// -1 indicates no plug-in.
    /// </para>
    /// </summary>
    public readonly partial struct PluginID : System.IEquatable<PluginID>
    {
        private readonly int _value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public PluginID(int value) => _value = value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PluginID(int value) => new PluginID(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(PluginID value) => value._value;

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

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}