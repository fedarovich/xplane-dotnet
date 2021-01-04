#nullable enable
using System;
using System.Diagnostics;

namespace XP.SDK
{
    /// <summary>
    /// Mark partial method returning <see cref="Utf8String"/> or <see cref="Utf8StringScope"/> with this attribute
    /// in order to automatically generate the return value from the specified format string and method parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [Conditional("SAVE_UTF8_FORMAT_IN_METADATA")]
    public class Utf8StringFormatAttribute : Attribute
    {
        public Utf8StringFormatAttribute(string format)
        {
            Format = format;
        }
        
        public string Format { get; }

        public int InitialBufferCapacity { get; set; }
        
        public string? NullDisplayText { get; set; }
    }
}
