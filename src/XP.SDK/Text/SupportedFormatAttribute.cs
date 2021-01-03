using System;

namespace XP.SDK.Text
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
    public class SupportedFormatAttribute : Attribute
    {
        public SupportedFormatAttribute(char format)
        {
            Format = format;
        }

        public char Format { get; }

        public string Description { get; set; }
    }
}
