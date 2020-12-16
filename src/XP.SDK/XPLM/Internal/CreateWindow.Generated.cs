using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// The XPMCreateWindow_t structure defines all of the parameters used to
    /// create a modern window using XPLMCreateWindowEx().  The structure will be
    /// expanded in future SDK APIs to include more features.  Always set the
    /// structSize member to the size of your struct in bytes!
    /// </para>
    /// <para>
    /// All windows created by this function in the XPLM300 version of the API are
    /// created with the new X-Plane 11 GUI features. This means your plugin will
    /// get to "know" about the existence of X-Plane windows other than the main
    /// window. All drawing and mouse callbacks for your window will occur in
    /// "boxels," giving your windows automatic support for high-DPI scaling in
    /// X-Plane. In addition, your windows can opt-in to decoration with the
    /// X-Plane 11 window styling, and you can use the
    /// XPLMSetWindowPositioningMode() API to make your window "popped out" into a
    /// first-class operating system window.
    /// </para>
    /// <para>
    /// Note that this requires dealing with your window's bounds in "global
    /// desktop" positioning units, rather than the traditional panel coordinate
    /// system. In global desktop coordinates, the main X-Plane window may not have
    /// its origin at coordinate (0, 0), and your own window may have negative
    /// coordinates. Assuming you don't implicitly assume (0, 0) as your origin,
    /// the only API change you should need is to start using
    /// XPLMGetMouseLocationGlobal() rather than XPLMGetMouseLocation(), and
    /// XPLMGetScreenBoundsGlobal() instead of XPLMGetScreenSize().
    /// </para>
    /// <para>
    /// If you ask to be decorated as a floating window, you'll get the blue window
    /// control bar and blue backing that you see in X-Plane 11's normal "floating"
    /// windows (like the map).
    /// </para>
    /// </summary>
    public unsafe partial struct CreateWindow
    {
        public int structSize;
        public int left;
        public int top;
        public int right;
        public int bottom;
        public int visible;
        [ManagedTypeAttribute(typeof(DrawWindowCallback))]
        public delegate* unmanaged[Cdecl]<WindowID, void*, void> drawWindowFunc;
        [ManagedTypeAttribute(typeof(HandleMouseClickCallback))]
        public delegate* unmanaged[Cdecl]<WindowID, int, int, MouseStatus, void*, int> handleMouseClickFunc;
        [ManagedTypeAttribute(typeof(HandleKeyCallback))]
        public delegate* unmanaged[Cdecl]<WindowID, byte, KeyFlags, byte, void*, int, void> handleKeyFunc;
        [ManagedTypeAttribute(typeof(HandleCursorCallback))]
        public delegate* unmanaged[Cdecl]<WindowID, int, int, void*, CursorStatus> handleCursorFunc;
        [ManagedTypeAttribute(typeof(HandleMouseWheelCallback))]
        public delegate* unmanaged[Cdecl]<WindowID, int, int, int, int, void*, int> handleMouseWheelFunc;
        public void* refcon;
        public WindowDecoration decorateAsFloatingWindow;
        public WindowLayer layer;
        [ManagedTypeAttribute(typeof(HandleMouseClickCallback))]
        public delegate* unmanaged[Cdecl]<WindowID, int, int, MouseStatus, void*, int> handleRightClickFunc;
    }
}