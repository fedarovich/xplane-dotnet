using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace XP.SDK
{
    /// <summary>
    /// Represents null-terminated C-style string in UTF-8 encoding.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    public readonly ref struct Utf8String
    {
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
        /// If the <paramref name="data"/> is empty, <see cref="Utf8String.IsNull"/> of the resulting string will be null.
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

        public override string ToString() => Data.IsEmpty ? null : Encoding.UTF8.GetString(Data[..^1]);

        public bool Equals(in Utf8String other) => Data.Length == other.Data.Length && Data.SequenceEqual(other.Data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly byte GetPinnableReference() => ref Data.GetPinnableReference();
    }
}
