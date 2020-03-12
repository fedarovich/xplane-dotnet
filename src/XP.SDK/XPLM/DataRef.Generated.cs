using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// A data ref is an opaque handle to data provided by the simulator or another
    /// plugin. It uniquely identifies one variable (or array of variables) over
    /// the lifetime of your plugin. You never hard code these values; you always
    /// get them from XPLMFindDataRef.
    /// </para>
    /// </summary>
    public readonly partial struct DataRef : System.IEquatable<DataRef>
    {
        private readonly System.IntPtr _value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public DataRef(System.IntPtr value) => _value = value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator DataRef(System.IntPtr value) => new DataRef(value);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.IntPtr(DataRef value) => value._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is DataRef other && Equals(other);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _value.GetHashCode();
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(DataRef other) => _value == other._value;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(DataRef left, DataRef right) => left.Equals(right);
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(DataRef left, DataRef right) => !left.Equals(right);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _value.ToString();
    }
}