using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XP.SDK.Text.Formatters
{
    [SupportedFormat('g'), SupportedFormat('G')]
    [SupportedFormat('f'), SupportedFormat('F')]
    [SupportedFormat('d'), SupportedFormat('D')]
    [SupportedFormat('x'), SupportedFormat('X')]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public readonly unsafe struct EnumFormatter<TEnum> : IUtf8Formattable
        where TEnum : unmanaged, Enum
    {
        private static readonly delegate*<TEnum, in Span<byte>, out int, StandardFormat, bool> TryFormatAsNumber;
        private static bool IsFlags;

        public TEnum Value { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EnumFormatter(TEnum value) => Value = value;

        static EnumFormatter()
        {
            IsFlags = typeof(TEnum).GetCustomAttribute<FlagsAttribute>() != null;
            var baseType = Enum.GetUnderlyingType(typeof(TEnum));
            
            if (baseType == typeof(byte))
                TryFormatAsNumber = &TryFormatAsByte;
            else if (baseType == typeof(short))
                TryFormatAsNumber = &TryFormatAsInt16;
            else if (baseType == typeof(int))
                TryFormatAsNumber = &TryFormatAsInt32;
            else if (baseType == typeof(long))
                TryFormatAsNumber = &TryFormatAsInt64;
            else if (baseType == typeof(sbyte))
                TryFormatAsNumber = &TryFormatAsSByte;
            else if (baseType == typeof(ushort))
                TryFormatAsNumber = &TryFormatAsUInt16;
            else if (baseType == typeof(uint))
                TryFormatAsNumber = &TryFormatAsUInt32;
            else if (baseType == typeof(ulong))
                TryFormatAsNumber = &TryFormatAsUInt64;
            else
                throw new ArgumentException("Unknown underlying base type of enum.");
        }
        
        public bool TryFormat(in Span<byte> destination, out int bytesWritten, StandardFormat format = default)
        {
            var symbol = GetSymbolOrDefault(format, 'G');
            switch (symbol)
            {
                case 'G' or 'g':
                    if (EnumStringCache.Values.TryGetValue(Value, out var strMemory))
                    {
                        if (destination.Length >= strMemory.Length)
                        {
                            strMemory.Span.CopyTo(destination);
                            bytesWritten = strMemory.Span.Length;
                            return true;
                        }
                    }
                    else if (IsFlags)
                    {
                        return TryFormatAsFlags(Value, destination, out bytesWritten); ;
                    }
                    else
                    {
                        return TryFormatAsNumber(Value, destination, out bytesWritten, new StandardFormat('D'));
                    }
                    break;
                case 'F' or 'f':
                    if (EnumStringCache.Values.TryGetValue(Value, out var flagsMemory))
                    {
                        if (destination.Length >= flagsMemory.Length)
                        {
                            flagsMemory.Span.CopyTo(destination);
                            bytesWritten = flagsMemory.Span.Length;
                            return true;
                        }
                    }
                    else
                    {
                        return TryFormatAsFlags(Value, destination, out bytesWritten);
                    }
                    break;
                case 'X' or 'x' or 'D' or 'd':
                    return TryFormatAsNumber(Value, destination, out bytesWritten, format);
                default:
                    throw new FormatException($"Unsupported format '{symbol}.'");
            }

            bytesWritten = 0;
            return false;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static bool TryFormatAsFlags(TEnum value, in Span<byte> destination, out int bytesWritten)
            {
                var flagsString = Utf8String.FromString(value.ToString("F"));
                if (destination.Length >= flagsString.Length)
                {
                    flagsString.Span.Slice(0, flagsString.Length).CopyTo(destination);
                    bytesWritten = flagsString.Length;
                    return true;
                }

                bytesWritten = 0;
                return false;
            }
        }

        private static bool TryFormatAsByte(TEnum enumValue, in Span<byte> destination, out int writtenCount, StandardFormat format)
        {
            var value = Unsafe.As<TEnum, byte>(ref enumValue);
            return Utf8Formatter.TryFormat(value, destination, out writtenCount, format);
        }

        private static bool TryFormatAsInt16(TEnum enumValue, in Span<byte> destination, out int writtenCount, StandardFormat format)
        {
            var value = Unsafe.As<TEnum, short>(ref enumValue);
            return Utf8Formatter.TryFormat(value, destination, out writtenCount, format);
        }

        private static bool TryFormatAsInt32(TEnum enumValue, in Span<byte> destination, out int writtenCount, StandardFormat format)
        {
            var value = Unsafe.As<TEnum, int>(ref enumValue);
            return Utf8Formatter.TryFormat(value, destination, out writtenCount, format);
        }

        private static bool TryFormatAsInt64(TEnum enumValue, in Span<byte> destination, out int writtenCount, StandardFormat format)
        {
            var value = Unsafe.As<TEnum, long>(ref enumValue);
            return Utf8Formatter.TryFormat(value, destination, out writtenCount, format);
        }

        private static bool TryFormatAsSByte(TEnum enumValue, in Span<byte> destination, out int writtenCount, StandardFormat format)
        {
            var value = Unsafe.As<TEnum, sbyte>(ref enumValue);
            return Utf8Formatter.TryFormat(value, destination, out writtenCount, format);
        }

        private static bool TryFormatAsUInt16(TEnum enumValue, in Span<byte> destination, out int writtenCount, StandardFormat format)
        {
            var value = Unsafe.As<TEnum, ushort>(ref enumValue);
            return Utf8Formatter.TryFormat(value, destination, out writtenCount, format);
        }

        private static bool TryFormatAsUInt32(TEnum enumValue, in Span<byte> destination, out int writtenCount, StandardFormat format)
        {
            var value = Unsafe.As<TEnum, uint>(ref enumValue);
            return Utf8Formatter.TryFormat(value, destination, out writtenCount, format);
        }

        private static bool TryFormatAsUInt64(TEnum enumValue, in Span<byte> destination, out int writtenCount, StandardFormat format)
        {
            var value = Unsafe.As<TEnum, ulong>(ref enumValue);
            return Utf8Formatter.TryFormat(value, destination, out writtenCount, format);
        }

        /// <summary>
        /// Returns the symbol contained within the standard format. If the standard format
        /// has not been initialized, returns the provided fallback symbol.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char GetSymbolOrDefault(in StandardFormat format, char defaultSymbol)
        {
            // This is equivalent to the line below, but it is written in such a way
            // that the JIT is able to perform more optimizations.
            //
            // return (format.IsDefault) ? defaultSymbol : format.Symbol;

            char symbol = format.Symbol;
            if (symbol == default && format.Precision == default)
            {
                symbol = defaultSymbol;
            }
            return symbol;
        }

        private static class EnumStringCache
        {
            public static readonly IReadOnlyDictionary<TEnum, ReadOnlyMemory<byte>> Values;

            static EnumStringCache()
            {
                var values = Enum.GetValues<TEnum>();
                var dictionary = new Dictionary<TEnum, ReadOnlyMemory<byte>>(values.Length);
                foreach (var value in values)
                {
                    var str = value.ToString("G");
                    var bytes = Utils.UTF8WithoutPreamble.GetBytes(str);
                    dictionary.Add(value, bytes);
                }

                Values = dictionary;
            }
        }
    }
}
