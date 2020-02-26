using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// A menu handler function takes two reference pointers, one for the menu
    /// (specified when the menu was created) and one for the item (specified when
    /// the item was created).
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void MenuHandlerCallback(void* inMenuRef, void* inItemRef);
}