using System;
using System.Buffers;
using XP.SDK.Buffers;

namespace XP.SDK
{
    /// <summary>
    /// Provides the means to create <see cref="Utf8StringBuilder"/>.
    /// </summary>
    /// <example>
    /// The following example shows how you can use <see cref="Utf8StringBuilderFactory"/>
    /// to create a <see cref="Utf8StringBuilder"/> and build a <see cref="Utf8String"/> with it.
    /// <code>
    /// <![CDATA[
    /// void SayHello(Utf8StringBuilderFactory factory)
    /// {
    ///     using Utf8StringBuilder builder = factory.CreateBuilder();
    ///     builder.Append("Hello, world!");
    ///     builder.AppendLine();
    ///     Utf8String str = builder.Complete();
    ///     XPlane.Trace.Write(str);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class Utf8StringBuilderFactory : IDisposable
    {
        private const int DefaultCapacity = 256;

        private readonly object _pool;
        private readonly bool _disposePool;
        
        /// <summary>
        /// Initializes a new instance of <see cref="Utf8StringBuilderFactory"/> that will use a new managed array for each builder.
        /// </summary>
        public Utf8StringBuilderFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Utf8StringBuilderFactory"/> that will use managed arrays taken from the <paramref name="arrayPool"/> for each builder.
        /// </summary>
        /// <param name="arrayPool">The array pool to get arrays for the builder from.</param>
        public Utf8StringBuilderFactory(ArrayPool<byte> arrayPool) : this(arrayPool, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Utf8StringBuilderFactory"/> that will use managed arrays taken from the <paramref name="arrayPool"/> for each builder.
        /// </summary>
        /// <param name="arrayPool">The array pool to get arrays for the builder from.</param>
        /// <param name="disposePool">
        /// The value indicating whether the <paramref name="arrayPool"/> must be disposed while disposing this factory.
        /// If the array pool does not implement <see cref="IDisposable"/>, this parameter is ignored.
        /// </param>
        public Utf8StringBuilderFactory(ArrayPool<byte> arrayPool, bool disposePool)
        {
            _pool = arrayPool;
            _disposePool = disposePool;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Utf8StringBuilderFactory"/> that will use memory taken from the <paramref name="memoryPool"/> for each builder.
        /// </summary>
        /// <param name="memoryPool">The memory pool to get memory for the builder from.</param>
        /// <param name="disposePool">
        /// The value indicating whether the <paramref name="memoryPool"/> must be disposed while disposing this factory.
        /// </param>
        public Utf8StringBuilderFactory(MemoryPool<byte> memoryPool, bool disposePool)
        {
            _pool = memoryPool;
            _disposePool = disposePool;
        }

        /// <summary>
        /// Gets the shared instance of <see cref="Utf8StringBuilderFactory"/> that will use a new managed array for each builder.
        /// </summary>
        public static Utf8StringBuilderFactory Shared => SharedHolder.Instance;

        /// <summary>
        /// Gets the shared instance of <see cref="Utf8StringBuilderFactory"/> that will use managed arrays taken from the <see cref="ArrayPool{T}.Shared"/> for each builder.
        /// </summary>
        public static Utf8StringBuilderFactory SharedPooled => SharedPooledHolder.Instance;
        
        /// <summary>
        /// Creates a new <see cref="Utf8StringBuilder"/> with the specified initial capacity.
        /// </summary>
        /// <param name="initialCapacity">The initial buffer capacity of the string builder.</param>
        public Utf8StringBuilder CreateBuilder(int initialCapacity = DefaultCapacity)
        {
            ICompletableBufferWriter<byte> bufferWriter = _pool switch
            {
                null => new CompletableArrayBufferWriter<byte>(initialCapacity),
                ArrayPool<byte> arrayPool => new ArrayPoolBufferWriter<byte>(arrayPool, initialCapacity),
                MemoryPool<byte> memoryPool => new MemoryPoolBufferWriter<byte>(memoryPool, false, initialCapacity),
                _ => throw new InvalidOperationException("Unsupported pool type.")
            };
            return new Utf8StringBuilder(bufferWriter);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        /// If the factory was created using the default constructor or with a pool that is not disposable or must not be disposed together with this factory, you can skip calling this method.
        /// </remarks>
        public void Dispose()
        {
            if (_disposePool && _pool is IDisposable disposable)
                disposable.Dispose();
        }

        private static class SharedHolder
        {
            internal static readonly Utf8StringBuilderFactory Instance;

            static SharedHolder() => Instance = new Utf8StringBuilderFactory();
        }

        private static class SharedPooledHolder
        {
            internal static readonly Utf8StringBuilderFactory Instance;

            static SharedPooledHolder() => Instance = new Utf8StringBuilderFactory(ArrayPool<byte>.Shared);
        }
    }
}
