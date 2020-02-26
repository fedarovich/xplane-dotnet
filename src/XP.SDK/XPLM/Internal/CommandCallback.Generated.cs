using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// A command callback is a function in your plugin that is called when a
    /// command is pressed. Your callback receives the command reference for the
    /// particular command, the phase of the command that is executing, and a
    /// reference pointer that you specify when registering the callback.
    /// </para>
    /// <para>
    /// Your command handler should return 1 to let processing of the command
    /// continue to other plugins and X-Plane, or 0 to halt processing, potentially
    /// bypassing X-Plane code.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int CommandCallback(CommandRef inCommand, CommandPhase inPhase, void* inRefcon);
}