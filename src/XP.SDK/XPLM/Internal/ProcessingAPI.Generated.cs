using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class ProcessingAPI
    {
        private static IntPtr GetElapsedTimePtr;
        private static IntPtr GetCycleNumberPtr;
        private static IntPtr RegisterFlightLoopCallbackPtr;
        private static IntPtr UnregisterFlightLoopCallbackPtr;
        private static IntPtr SetFlightLoopCallbackIntervalPtr;
        private static IntPtr CreateFlightLoopPtr;
        private static IntPtr DestroyFlightLoopPtr;
        private static IntPtr ScheduleFlightLoopPtr;

        static ProcessingAPI()
        {
            GetElapsedTimePtr = Lib.GetExport("XPLMGetElapsedTime");
            GetCycleNumberPtr = Lib.GetExport("XPLMGetCycleNumber");
            RegisterFlightLoopCallbackPtr = Lib.GetExport("XPLMRegisterFlightLoopCallback");
            UnregisterFlightLoopCallbackPtr = Lib.GetExport("XPLMUnregisterFlightLoopCallback");
            SetFlightLoopCallbackIntervalPtr = Lib.GetExport("XPLMSetFlightLoopCallbackInterval");
            CreateFlightLoopPtr = Lib.GetExport("XPLMCreateFlightLoop");
            DestroyFlightLoopPtr = Lib.GetExport("XPLMDestroyFlightLoop");
            ScheduleFlightLoopPtr = Lib.GetExport("XPLMScheduleFlightLoop");
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float GetElapsedTime()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetElapsedTimePtr);
            float result;
            IL.Push(GetElapsedTimePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine returns a counter starting at zero for each sim cycle
        /// computed/video frame rendered.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetCycleNumber()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetCycleNumberPtr);
            int result;
            IL.Push(GetCycleNumberPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void RegisterFlightLoopCallbackPrivate(IntPtr inFlightLoop, float inInterval, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(RegisterFlightLoopCallbackPtr);
            IL.Push(inFlightLoop);
            IL.Push(inInterval);
            IL.Push(inRefcon);
            IL.Push(RegisterFlightLoopCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(FlightLoopCallback), typeof(float), typeof(void*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RegisterFlightLoopCallback(FlightLoopCallback inFlightLoop, float inInterval, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inFlightLoopPtr = inFlightLoop != null ? Marshal.GetFunctionPointerForDelegate(inFlightLoop) : default;
            RegisterFlightLoopCallbackPrivate(inFlightLoopPtr, inInterval, inRefcon);
            GC.KeepAlive(inFlightLoop);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void UnregisterFlightLoopCallbackPrivate(IntPtr inFlightLoop, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(UnregisterFlightLoopCallbackPtr);
            IL.Push(inFlightLoop);
            IL.Push(inRefcon);
            IL.Push(UnregisterFlightLoopCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(FlightLoopCallback), typeof(void*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void UnregisterFlightLoopCallback(FlightLoopCallback inFlightLoop, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inFlightLoopPtr = inFlightLoop != null ? Marshal.GetFunctionPointerForDelegate(inFlightLoop) : default;
            UnregisterFlightLoopCallbackPrivate(inFlightLoopPtr, inRefcon);
            GC.KeepAlive(inFlightLoop);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void SetFlightLoopCallbackIntervalPrivate(IntPtr inFlightLoop, float inInterval, int inRelativeToNow, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(SetFlightLoopCallbackIntervalPtr);
            IL.Push(inFlightLoop);
            IL.Push(inInterval);
            IL.Push(inRelativeToNow);
            IL.Push(inRefcon);
            IL.Push(SetFlightLoopCallbackIntervalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(FlightLoopCallback), typeof(float), typeof(int), typeof(void*)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetFlightLoopCallbackInterval(FlightLoopCallback inFlightLoop, float inInterval, int inRelativeToNow, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inFlightLoopPtr = inFlightLoop != null ? Marshal.GetFunctionPointerForDelegate(inFlightLoop) : default;
            SetFlightLoopCallbackIntervalPrivate(inFlightLoopPtr, inInterval, inRelativeToNow, inRefcon);
            GC.KeepAlive(inFlightLoop);
        }

        
        /// <summary>
        /// <para>
        /// This routine creates a flight loop callback and returns its ID. The flight
        /// loop callback is created using the input param struct, and is inited to be
        /// unscheduled.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe FlightLoopID CreateFlightLoop(CreateFlightLoop* inParams)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CreateFlightLoopPtr);
            FlightLoopID result;
            IL.Push(inParams);
            IL.Push(CreateFlightLoopPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(FlightLoopID), typeof(CreateFlightLoop*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine destroys a flight loop callback by ID. Only call it on flight
        /// loops created with the newer XPLMCreateFlightLoop API.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyFlightLoop(FlightLoopID inFlightLoopID)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DestroyFlightLoopPtr);
            IL.Push(inFlightLoopID);
            IL.Push(DestroyFlightLoopPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(FlightLoopID)));
        }

        
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
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ScheduleFlightLoop(FlightLoopID inFlightLoopID, float inInterval, int inRelativeToNow)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ScheduleFlightLoopPtr);
            IL.Push(inFlightLoopID);
            IL.Push(inInterval);
            IL.Push(inRelativeToNow);
            IL.Push(ScheduleFlightLoopPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(FlightLoopID), typeof(float), typeof(int)));
        }
    }
}