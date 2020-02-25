using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RegisterFlightLoopCallback(FlightLoopCallback inFlightLoop, float inInterval, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inFlightLoopPtr = Marshal.GetFunctionPointerForDelegate(inFlightLoop);
            IL.Push(inFlightLoopPtr);
            IL.Push(inInterval);
            IL.Push(inRefcon);
            IL.Push(RegisterFlightLoopCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(float), typeof(void *)));
            GC.KeepAlive(inFlightLoop);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void UnregisterFlightLoopCallback(FlightLoopCallback inFlightLoop, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inFlightLoopPtr = Marshal.GetFunctionPointerForDelegate(inFlightLoop);
            IL.Push(inFlightLoopPtr);
            IL.Push(inRefcon);
            IL.Push(UnregisterFlightLoopCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(void *)));
            GC.KeepAlive(inFlightLoop);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetFlightLoopCallbackInterval(FlightLoopCallback inFlightLoop, float inInterval, int inRelativeToNow, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inFlightLoopPtr = Marshal.GetFunctionPointerForDelegate(inFlightLoop);
            IL.Push(inFlightLoopPtr);
            IL.Push(inInterval);
            IL.Push(inRelativeToNow);
            IL.Push(inRefcon);
            IL.Push(SetFlightLoopCallbackIntervalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(float), typeof(int), typeof(void *)));
            GC.KeepAlive(inFlightLoop);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe FlightLoopID CreateFlightLoop(CreateFlightLoop*inParams)
        {
            IL.DeclareLocals(false);
            FlightLoopID result;
            IL.Push(inParams);
            IL.Push(CreateFlightLoopPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(FlightLoopID), typeof(CreateFlightLoop*)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyFlightLoop(FlightLoopID inFlightLoopID)
        {
            IL.DeclareLocals(false);
            IL.Push(inFlightLoopID);
            IL.Push(DestroyFlightLoopPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(FlightLoopID)));
        }

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