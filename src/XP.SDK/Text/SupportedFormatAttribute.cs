using System;

namespace XP.SDK.Text
{
    /// <summary>
    /// Identifies one of the format symbols supported by <see cref="IUtf8Formattable"/> implementation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
    public class SupportedFormatAttribute : Attribute
    {
        /// <summary>
        /// Initialized a new instance of <see cref="SupportedFormatAttribute"/>.
        /// </summary>
        /// <param name="format">The supported format symbol.</param>
        public SupportedFormatAttribute(char format)
        {
            Format = format;
        }

        /// <summary>
        /// Gets the supported format symbol.
        /// </summary>
        public char Format { get; }

        /// <summary>
        /// Gets or sets the optional description.
        /// </summary>
        public string Description { get; set; }
    }
}
