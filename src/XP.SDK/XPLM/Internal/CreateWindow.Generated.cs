using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    public unsafe partial struct CreateWindow
    {
        public int structSize;
        public int left;
        public int top;
        public int right;
        public int bottom;
        public int visible;
        [ManagedTypeAttribute(typeof(DrawWindowCallback))]
        public IntPtr drawWindowFunc;
        [ManagedTypeAttribute(typeof(HandleMouseClickCallback))]
        public IntPtr handleMouseClickFunc;
        [ManagedTypeAttribute(typeof(HandleKeyCallback))]
        public IntPtr handleKeyFunc;
        [ManagedTypeAttribute(typeof(HandleCursorCallback))]
        public IntPtr handleCursorFunc;
        [ManagedTypeAttribute(typeof(HandleMouseWheelCallback))]
        public IntPtr handleMouseWheelFunc;
        public void *refcon;
        public WindowDecoration decorateAsFloatingWindow;
        public WindowLayer layer;
        [ManagedTypeAttribute(typeof(HandleMouseClickCallback))]
        public IntPtr handleRightClickFunc;
    }
}