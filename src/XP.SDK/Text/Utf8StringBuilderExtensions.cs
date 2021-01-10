using System;
using System.Buffers;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// Appends the value of type implementing <see cref="IUtf8Formattable"/> interface.
        /// </summary>
        /// <typeparam name="T">The formattable type.</typeparam>
        /// <param name="builder">An instance of <see cref="Utf8StringBuilder"/>.</param>
        /// <param name="value">The formattable value.</param>
        /// <param name="format">The format.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Append<T>(this in Utf8StringBuilder builder, ref T value, StandardFormat format = default)
            where T : IUtf8Formattable
        {
            builder.AppendRef(ref value, format);
        }
    }
}
