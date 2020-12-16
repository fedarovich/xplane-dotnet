using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class CameraAPI
    {
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMControlCamera", ExactSpelling = true)]
        private static extern unsafe void ControlCameraPrivate(CameraControlDuration inHowLong, IntPtr inControlFunc, void* inRefcon);

        
        /// <summary>
        /// <para>
        /// This function repositions the camera on the next drawing cycle. You must
        /// pass a non-null control function. Specify in inHowLong how long you'd like
        /// control (indefinitely or until a new view mode is set by the user).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ControlCamera(CameraControlDuration inHowLong, CameraControlCallback inControlFunc, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inControlFuncPtr = inControlFunc != null ? Marshal.GetFunctionPointerForDelegate(inControlFunc) : default;
            ControlCameraPrivate(inHowLong, inControlFuncPtr, inRefcon);
            GC.KeepAlive(inControlFunc);
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDontControlCamera", ExactSpelling = true)]
        public static extern void DontControlCamera();

        
        /// <summary>
        /// <para>
        /// This routine returns 1 if the camera is being controlled, zero if it is
        /// not. If it is and you pass in a pointer to a camera control duration, the
        /// current control duration will be returned.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMIsCameraBeingControlled", ExactSpelling = true)]
        public static extern unsafe int IsCameraBeingControlled(CameraControlDuration* outCameraControlDuration);

        
        /// <summary>
        /// <para>
        /// This function reads the current camera position.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMReadCameraPosition", ExactSpelling = true)]
        public static extern unsafe void ReadCameraPosition(CameraPosition* outCameraPosition);
    }
}