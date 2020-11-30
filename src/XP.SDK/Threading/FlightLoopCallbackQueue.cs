#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using XP.SDK.XPLM;

namespace XP.SDK.Threading
{
    internal sealed class FlightLoopCallbackQueue : FlightLoop, ICallbackQueue
    {
        private readonly CountdownEvent _countdown = new CountdownEvent(1);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly BlockingCollection<(SendOrPostCallback callback, object? state)> _queue = new BlockingCollection<(SendOrPostCallback callback, object? state)>();

        public FlightLoopCallbackQueue(FlightLoopPhaseType phase = FlightLoopPhaseType.AfterFlightModel) : base(phase)
        {
        }

        protected override float OnFlightLoopCallback(float elapsedSinceLastCall, float elapsedTimeSinceLastFlightLoop, int counter)
        {
            while (_queue.TryTake(out var item))
            {
                var (callback, state) = item;
                callback.Invoke(state);
            }

            return -1;
        }

        public void Post(SendOrPostCallback d, object? state)
        {
            _queue.Add((d, state));
        }

        public void OperationStarted()
        {
            _countdown.AddCount();
        }

        public void OperationCompleted()
        {
            _countdown.Signal();
        }

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _cancellationTokenSource.Cancel();
            _queue.CompleteAdding();
            while (_queue.TryTake(out var item))
            {
                var (callback, state) = item;
                callback.Invoke(state);
            }

            _countdown.Signal();
            _countdown.Wait();
            _countdown.Dispose();
            _queue.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}
