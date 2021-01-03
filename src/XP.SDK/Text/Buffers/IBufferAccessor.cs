using System;

namespace XP.SDK.Text.Buffers
{
    public interface IBufferAccessor<T>
    {
        /// <summary>
        /// Returns the data written to the underlying buffer so far, as a <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        ReadOnlyMemory<T> WrittenMemory { get; }

        /// <summary>
        /// Returns the data written to the underlying buffer so far, as a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        ReadOnlySpan<T> WrittenSpan { get; }

        /// <summary>
        /// Returns the amount of data written to the underlying buffer so far.
        /// </summary>
        int WrittenCount { get; }
    }
}
