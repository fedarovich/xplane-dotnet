using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// This function is called when a key is pressed or keyboard focus is taken
    /// away from your window.  If losingFocus is 1, you are losing the keyboard
    /// focus, otherwise a key was pressed and inKey contains its character.  You
    /// are also passed your window and a refcon.
    /// </para>
    /// <para>
    /// Warning: this API declares virtual keys as a signed character; however the
    /// VKEY #define macros in XPLMDefs.h define the vkeys using unsigned values
    /// (that is 0x80 instead of -0x80).  So you may need to cast the incoming vkey
    /// to an unsigned char to get correct comparisons in C.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void HandleKeyCallback(WindowID inWindowID, byte inKey, KeyFlags inFlags, byte inVirtualKey, void* inRefcon, int losingFocus);
}