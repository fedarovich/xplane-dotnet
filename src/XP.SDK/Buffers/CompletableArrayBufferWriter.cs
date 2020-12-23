#nullable enable
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace XP.SDK.Buffers
{
    public class CompletableArrayBufferWriter<T> : ICompletableBufferWriter<T>
    {
        private readonly ArrayBufferWriter<T> _writer;

        public CompletableArrayBufferWriter() 
            : this(new ArrayBufferWriter<T>())
        {
        }

        public CompletableArrayBufferWriter(int initialCapacity)
            : this(new ArrayBufferWriter<T>(initialCapacity))
        {
        }

        internal CompletableArrayBufferWriter(ArrayBufferWriter<T> writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Notifies <see cref="IBufferWriter{T}"/> that <paramref name="count"/> amount of data was written to the output <see cref="Span{T}"/>/<see cref="Memory{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="count"/> is negative.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attempting to advance past the end of the underlying buffer.
        /// </exception>
        /// <remarks>
        /// You must request a new buffer after calling Advance to continue writing more data and cannot write to a previously acquired buffer.
        /// </remarks>
        public void Advance(int count)
        {
            CheckNotCompleted();
            _writer.Advance(count);
        }

        /// <summary>
        /// Returns a <see cref="Memory{T}"/> to write to that is at least the requested length (specified by <paramref name="sizeHint"/>).
        /// If no <paramref name="sizeHint"/> is provided (or it's equal to <code>0</code>), some non-empty buffer is returned.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="sizeHint"/> is negative.
        /// </exception>
        /// <remarks>
        /// This will never return an empty <see cref="Memory{T}"/>.
        /// </remarks>
        /// <remarks>
        /// There is no guarantee that successive calls will return the same buffer or the same-sized buffer.
        /// </remarks>
        /// <remarks>
        /// You must request a new buffer after calling Advance to continue writing more data and cannot write to a previously acquired buffer.
        /// </remarks>
        public Memory<T> GetMemory(int sizeHint = 0)
        {
            CheckNotCompleted();
            return _writer.GetMemory(sizeHint);
        }

        /// <summary>
        /// Returns a <see cref="Span{T}"/> to write to that is at least the requested length (specified by <paramref name="sizeHint"/>).
        /// If no <paramref name="sizeHint"/> is provided (or it's equal to <code>0</code>), some non-empty buffer is returned.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="sizeHint"/> is negative.
        /// </exception>
        /// <remarks>
        /// This will never return an empty <see cref="Span{T}"/>.
        /// </remarks>
        /// <remarks>
        /// There is no guarantee that successive calls will return the same buffer or the same-sized buffer.
        /// </remarks>
        /// <remarks>
        /// You must request a new buffer after calling Advance to continue writing more data and cannot write to a previously acquired buffer.
        /// </remarks>
        public Span<T> GetSpan(int sizeHint = 0)
        {
            CheckNotCompleted();
            return _writer.GetSpan(sizeHint);
        }

        /// <summary>
        /// Returns the data written to the underlying buffer so far, as a <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        public ReadOnlyMemory<T> WrittenMemory => _writer.WrittenMemory;

        /// <summary>
        /// Returns the data written to the underlying buffer so far, as a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        public ReadOnlySpan<T> WrittenSpan => _writer.WrittenSpan;

        /// <summary>
        /// Returns the amount of data written to the underlying buffer so far.
        /// </summary>
        public int WrittenCount => _writer.WrittenCount;
        
        /// <summary>
        /// Checks whether the writer is completed.
        /// </summary>
        /// <seealso cref="Complete"/>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Completes the writer, so that no more write operations can be done.
        /// </summary>
        /// <seealso cref="IsCompleted"/>
        public void Complete() => IsCompleted = true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckNotCompleted()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("The writer is complete.");
            }
        }
    }
}
