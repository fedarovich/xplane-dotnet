#nullable enable
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using XP.SDK.XPLM;

namespace XP.SDK.Threading
{
    public class FlightLoopSynchronizationContext : SynchronizationContext, IDisposable
    {
        private readonly State _state;
        private readonly FlightLoop? _flightLoop;

        private int _disposed;

        public FlightLoopSynchronizationContext(
            float interval = -1f, 
            int maxWorkItemsPerFrame = int.MaxValue, 
            long? maxTimeUsPerFrame = null,
            FlightLoopPhaseType phase = FlightLoopPhaseType.AfterFlightModel)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (interval == 0)
                throw new ArgumentException("The interval cannot be 0.", nameof(interval));
            if (maxWorkItemsPerFrame <= 0)
                throw new ArgumentException("The value cannot be less than 1.", nameof(maxWorkItemsPerFrame));
            if (maxTimeUsPerFrame <= 0)
                throw new ArgumentException("The value cannot be less than 1.", nameof(maxTimeUsPerFrame));


            _state = new State(Thread.CurrentThread.ManagedThreadId);
            if (maxTimeUsPerFrame != null)
            {
                long maxTicks = checked(maxTimeUsPerFrame.Value * Stopwatch.Frequency / 1_000_000);
                _flightLoop = FlightLoop.Create(phase, (a, b, c) =>
                {
                    TimeConstrainedLoop(maxWorkItemsPerFrame, maxTicks);
                    return interval;
                });
            }
            else
            {
                _flightLoop = FlightLoop.Create(phase, (a, b, c) =>
                {
                    ItemsConstrainedLoop(maxWorkItemsPerFrame);
                    return interval;
                });
            }

            _flightLoop.Schedule(interval, false);
        }

        // For unit tests only
        internal FlightLoopSynchronizationContext(
            int maxWorkItemsPerFrame,
            long maxTimeUsPerFrame,
            out Action invocation)
        {
            _state = new State(Thread.CurrentThread.ManagedThreadId);
            long maxTicks = checked(maxTimeUsPerFrame * Stopwatch.Frequency / 1_000_000);
            invocation = () => TimeConstrainedLoop(maxWorkItemsPerFrame, maxTicks);
        }

        // For unit tests only
        internal FlightLoopSynchronizationContext(
            int maxWorkItemsPerFrame,
            out Action invocation)
        {
            _state = new State(Thread.CurrentThread.ManagedThreadId);
            invocation = () => ItemsConstrainedLoop(maxWorkItemsPerFrame);
        }

        private FlightLoopSynchronizationContext(in State state)
        {
            _state = state;
        }

        public override SynchronizationContext CreateCopy()
        {
            return new FlightLoopSynchronizationContext(_state);
        }

        public override void OperationCompleted() => _state.Countdown.Signal();

        public override void OperationStarted() => _state.Countdown.AddCount();

        public override void Post(SendOrPostCallback callback, object? state) => _state.Queue.Add(new WorkItem(callback, state));

        public override void Send(SendOrPostCallback callback, object? state)
        {
            if (_state.ThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                callback(state);
            }
            else
            {
                using var @event = new ManualResetEventSlim(false);
                _state.Queue.Add(new WorkItem(callback, state, @event));
                @event.Wait();
            }
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0 && _flightLoop != null)
            {
                _flightLoop.Dispose();
                Shutdown();
            }

            UnhandledException = null;
        }

        internal void Shutdown()
        {
            _state.Queue.CompleteAdding();

            while (_state.Queue.TryTake(out var workItem))
            {
                try
                {
                    workItem.Execute();
                }
                catch (Exception ex)
                {
                    UnhandledException?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
                }
            }

            _state.Countdown.Signal();
            _state.Countdown.Wait();
            _state.Countdown.Dispose();
            _state.Queue.Dispose();
        }

        public event UnhandledExceptionEventHandler? UnhandledException;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private void TimeConstrainedLoop(int maxWorkItemsPerFrame, long maxTicks)
        {
            var stopwatch = Stopwatch.StartNew();
            var start = stopwatch.ElapsedTicks;
            int counter = 0;
            while (counter++ < maxWorkItemsPerFrame && _state.Queue.TryTake(out var item))
            {
                try
                {
                    item.Execute();
                }
                catch (Exception ex)
                {
                    UnhandledException?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
                }

                if ((stopwatch.ElapsedTicks - start) >= maxTicks)
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private void ItemsConstrainedLoop(int maxWorkItemsPerFrame)
        {
            int counter = 0;
            while (counter++ < maxWorkItemsPerFrame && _state.Queue.TryTake(out var item))
            {
                try
                {
                    item.Execute();
                }
                catch (Exception ex)
                {
                    UnhandledException?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
                }
            }
        }

        private readonly struct State
        {
            public State(int threadId)
            {
                ThreadId = threadId;
                Countdown = new CountdownEvent(1);
                Queue = new BlockingCollection<WorkItem>();
            }

            public CountdownEvent Countdown { get; }
            public BlockingCollection<WorkItem> Queue { get; }
            public int ThreadId { get; }
        }

        private readonly struct WorkItem
        {
            private readonly SendOrPostCallback _callback;
            private readonly object? _state;
            private readonly ManualResetEventSlim? _event;

            public WorkItem(SendOrPostCallback callback, object? state, ManualResetEventSlim @event = null)
            {
                _callback = callback ?? throw new ArgumentNullException(nameof(callback));
                _state = state;
                _event = @event;
            }

            public void Execute()
            {
                try
                {
                    _callback(_state);
                }
                finally
                {
                    _event?.Set();
                }
            }
        }
    }
}
