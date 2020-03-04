using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

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
            ControlCameraPtr = Lib.GetExport("XPLMControlCamera");
            DontControlCameraPtr = Lib.GetExport("XPLMDontControlCamera");
            IsCameraBeingControlledPtr = Lib.GetExport("XPLMIsCameraBeingControlled");
            ReadCameraPositionPtr = Lib.GetExport("XPLMReadCameraPosition");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void ControlCameraPrivate(CameraControlDuration inHowLong, IntPtr inControlFunc, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ControlCameraPtr);
            IL.Push(inHowLong);
            IL.Push(inControlFunc);
            IL.Push(inRefcon);
            IL.Push(ControlCameraPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CameraControlDuration), typeof(CameraControlCallback), typeof(void*)));
        }

        
        /// <summary>
        /// <para>
        /// This function repositions the camera on the next drawing cycle. You must
        /// pass a non-null control function. Specify in inHowLong how long you'd like
        /// control (indefinitely or until a key is pressed).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ControlCamera(CameraControlDuration inHowLong, CameraControlCallback inControlFunc, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inControlFuncPtr = Marshal.GetFunctionPointerForDelegate(inControlFunc);
            ControlCameraPrivate(inHowLong, inControlFuncPtr, inRefcon);
            GC.KeepAlive(inControlFuncPtr);
        }

        
        /// <summary>
        /// <para>
        /// This function stops you from controlling the camera. If you have a camera
        /// control function, it will not be called with an inIsLosingControl flag.
        /// X-Plane will control the camera on the next cycle.
        /// </para>
        /// <para>
        /// For maximum compatibility you should not use this routine unless you are in
        /// posession of the camera.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DontControlCamera()
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DontControlCameraPtr);
            IL.Push(DontControlCameraPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void)));
        }

        
        /// <summary>
        /// <para>
        /// This routine returns 1 if the camera is being controlled, zero if it is
        /// not. If it is and you pass in a pointer to a camera control duration, the
        /// current control duration will be returned.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IsCameraBeingControlled(CameraControlDuration* outCameraControlDuration)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(IsCameraBeingControlledPtr);
            int result;
            IL.Push(outCameraControlDuration);
            IL.Push(IsCameraBeingControlledPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(CameraControlDuration*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This function reads the current camera position.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ReadCameraPosition(CameraPosition* outCameraPosition)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ReadCameraPositionPtr);
            IL.Push(outCameraPosition);
            IL.Push(ReadCameraPositionPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(CameraPosition*)));
        }
    }
}