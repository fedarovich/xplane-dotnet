using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Interop;

#nullable enable

namespace XP.SDK.XPLM
{
    /// <summary>
    /// <para>
    /// This class provides an abstraction over X-Plane's flight loop.
    /// </para>
    /// <para>
    /// It allows you to get regular callbacks during the flight loop, the part of X-Plane where the plane’s position calculates the physics of flight, etc.
    /// Use these APIs to accomplish periodic tasks like logging data and performing I/O.
    /// </para>
    /// <para>
    /// You can receive a callback either just before or just after the per-frame physics calculations happen -
    /// you can use post-FM callbacks to &quot;patch&quot; the flight model after it has run.
    /// </para>
    /// <para>
    /// If the user has set the number of flight model iterations per frame greater than one your plugin will not see this;
    /// these integrations run on the sub-section of the flight model where iterations improve responsiveness
    /// (e.g. physical integration, not simple systems tracking) and are thus opaque to plugins.
    /// </para>
    /// <para>
    /// Flight loop scheduling, when scheduled by time, is scheduled by a “first callback after the deadline” schedule,
    /// e.g. your callbacks will always be slightly late to ensure that we don’t run faster than your deadline.
    /// </para>
    /// <para>
    /// WARNING: Do NOT use these callbacks to draw! You cannot draw during flight loop callbacks.
    /// Use the drawing callbacks for graphics.
    /// (One exception: you can use a post-flight loop callback to update your own off-screen FBOs.)
    /// </para>
    /// </summary>
    public abstract class FlightLoop : IDisposable
    {
        private volatile int _disposed;
        private FlightLoopID _id;
        private GCHandle _handle;

        /// <summary>
        /// Initializes a new instance of <see cref="FlightLoop"/>.
        /// </summary>
        /// <param name="phase">The flight loop phase.</param>
        protected unsafe FlightLoop(FlightLoopPhaseType phase)
        {
            _handle = GCHandle.Alloc(this);
            var parameters = new CreateFlightLoop
            {
                structSize = sizeof(CreateFlightLoop),
                phase = phase,
                callbackFunc = &FlightLoopCallback,
                refcon = GCHandle.ToIntPtr(_handle).ToPointer()
            };
            _id = ProcessingAPI.CreateFlightLoop(&parameters);

            [UnmanagedCallersOnly]
            static float FlightLoopCallback(float inelapsedsincelastcall, float inelapsedtimesincelastflightloop, int incounter, void* inrefcon) =>
                Utils.TryGetObject<FlightLoop>(inrefcon)?.OnFlightLoopCallback(
                    inelapsedsincelastcall, inelapsedtimesincelastflightloop, incounter) ?? 0;
        }

        /// <summary>
        /// Creates a new instance of <see cref="FlightLoop"/> that will invoke the specified <paramref name="callback"/>.
        /// </summary>
        /// <param name="phase">The flight loop phase.</param>
        /// <param name="callback">The callback.</param>
        public static FlightLoop Create(FlightLoopPhaseType phase, Callback callback)
        {
            if (callback == null) 
                throw new ArgumentNullException(nameof(callback));

            return new CallbackFlightLoop(phase, callback);
        }

        /// <summary>
        /// Creates a new instance of <see cref="FlightLoop"/> that will invoke the specified <paramref name="callback"/>.
        /// </summary>
        /// <param name="phase">The flight loop phase.</param>
        /// <param name="callback">The callback.</param>
        public static unsafe FlightLoop Create(FlightLoopPhaseType phase, delegate* unmanaged<float, float, int, float> callback)
        {
            if (callback == null) 
                throw new ArgumentNullException(nameof(callback));

            return new UnmanagedCallbackFlightLoop(phase, callback);
        }

        /// <summary>
        /// Returns a counter starting at zero for each sim cycle computed/video frame rendered.
        /// </summary>
        public static int CycleNumber => ProcessingAPI.GetCycleNumber();

        /// <summary>
        /// <para>
        /// This routine returns the elapsed time since the sim started up in decimal
        /// seconds. This is a wall timer; it keeps counting upward even if the sim is
        /// paused.
        /// </para>
        /// <para>
        /// WARNING: It is not a very good timer!  It lacks
        /// precision in both its data type and its source.  Do not attempt to use it
        /// for timing critical applications like network multiplayer.
        /// </para>
        /// </summary>
        public static float ElapsedTime => ProcessingAPI.GetElapsedTime();

        /// <summary>
        /// Gets the ID of the flight loop.
        /// </summary>
        public FlightLoopID Id => _id;

        /// <summary>
        /// <para>
        /// This routine schedules a flight loop callback for future execution. If
        /// inInterval is negative, it is run in a certain number of frames based on
        /// the absolute value of the input. If the interval is positive, it is a
        /// duration in seconds.
        /// </para>
        /// <para>
        /// If inRelativeToNow is true, ties are interpretted relative to the time this
        /// routine is called; otherwise they are relative to the last call time or the
        /// time the flight loop was registered (if never called).
        /// </para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Schedule(float interval, bool relativeToNow)
        {
            if (_disposed == 0)
            {
                ProcessingAPI.ScheduleFlightLoop(_id, interval, relativeToNow.ToInt());
            }
        }

        /// <summary>
        /// This method is called each time flight loop callback occurs.
        /// </summary>
        /// <param name="elapsedSinceLastCall">The wall time since your last callback</param>
        /// <param name="elapsedTimeSinceLastFlightLoop">The wall time since any flight loop was dispatched.</param>
        /// <param name="counter">A monotonically increasing counter, bumped once per flight loop dispatch from the sim.</param>
        /// <returns>
        /// <para>
        /// 0 to stop receiving callbacks.
        /// </para>
        /// <para>
        /// A positive number to specify how many seconds until the next callback. (You will be called at or after this time, not before.)
        /// </para>
        /// <para>
        /// A negative number to specify how many loops must go by until you are called. For example, -1.0 means call me the very next loop.
        /// </para>
        /// </returns>
        /// <remarks>
        /// <para>
        /// Each time the flight loop is iterated through, you receive this call at the end.
        /// </para>
        /// <para>
        /// Try to run your flight loop as infrequently as is practical, and suspend it (using return value 0) when you do not need it;
        /// lots of flight loop callbacks that do nothing lowers X-Plane’s frame rate.
        /// </para>
        /// <para>
        /// Your callback will NOT be unregistered if you return 0; it will merely be inactive.
        /// </para>
        /// </remarks>
        protected abstract float OnFlightLoopCallback(float elapsedSinceLastCall, float elapsedTimeSinceLastFlightLoop, int counter);

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="FlightLoop"/>, and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <inheritdoc />
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

        /// <summary>
        /// This callback is called each time flight loop callback occurs.
        /// </summary>
        /// <param name="elapsedSinceLastCall">The wall time since your last callback</param>
        /// <param name="elapsedTimeSinceLastFlightLoop">The wall time since any flight loop was dispatched.</param>
        /// <param name="counter">A monotonically increasing counter, bumped once per flight loop dispatch from the sim.</param>
        /// <returns>
        /// <para>
        /// 0 to stop receiving callbacks.
        /// </para>
        /// <para>
        /// A positive number to specify how many seconds until the next callback. (You will be called at or after this time, not before.)
        /// </para>
        /// <para>
        /// A negative number to specify how many loops must go by until you are called. For example, -1.0 means call me the very next loop.
        /// </para>
        /// </returns>
        /// <remarks>
        /// <para>
        /// Each time the flight loop is iterated through, you receive this call at the end.
        /// </para>
        /// <para>
        /// Try to run your flight loop as infrequently as is practical, and suspend it (using return value 0) when you do not need it;
        /// lots of flight loop callbacks that do nothing lowers X-Plane’s frame rate.
        /// </para>
        /// <para>
        /// Your callback will NOT be unregistered if you return 0; it will merely be inactive.
        /// </para>
        /// </remarks>
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

        private unsafe sealed class UnmanagedCallbackFlightLoop : FlightLoop
        {
            private readonly delegate* unmanaged<float, float, int, float> _callback;

            public UnmanagedCallbackFlightLoop(FlightLoopPhaseType phase, delegate* unmanaged<float, float, int, float> callback) : base(phase)
            {
                _callback = callback;
            }

            protected override float OnFlightLoopCallback(float elapsedSinceLastCall, float elapsedTimeSinceLastFlightLoop, int counter)
            {
                return _callback(elapsedSinceLastCall, elapsedTimeSinceLastFlightLoop, counter);
            }
        }
    }
}
