using System;
using System.Runtime.InteropServices;
using System.Threading;

#nullable enable

namespace XP.SDK
{
    internal abstract class DelegateWrapper<T> : IDisposable
        where T : Delegate
    {
        private GCHandle _handle;
        private int _disposed;

        protected DelegateWrapper(T @delegate)
        {
            Delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));
            _handle = GCHandle.Alloc(this);
        }
        protected T Delegate { get; }

        public GCHandle Handle => _handle;

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                _handle.Free();
            }
        }
    }
}
