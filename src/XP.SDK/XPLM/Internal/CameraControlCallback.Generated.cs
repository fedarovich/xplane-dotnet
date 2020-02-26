using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// You use an XPLMCameraControl function to provide continuous control over
    /// the camera. You are passed in a structure in which to put the new camera
    /// position; modify it and return 1 to reposition the camera. Return 0 to
    /// surrender control of the camera; camera control will be handled by X-Plane
    /// on this draw loop. The contents of the structure as you are called are
    /// undefined.
    /// </para>
    /// <para>
    /// If X-Plane is taking camera control away from you, this function will be
    /// called with inIsLosingControl set to 1 and ioCameraPosition NULL.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int CameraControlCallback(CameraPosition* outCameraPosition, int inIsLosingControl, void* inRefcon);
}