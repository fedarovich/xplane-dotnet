using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// A callback to notify your plugin that a new map has been created in
    /// X-Plane. This is the best time to add a custom map layer using
    /// XPLMCreateMapLayer().
    /// </para>
    /// <para>
    /// No OpenGL drawing is permitted within this callback.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MapCreatedCallback(byte* mapIdentifier, void* refcon);
}