using System;
using System.Buffers;
using XP.SDK.Text.Formatters;

namespace XP.SDK.Text
{
    public static class Utf8StringBuilderExtensions
    {
        /// <summary>
        /// Append the value of enum type <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="builder">An instance of <see cref="Utf8StringBuilder"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="format">The format.</param>
        public static void Append<TEnum>(this in Utf8StringBuilder builder, TEnum value, StandardFormat format = default)
            where TEnum : unmanaged, Enum
        {
            builder.Append(new EnumFormatter<TEnum>(value), format);
        }
    }
}
