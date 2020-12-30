using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// This is the prototype for a low level key-sniffing function.  Window-based
    /// UI _should not use this_!  The windowing system provides high-level
    /// mediated keyboard access, via the callbacks you attach to your
    /// XPLMCreateWindow_t. By comparison, the key sniffer provides low level
    /// keyboard access.
    /// </para>
    /// <para>
    /// Key sniffers are provided to allow libraries to provide non-windowed user
    /// interaction.  For example, the MUI library uses a key sniffer to do pop-up
    /// text entry.
    /// </para>
    /// <para>
    /// Return 1 to pass the key on to the next sniffer, the window manager,
    /// X-Plane, or whomever is down stream.  Return 0 to consume the key.
    /// </para>
    /// <para>
    /// Warning: this API declares virtual keys as a signed character; however the
    /// VKEY #define macros in XPLMDefs.h define the vkeys using unsigned values
    /// (that is 0x80 instead of -0x80).  So you may need to cast the incoming vkey
    /// to an unsigned char to get correct comparisons in C.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate int KeySnifferCallback(byte inChar, KeyFlags inFlags, byte inVirtualKey, void* inRefcon);
}