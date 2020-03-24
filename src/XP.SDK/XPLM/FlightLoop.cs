using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using XP.SDK.XPLM.Internal;

#nullable enable

namespace XP.SDK.XPLM
{
    public abstract class FlightLoop : IDisposable
    {
        private static readonly FlightLoopCallback _flightLoopCallback;

        private volatile int _disposed;
        private FlightLoopID _id;
        private GCHandle _handle;

        static unsafe FlightLoop()
        {
            _flightLoopCallback = FlightLoopCallback;

            static float FlightLoopCallback(float inelapsedsincelastcall, float inelapsedtimesincelastflightloop, int incounter, void* inrefcon) =>
                Utils.TryGetObject<FlightLoop>(inrefcon)?.OnFlightLoopCallback(
                    inelapsedsincelastcall, inelapsedtimesincelastflightloop, incounter) ?? 0;
        }

        protected unsafe FlightLoop(FlightLoopPhaseType phase)
        {
            _handle = GCHandle.Alloc(this);
            var parameters = new CreateFlightLoop
            {
                structSize = sizeof(CreateFlightLoop),
                phase = phase,
                callbackFunc = Marshal.GetFunctionPointerForDelegate(_flightLoopCallback),
                refcon = GCHandle.ToIntPtr(_handle).ToPointer()
            };
            _id = ProcessingAPI.CreateFlightLoop(&parameters);
        }

        public static FlightLoop Create(FlightLoopPhaseType phase, Callback callback)
        {
            if (callback == null) 
                throw new ArgumentNullException(nameof(callback));

            return new CallbackFlightLoop(phase, callback);
        }

        public static int CycleNumber => ProcessingAPI.GetCycleNumber();

        public static float ElapsedTime => ProcessingAPI.GetElapsedTime();

        public FlightLoopID Id => _id;

        public void Schedule(float interval, bool relativeToNow)
        {
            if (_disposed == 0)
            {
                ProcessingAPI.ScheduleFlightLoop(_id, interval, relativeToNow.ToInt());
            }
        }

        protected abstract float OnFlightLoopCallback(float elapsedSinceLastCall, float elapsedTimeSinceLastFlightLoop, int counter);

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                try
                {
                    Dispose(true);
                }
                finally
                {
                    ProcessingAPI.DestroyFlightLoop(_id);
                    _handle.Free();
                }
            }
        }

        public delegate float Callback(float elapsedSinceLastCall, float elapsedTimeSinceLastFlightLoop, int counter);

        private sealed class CallbackFlightLoop : FlightLoop
        {
            private readonly Callback _callback;

            public CallbackFlightLoop(FlightLoopPhaseType phase, Callback callback) : base(phase)
            {
                _callback = callback;
            }

            protected override float OnFlightLoopCallback(float elapsedSinceLastCall, float elapsedTimeSinceLastFlightLoop, int counter)
            {
                return _callback.Invoke(elapsedSinceLastCall, elapsedTimeSinceLastFlightLoop, counter);
            }
        }
    }
}
