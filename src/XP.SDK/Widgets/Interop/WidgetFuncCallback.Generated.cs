using System;
using System.Runtime.InteropServices;

namespace XP.SDK.Widgets.Interop
{
    
    /// <summary>
    /// <para>
    /// This function defines your custom widget's behavior. It will be called by
    /// the widgets library to send messages to your widget. The message and widget
    /// ID are passed in, as well as two ptr-width signed parameters whose meaning
    /// varies with the message. Return 1 to indicate that you have processed the
    /// message, 0 to indicate that you have not. For any message that is not
    /// understood, return 0.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate int WidgetFuncCallback(WidgetMessage inMessage, WidgetID inWidget, nint inParam1, nint inParam2);
}