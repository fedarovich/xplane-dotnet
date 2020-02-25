using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Camera
    {
        private static IntPtr ControlCameraPtr;
        private static IntPtr DontControlCameraPtr;
        private static IntPtr IsCameraBeingControlledPtr;
        private static IntPtr ReadCameraPositionPtr;
        static Camera()
        {
            const string libraryName = "XPLM";
            ControlCameraPtr = FunctionResolver.Resolve(libraryName, "XPLMControlCamera");
            DontControlCameraPtr = FunctionResolver.Resolve(libraryName, "XPLMDontControlCamera");
            IsCameraBeingControlledPtr = FunctionResolver.Resolve(libraryName, "XPLMIsCameraBeingControlled");
            ReadCameraPositionPtr = FunctionResolver.Resolve(libraryName, "XPLMReadCameraPosition");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ControlCamera(CameraControlDuration inHowLong, CameraControlCallback inControlFunc, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inControlFuncPtr = Marshal.GetFunctionPointerForDelegate(inControlFunc);
            IL.Push(inHowLong);
            IL.Push(inControlFuncPtr);
            IL.Push(inRefcon);
            IL.Push(ControlCameraPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CameraControlDuration), typeof(IntPtr), typeof(void *)));
            GC.KeepAlive(inControlFunc);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DontControlCamera()
        {
            IL.DeclareLocals(false);
            IL.Push(DontControlCameraPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IsCameraBeingControlled(CameraControlDuration*outCameraControlDuration)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(outCameraControlDuration);
            IL.Push(IsCameraBeingControlledPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(CameraControlDuration*)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ReadCameraPosition(CameraPosition*outCameraPosition)
        {
            IL.DeclareLocals(false);
            IL.Push(outCameraPosition);
            IL.Push(ReadCameraPositionPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CameraPosition*)));
        }
    }
}