using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XP.SDK.Text.Buffers
{
    public sealed class MemoryPoolBufferWriter<T> : ICompletableBufferWriter<T>, IDisposable
    {
        private MemoryPool<T> _pool;
        private readonly bool _disposePool;
        private IMemoryOwner<T> _buffer;
        private int _index;

        private const int DefaultInitialBufferSize = 256;

        /// <summary>
        /// Creates an instance of an <see cref="MemoryPoolBufferWriter{T}"/>, in which data can be written to,
        /// with the default initial capacity.
        /// </summary>
        /// <param name="pool">The <see cref="MemoryPool{T}"/> to get arrays from.</param>
        /// <param name="disposePool">The value indicating whether the <paramref name="pool"/> must be disposed while disposing this instance.</param>
        public MemoryPoolBufferWriter(MemoryPool<T> pool, bool disposePool)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _disposePool = disposePool;
            _buffer = _pool.Rent(0);
            _index = 0;
        }

        /// <summary>
        /// Creates an instance of an <see cref="MemoryPoolBufferWriter{T}"/>, in which data can be written to,
        /// with an initial capacity specified.
        /// </summary>
        /// <param name="initialCapacity">The minimum capacity with which to initialize the underlying buffer.</param>
        /// <param name="pool">The <see cref="MemoryPool{T}"/> to get arrays from.</param>
        /// <param name="disposePool">The value indicating whether the <paramref name="pool"/> must be disposed while disposing this instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="initialCapacity"/> is not positive (i.e. less than or equal to 0).
        /// </exception>
        public MemoryPoolBufferWriter(MemoryPool<T> pool, bool disposePool, int initialCapacity)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _disposePool = disposePool;
            if (initialCapacity <= 0)
                throw new ArgumentException(null, nameof(initialCapacity));

            _buffer = _pool.Rent(initialCapacity);
            _index = 0;
        }

        /// <summary>
        /// Returns the data written to the underlying buffer so far, as a <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        public ReadOnlyMemory<T> WrittenMemory
        {
            get
            {
                CheckNotDisposed();
                return _buffer.Memory.Slice(0, _index);
            }
        }

        /// <summary>
        /// Returns the data written to the underlying buffer so far, as a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        public ReadOnlySpan<T> WrittenSpan
        {
            get
            {
                CheckNotDisposed();
                return _buffer.Memory.Span.Slice(0, _index);
            }
        }

        /// <summary>
        /// Returns the amount of data written to the underlying buffer so far.
        /// </summary>
        public int WrittenCount
        {
            get
            {
                CheckNotDisposed();
                return _index;
            }
        }

        /// <summary>
        /// Returns the total amount of space within the underlying buffer.
        /// </summary>
        public int Capacity
        {
            get
            {
                CheckNotDisposed();
                return _buffer.Memory.Length;
            }
        }

        /// <summary>
        /// Returns the amount of space available that can still be written into without forcing the underlying buffer to grow.
        /// </summary>
        public int FreeCapacity
        {
            get
            {
                CheckNotDisposed();
                return _buffer.Memory.Length - _index;
            }
        }

        /// <summary>
        /// Clears the data written to the underlying buffer.
        /// </summary>
        /// <remarks>
        /// You must clear the <see cref="ArrayBufferWriter{T}"/> before trying to re-use it.
        /// </remarks>
        public void Clear()
        {
            CheckNotDisposed();
            CheckNotCompleted();

            Debug.Assert(_buffer.Memory.Length >= _index);
            _buffer.Memory.Span.Slice(0, _index).Clear();
            _index = 0;
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
            CheckNotDisposed();
            CheckNotCompleted();

            if (count < 0)
                throw new ArgumentException(null, nameof(count));

            if (_index > _buffer.Memory.Length - count)
                ThrowInvalidOperationException_AdvancedTooFar(_buffer.Memory.Length);

            _index += count;
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
            CheckNotDisposed();
            CheckNotCompleted();
            CheckAndResizeBuffer(sizeHint);
            Debug.Assert(_buffer.Memory.Length > _index);
            return _buffer.Memory.Slice(_index);
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
            CheckNotDisposed();
            CheckNotCompleted();
            CheckAndResizeBuffer(sizeHint);
            Debug.Assert(_buffer.Memory.Length > _index);
            return _buffer.Memory.Span.Slice(_index);
        }

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

        /// <inheritdoc />
        public void Dispose()
        {
            if (_pool != null)
            {
                _buffer.Dispose();
                _buffer = null;
                if (_disposePool)
                {
                    _pool.Dispose();
                }
                _pool = null;
            }
        }

        private void CheckAndResizeBuffer(int sizeHint)
        {
            if (sizeHint < 0)
                throw new ArgumentException(nameof(sizeHint));

            if (sizeHint == 0)
            {
                sizeHint = 1;
            }

            if (sizeHint > FreeCapacity)
            {
                int currentLength = _buffer.Memory.Length;
                int growBy = Math.Max(sizeHint, currentLength);

                if (currentLength == 0)
                {
                    growBy = Math.Max(growBy, DefaultInitialBufferSize);
                }

                int newSize = currentLength + growBy;

                if ((uint)newSize > int.MaxValue)
                {
                    newSize = currentLength + sizeHint;
                    if ((uint)newSize > int.MaxValue)
                    {
                        ThrowOutOfMemoryException((uint)newSize);
                    }
                }

                var oldBuffer = _buffer;
                _buffer = _pool.Rent(newSize);
                oldBuffer.Memory.CopyTo(_buffer.Memory);
                oldBuffer.Dispose();
            }

            Debug.Assert(FreeCapacity > 0 && FreeCapacity >= sizeHint);
        }

        private static void ThrowInvalidOperationException_AdvancedTooFar(int capacity)
        {
            throw new InvalidOperationException($"Cannot advance past the end of the buffer, which has a size of {capacity}.");
        }

        private static void ThrowOutOfMemoryException(uint capacity)
        {
            throw new OutOfMemoryException($"Cannot allocate a buffer of size {capacity}.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckNotCompleted()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("The writer is complete.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckNotDisposed()
        {
            if (_pool == null)
            {
                throw new ObjectDisposedException(nameof(MemoryPoolBufferWriter<T>));
            }
        }
    }
}
