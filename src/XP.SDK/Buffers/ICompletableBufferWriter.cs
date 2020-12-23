using System.Buffers;

namespace XP.SDK.Buffers
{
    /// <summary>
    /// Buffer writer which can be completed.
    /// </summary>
    public interface ICompletableBufferWriter<T> : IBufferWriter<T>, IBufferAccessor<T>
    {
        /// <summary>
        /// Checks whether the writer is completed.
        /// </summary>
        /// <seealso cref="Complete"/>
        bool IsCompleted { get; }

        /// <summary>
        /// Completes the writer, so that no more write operations can be done.
        /// </summary>
        /// <seealso cref="IsCompleted"/>
        void Complete();
    }
}