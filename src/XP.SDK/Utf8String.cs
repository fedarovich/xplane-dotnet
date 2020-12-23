#nullable enable
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace XP.SDK
{
    /// <summary>
    /// Represents null-terminated C-style string in UTF-8 encoding.
    /// </summary>
    public readonly ref struct Utf8String
    {
        private static readonly UTF8Encoding UTF8 = new (false);
        
        /// <summary>
        /// Gets the underlying span.
        /// </summary>
        public ReadOnlySpan<byte> Data { get; }

        /// <summary>
        /// Gets the string length.
        /// </summary>
        public int Length => Data.IsEmpty ? 0 : Data.Length - 1;

        /// <summary>
        /// Gets the value indicating whether the string is null.
        /// </summary>
        public bool IsNull => Data.IsEmpty;

        /// <summary>
        /// Gets the value indicating whether the string is null or empty.
        /// </summary>
        public bool IsNullOrEmpty => Data.Length <= 1;

        /// <summary>
        /// Initializes the string from read-only span.
        /// </summary>
        /// <param name="data">The read-only span containing the UTF-8 encoded string data.</param>
        /// <param name="length">The length of the string.</param>
        /// <remarks>
        /// <para>
        /// The <paramref name="data"/> must be of at least <c>length + 1</c> bytes long and must contain value <c>0</c> at position <paramref name="length"/>.
        /// This <c>0</c> value must be the first one in the string.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">The <paramref name="data"/> is empty.</exception>
        /// <exception cref="ArgumentException">The <paramref name="data"/> does not contain <c>0</c> at position <paramref name="length"/>.</exception>
        /// <exception cref="IndexOutOfRangeException">The <paramref name="length"/> is less than 0 or greater than or equal to <c>data.Length</c></exception>
        public Utf8String(in ReadOnlySpan<byte> data, int length)
        {
            if (data.IsEmpty)
                throw new ArgumentException("The data span cannot be empty.");

            if (data[length] != 0)
                throw new ArgumentException("The data span must have value 0 at index length.");

            Data = data;
        }

        /// <summary>
        /// Initializes the string from read-only span.
        /// </summary>
        /// <param name="data">The read-only span containing the data.</param>
        /// <remarks>
        /// <para>
        /// If the <paramref name="data"/> is empty, <see cref="Utf8String.IsNull"/> property of the resulting string will be null.
        /// </para>
        /// <para>
        /// The <paramref name="data"/> must contain at least one byte with the value <c>0</c>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">The <paramref name="data"/> does not contain zero byte.</exception>
        public Utf8String(in ReadOnlySpan<byte> data)
        {
            if (data.IsEmpty)
            {
                this = default;
            }
            else
            {
                var length = data.IndexOf((byte) 0);
                if (length < 0)
                    throw new ArgumentException("The data must be a null-terminated string.");

                Data = data.Slice(0, length + 1);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Utf8String"/> from the string.
        /// </summary>
        /// <remarks>The actual string data is stored in a managed array.</remarks>
        public static Utf8String FromString(string? str)
        {
            if (str == null)
                return default;

            var length = UTF8.GetByteCount(str);
            var buffer = new byte[length + 1];
            UTF8.GetBytes(str, buffer);
            return new Utf8String(buffer, length);
        }

        /// <inheritdoc />
        public override string? ToString() => Data.IsEmpty ? null : UTF8.GetString(Data[..^1]);

        /// <summary>
        /// Compares whether two UTF8 strings are equal.
        /// </summary>
        /// <param name="other">The string to compare with.</param>
        public bool Equals(in Utf8String other) => Data.Length == other.Data.Length && Data.SequenceEqual(other.Data);

        /// <summary>
        /// Returns a reference to the 0th element of the UTF-8 string. If the string is null (i.e. the underlying span is empty), returns null reference.
        /// It can be used for pinning and is required to support the use of span within a fixed statement.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly byte GetPinnableReference() => ref Data.GetPinnableReference();

        /// <summary>
        /// Converts the <see cref="Utf8String"/> to <see cref="string"/>.
        /// </summary>
        public static implicit operator string?(in Utf8String str) => str.ToString();

        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this string.
        /// </summary>
        /// <param name="substring">The substring to seek.</param>
        /// <returns><see langword="true"/> if the value parameter occurs within this string, or if value is the empty string; otherwise, <see langword="false"/>.</returns>
        public bool Contains(in Utf8String substring) => IndexOf(substring) >= 0;

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified substring in this instance.
        /// </summary>
        /// <param name="substring">The substring to seek.</param>
        /// <returns>The zero-based index position of value if that substring is found, or <c>-1</c> if it is not. If value is Null or Empty, the return value is <c>0</c>.</returns>
        public int IndexOf(in Utf8String substring) => substring.IsNullOrEmpty ? 0 : Data.IndexOf(substring.Data[..^1]);

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified substring in this instance.
        /// </summary>
        /// <param name="substring">The substring to seek.</param>
        /// <returns>The zero-based index position of value if that substring is found, or <c>-1</c> if it is not. If value is Null or Empty, the return value is <c>-1</c>.</returns>
        public int LastIndexOf(in Utf8String substring) => substring.IsNullOrEmpty ? -1 : Data.LastIndexOf(substring.Data[..^1]);

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified string.
        /// </summary>
        /// <param name="substring">The string to compare.</param>
        /// <returns><see langword="true"/> if substring matches the beginning of this string; otherwise, <see langword="false"/></returns>
        public bool StartsWith(in Utf8String substring) => substring.IsNullOrEmpty || Data.StartsWith(substring.Data[..^1]);

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string.
        /// </summary>
        /// <param name="substring">The string to compare.</param>
        /// <returns><see langword="true"/> if substring matches the end of this string; otherwise, <see langword="false"/></returns>

        public bool EndsWith(in Utf8String substring) => substring.IsNullOrEmpty || Data.EndsWith(substring.Data[..^1]);
    }
}
