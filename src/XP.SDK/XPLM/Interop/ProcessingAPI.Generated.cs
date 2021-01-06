using System;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Interop
{
    public static partial class ProcessingAPI
    {
        
        /// <summary>
        /// <para>
        /// This routine returns the elapsed time since the sim started up in decimal
        /// seconds. This is a wall timer; it keeps counting upward even if the sim is
        /// pasued.
        /// </para>
        /// <para>
        /// __WARNING__: XPLMGetElapsedTime is not a very good timer!  It lacks
        /// precision in both its data type and its source.  Do not attempt to use it
        /// for timing critical applications like network multiplayer.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetElapsedTime", ExactSpelling = true)]
        public static extern float GetElapsedTime();

        
        /// <summary>
        /// <para>
        /// This routine returns a counter starting at zero for each sim cycle
        /// computed/video frame rendered.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetCycleNumber", ExactSpelling = true)]
        public static extern int GetCycleNumber();

        
        /// <summary>
        /// <para>
        /// This routine registers your flight loop callback. Pass in a pointer to a
        /// flight loop function and a refcon. inInterval defines when you will be
        /// called. Pass in a positive number to specify seconds from registration time
        /// to the next callback. Pass in a negative number to indicate when you will
        /// be called (e.g. pass -1 to be called at the next cylcle). Pass 0 to not be
        /// called; your callback will be inactive.
        /// </para>
        /// <para>
        /// (This legacy function only installs pre-flight-loop callbacks; use
        /// XPLMCreateFlightLoop for more control.)
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMRegisterFlightLoopCallback", ExactSpelling = true)]
        public static extern unsafe void RegisterFlightLoopCallback(delegate* unmanaged<float, float, int, void*, float> inFlightLoop, float inInterval, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine unregisters your flight loop callback. Do NOT call it from
        /// your flight loop callback. Once your flight loop callback is unregistered,
        /// it will not be called again.
        /// </para>
        /// <para>
        /// Only use this on flight loops registered via
        /// XPLMRegisterFlightLoopCallback.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMUnregisterFlightLoopCallback", ExactSpelling = true)]
        public static extern unsafe void UnregisterFlightLoopCallback(delegate* unmanaged<float, float, int, void*, float> inFlightLoop, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine sets when a callback will be called. Do NOT call it from your
        /// callback; use the return value of the callback to change your callback
        /// interval from inside your callback.
        /// </para>
        /// <para>
        /// inInterval is formatted the same way as in XPLMRegisterFlightLoopCallback;
        /// positive for seconds, negative for cycles, and 0 for deactivating the
        /// callback. If inRelativeToNow is 1, times are from the time of this call;
        /// otherwise they are from the time the callback was last called (or the time
        /// it was registered if it has never been called.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSetFlightLoopCallbackInterval", ExactSpelling = true)]
        public static extern unsafe void SetFlightLoopCallbackInterval(delegate* unmanaged<float, float, int, void*, float> inFlightLoop, float inInterval, int inRelativeToNow, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This routine creates a flight loop callback and returns its ID. The flight
        /// loop callback is created using the input param struct, and is inited to be
        /// unscheduled.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCreateFlightLoop", ExactSpelling = true)]
        public static extern unsafe FlightLoopID CreateFlightLoop(CreateFlightLoop* inParams);

        
        /// <summary>
        /// <para>
        /// This routine destroys a flight loop callback by ID. Only call it on flight
        /// loops created with the newer XPLMCreateFlightLoop API.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDestroyFlightLoop", ExactSpelling = true)]
        public static extern void DestroyFlightLoop(FlightLoopID inFlightLoopID);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMScheduleFlightLoop", ExactSpelling = true)]
        public static extern void ScheduleFlightLoop(FlightLoopID inFlightLoopID, float inInterval, int inRelativeToNow);
    }
}