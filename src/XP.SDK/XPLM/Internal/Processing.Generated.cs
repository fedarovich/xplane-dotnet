using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Processing
    {
        private static IntPtr GetElapsedTimePtr;
        private static IntPtr GetCycleNumberPtr;
        private static IntPtr RegisterFlightLoopCallbackPtr;
        private static IntPtr UnregisterFlightLoopCallbackPtr;
        private static IntPtr SetFlightLoopCallbackIntervalPtr;
        private static IntPtr CreateFlightLoopPtr;
        private static IntPtr DestroyFlightLoopPtr;
        private static IntPtr ScheduleFlightLoopPtr;

        static Processing()
        {
            const string libraryName = "XPLM";
            GetElapsedTimePtr = FunctionResolver.Resolve(libraryName, "XPLMGetElapsedTime");
            GetCycleNumberPtr = FunctionResolver.Resolve(libraryName, "XPLMGetCycleNumber");
            RegisterFlightLoopCallbackPtr = FunctionResolver.Resolve(libraryName, "XPLMRegisterFlightLoopCallback");
            UnregisterFlightLoopCallbackPtr = FunctionResolver.Resolve(libraryName, "XPLMUnregisterFlightLoopCallback");
            SetFlightLoopCallbackIntervalPtr = FunctionResolver.Resolve(libraryName, "XPLMSetFlightLoopCallbackInterval");
            CreateFlightLoopPtr = FunctionResolver.Resolve(libraryName, "XPLMCreateFlightLoop");
            DestroyFlightLoopPtr = FunctionResolver.Resolve(libraryName, "XPLMDestroyFlightLoop");
            ScheduleFlightLoopPtr = FunctionResolver.Resolve(libraryName, "XPLMScheduleFlightLoop");
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the elapsed time since the sim started up in decimal
        /// seconds.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float GetElapsedTime()
        {
            IL.DeclareLocals(false);
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
            int result;
            IL.Push(GetCycleNumberPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
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
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RegisterFlightLoopCallback(FlightLoopCallback inFlightLoop, float inInterval, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inFlightLoopPtr = Marshal.GetFunctionPointerForDelegate(inFlightLoop);
            IL.Push(inFlightLoopPtr);
            IL.Push(inInterval);
            IL.Push(inRefcon);
            IL.Push(RegisterFlightLoopCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(float), typeof(void*)));
            GC.KeepAlive(inFlightLoop);
        }

        
        /// <summary>
        /// <para>
        /// This routine unregisters your flight loop callback. Do NOT call it from
        /// your flight loop callback. Once your flight loop callback is unregistered,
        /// it will not be called again.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void UnregisterFlightLoopCallback(FlightLoopCallback inFlightLoop, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inFlightLoopPtr = Marshal.GetFunctionPointerForDelegate(inFlightLoop);
            IL.Push(inFlightLoopPtr);
            IL.Push(inRefcon);
            IL.Push(UnregisterFlightLoopCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(void*)));
            GC.KeepAlive(inFlightLoop);
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
            IntPtr inFlightLoopPtr = Marshal.GetFunctionPointerForDelegate(inFlightLoop);
            IL.Push(inFlightLoopPtr);
            IL.Push(inInterval);
            IL.Push(inRelativeToNow);
            IL.Push(inRefcon);
            IL.Push(SetFlightLoopCallbackIntervalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(float), typeof(int), typeof(void*)));
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
            FlightLoopID result;
            IL.Push(inParams);
            IL.Push(CreateFlightLoopPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(FlightLoopID), typeof(CreateFlightLoop*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine destroys a flight loop callback by ID.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyFlightLoop(FlightLoopID inFlightLoopID)
        {
            IL.DeclareLocals(false);
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
        /// <para>
        /// THREAD SAFETY: it is legal to call this routine from any thread under the
        /// following conditions:
        /// </para>
        /// <para>
        /// 1. The call must be between the beginning of an XPLMEnable and the end of
        /// an XPLMDisable sequence. (That is, you must not call this routine from
        /// thread activity when your plugin was supposed to be disabled. Since plugins
        /// are only enabled while loaded, this also implies you cannot run this
        /// routine outside an XPLMStart/XPLMStop sequence.)
        /// </para>
        /// <para>
        /// 2. You may not call this routine re-entrantly for a single flight loop ID.
        /// (That is, you can't enable from multiple threads at the same time.)
        /// </para>
        /// <para>
        /// 3. You must call this routine between the time after XPLMCreateFlightLoop
        /// returns a value and the time you call XPLMDestroyFlightLoop. (That is, you
        /// must ensure that your threaded activity is within the life of the object.
        /// The SDK does not check this for you, nor does it synchronize destruction of
        /// the object.)
        /// </para>
        /// <para>
        /// 4. The object must be unscheduled if this routine is to be called from a
        /// thread other than the main thread.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ScheduleFlightLoop(FlightLoopID inFlightLoopID, float inInterval, int inRelativeToNow)
        {
            IL.DeclareLocals(false);
            IL.Push(inFlightLoopID);
            IL.Push(inInterval);
            IL.Push(inRelativeToNow);
            IL.Push(ScheduleFlightLoopPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(FlightLoopID), typeof(float), typeof(int)));
        }
    }
}