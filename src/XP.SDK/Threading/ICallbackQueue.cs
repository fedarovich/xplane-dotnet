#nullable enable
using System;
using System.Threading;

namespace XP.SDK.Threading
{
    public interface ICallbackQueue : IDisposable
    {
        void Post(SendOrPostCallback d, object? state);
        void OperationStarted();
        void OperationCompleted();
    }
}