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
        /// <summary>
        /// Gets the empty UTF-8 string.
        /// </summary>
        public static Utf8String Empty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Utf8String(new byte[] {0}, 0);
        }

        /// <summary>
        /// Gets the underlying span.
        /// </summary>
        public ReadOnlySpan<byte> Span { get; }

        /// <summary>
        /// Gets the string length.
        /// </summary>
        /// <seealso cref="GetStringLength" />
        public int Length => Span.IsEmpty ? 0 : Span.Length - 1;

        /// <summary>
        /// Gets the value indicating whether the string is null.
        /// </summary>
        /// <seealso cref="IsEmpty"/>
        /// <seealso cref="IsNullOrEmpty" />
        public bool IsNull => Span.IsEmpty;

        /// <summary>
        /// Gets the value indicating whether the string is empty.
        /// </summary>
        /// <seealso cref="IsNull"/>
        /// <seealso cref="IsNullOrEmpty" />
        public bool IsEmpty => Span.Length == 1;

        /// <summary>
        /// Gets the value indicating whether the string is null or empty.
        /// </summary>
        /// <seealso cref="IsNull"/>
        /// <seealso cref="IsEmpty"/>
        public bool IsNullOrEmpty => Span.Length <= 1;

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

            Span = data;
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

                Span = data.Slice(0, length + 1);
            }
        }

        /// <summary>
        /// Initializes the string from pointer.
        /// </summary>
        /// <param name="data">The pointer to the NULL-terminated string data..</param>
        /// <remarks>
        /// <para>
        /// If the <paramref name="data"/> is <see langword="null"/>, <see cref="Utf8String.IsNull"/> property of the resulting string will be null.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">The length of the string exceeds 2147483646.</exception>
        public unsafe Utf8String(byte* data)
        {
            if (data == null)
            {
                this = default;
            }
            else
            {
                var length = (int) Utils.CStringLength(data, int.MaxValue);
                if (length == int.MaxValue)
                    throw new ArgumentException("The string length must not exceed 2147483646.");
                
                Span = new ReadOnlySpan<byte>(data, length + 1);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Utf8String"/> from the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="pinned">If <see langword="true"/>, the underlying memory buffer will be allocated in the Pinned Object Heap, instead of normal heap.</param>
        /// <remarks>The actual string data is stored in a managed array.</remarks>
        public static Utf8String FromString(string? str, bool pinned = false)
        {
            if (str == null)
                return default;

            var length = Utils.UTF8WithoutPreamble.GetByteCount(str);
            var buffer = GC.AllocateUninitializedArray<byte>(length + 1, pinned);
            Utils.UTF8WithoutPreamble.GetBytes(str, buffer);
            buffer[length] = 0;
            return new Utf8String(buffer, length);
        }

        /// <summary>
        /// Creates a new <see cref="Utf8String"/> from the span containing UTF-16 chars.
        /// </summary>
        /// <param name="chars">The span with char data.</param>
        /// <param name="pinned">If <see langword="true"/>, the underlying memory buffer will be allocated in the Pinned Object Heap, instead of normal heap.</param>
        /// <remarks>The actual string data is stored in a managed array.</remarks>
        public static Utf8String FromUtf16(in ReadOnlySpan<char> chars, bool pinned = false)
        {
            if (chars.IsEmpty)
                return default;

            var length = Utils.UTF8WithoutPreamble.GetByteCount(chars);
            var buffer = GC.AllocateUninitializedArray<byte>(length + 1, pinned);
            Utils.UTF8WithoutPreamble.GetBytes(chars, buffer);
            buffer[length] = 0;
            return new Utf8String(buffer, length);
        }

        /// <inheritdoc />
        public override string? ToString() => Span.IsEmpty ? null : Utils.UTF8WithoutPreamble.GetString(Span[..^1]);

        /// <summary>
        /// Compares whether two UTF8 strings are equal.
        /// </summary>
        /// <param name="other">The string to compare with.</param>
        public bool Equals(in Utf8String other) => Span.Length == other.Span.Length && Span.SequenceEqual(other.Span);

        /// <summary>
        /// Returns a reference to the 0th element of the UTF-8 string. If the string is null (i.e. the underlying span is empty), returns null reference.
        /// It can be used for pinning and is required to support the use of span within a fixed statement.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly byte GetPinnableReference() => ref Span.GetPinnableReference();

        /// <summary>
        /// Converts the <see cref="Utf8String"/> to <see cref="string"/>.
        /// </summary>
        public static explicit operator string?(in Utf8String str) => str.ToString();

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
        public int IndexOf(in Utf8String substring) => substring.IsNullOrEmpty ? 0 : Span.IndexOf(substring.Span[..^1]);

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified substring in this instance.
        /// </summary>
        /// <param name="substring">The substring to seek.</param>
        /// <returns>The zero-based index position of value if that substring is found, or <c>-1</c> if it is not. If value is Null or Empty, the return value is <c>-1</c>.</returns>
        public int LastIndexOf(in Utf8String substring) => substring.IsNullOrEmpty ? -1 : Span.LastIndexOf(substring.Span[..^1]);

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified string.
        /// </summary>
        /// <param name="substring">The string to compare.</param>
        /// <returns><see langword="true"/> if substring matches the beginning of this string; otherwise, <see langword="false"/></returns>
        public bool StartsWith(in Utf8String substring) => substring.IsNullOrEmpty || Span.StartsWith(substring.Span[..^1]);

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string.
        /// </summary>
        /// <param name="substring">The string to compare.</param>
        /// <returns><see langword="true"/> if substring matches the end of this string; otherwise, <see langword="false"/></returns>
        public bool EndsWith(in Utf8String substring) => substring.IsNullOrEmpty || Span.EndsWith(substring.Span[..^1]);

        /// <summary>
        /// Copies the data to the destination.
        /// </summary>
        /// <param name="destination">The destination to copy the data to. The destination buffer must be at least <c>Length + 1</c> bytes long.</param>
        public unsafe void CopyTo(byte* destination)
        {
            fixed (byte* source = Span)
            {
                Buffer.MemoryCopy(source, destination, Span.Length, Span.Length);
            }
        }

        /// <summary>
        /// Returns the length of the C string.
        /// </summary>
        /// <returns>
        /// <para>The length of the C string is this object contains a valid C-string; <c>-1</c> otherwise.</para>
        /// <para>If <see cref="IsNull"/>, returns <c>0</c>.</para>
        /// </returns>
        /// <remarks>
        /// The length of a C string is determined by the terminating null-character:
        /// A C string is as long as the number of characters between the beginning of the string and the terminating null character
        /// (without including the terminating null character itself).
        /// </remarks>
        /// <seealso cref="Length"/>
        public unsafe int GetStringLength()
        {
            if (Span.IsEmpty)
                return 0;

            fixed (byte* str = Span)
            {
                var length = (int) Utils.CStringLength(str, (nuint) Span.Length);
                return length < Span.Length ? length : -1;
            }
        }
    }
}
