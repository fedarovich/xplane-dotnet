using System;
using System.Diagnostics;

namespace XP.SDK
{
    /// <summary>
    /// Mark partial method returning <see cref="Utf8String"/> with this attribute
    /// in order to automatically generate the return value from the specified literal.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Conditional("SAVE_UTF8_LITERAL_IN_METADATA")]
    public sealed class Utf8StringLiteralAttribute : Attribute
    {
        public Utf8StringLiteralAttribute(string literal)
        {
            Literal = literal;
        }

        public string Literal { get; }
    }
}
