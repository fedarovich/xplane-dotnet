using System;
using System.Runtime.InteropServices;

namespace XP.SDK.Widgets.Internal
{
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public delegate int WidgetFuncCallback(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2);
}