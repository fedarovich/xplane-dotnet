using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Display
    {
        private static IntPtr RegisterDrawCallbackPtr;
        private static IntPtr UnregisterDrawCallbackPtr;
        private static IntPtr CreateWindowExPtr;
        private static IntPtr CreateWindowPtr;
        private static IntPtr DestroyWindowPtr;
        private static IntPtr GetScreenSizePtr;
        private static IntPtr GetScreenBoundsGlobalPtr;
        private static IntPtr GetAllMonitorBoundsGlobalPtr;
        private static IntPtr GetAllMonitorBoundsOSPtr;
        private static IntPtr GetMouseLocationPtr;
        private static IntPtr GetMouseLocationGlobalPtr;
        private static IntPtr GetWindowGeometryPtr;
        private static IntPtr SetWindowGeometryPtr;
        private static IntPtr GetWindowGeometryOSPtr;
        private static IntPtr SetWindowGeometryOSPtr;
        private static IntPtr GetWindowGeometryVRPtr;
        private static IntPtr SetWindowGeometryVRPtr;
        private static IntPtr GetWindowIsVisiblePtr;
        private static IntPtr SetWindowIsVisiblePtr;
        private static IntPtr WindowIsPoppedOutPtr;
        private static IntPtr WindowIsInVRPtr;
        private static IntPtr SetWindowGravityPtr;
        private static IntPtr SetWindowResizingLimitsPtr;
        private static IntPtr SetWindowPositioningModePtr;
        private static IntPtr SetWindowTitlePtr;
        private static IntPtr GetWindowRefConPtr;
        private static IntPtr SetWindowRefConPtr;
        private static IntPtr TakeKeyboardFocusPtr;
        private static IntPtr HasKeyboardFocusPtr;
        private static IntPtr BringWindowToFrontPtr;
        private static IntPtr IsWindowInFrontPtr;
        private static IntPtr RegisterKeySnifferPtr;
        private static IntPtr UnregisterKeySnifferPtr;
        private static IntPtr RegisterHotKeyPtr;
        private static IntPtr UnregisterHotKeyPtr;
        private static IntPtr CountHotKeysPtr;
        private static IntPtr GetNthHotKeyPtr;
        private static IntPtr GetHotKeyInfoPtr;
        private static IntPtr SetHotKeyCombinationPtr;
        static Display()
        {
            const string libraryName = "XPLM";
            RegisterDrawCallbackPtr = FunctionResolver.Resolve(libraryName, "XPLMRegisterDrawCallback");
            UnregisterDrawCallbackPtr = FunctionResolver.Resolve(libraryName, "XPLMUnregisterDrawCallback");
            CreateWindowExPtr = FunctionResolver.Resolve(libraryName, "XPLMCreateWindowEx");
            CreateWindowPtr = FunctionResolver.Resolve(libraryName, "XPLMCreateWindow");
            DestroyWindowPtr = FunctionResolver.Resolve(libraryName, "XPLMDestroyWindow");
            GetScreenSizePtr = FunctionResolver.Resolve(libraryName, "XPLMGetScreenSize");
            GetScreenBoundsGlobalPtr = FunctionResolver.Resolve(libraryName, "XPLMGetScreenBoundsGlobal");
            GetAllMonitorBoundsGlobalPtr = FunctionResolver.Resolve(libraryName, "XPLMGetAllMonitorBoundsGlobal");
            GetAllMonitorBoundsOSPtr = FunctionResolver.Resolve(libraryName, "XPLMGetAllMonitorBoundsOS");
            GetMouseLocationPtr = FunctionResolver.Resolve(libraryName, "XPLMGetMouseLocation");
            GetMouseLocationGlobalPtr = FunctionResolver.Resolve(libraryName, "XPLMGetMouseLocationGlobal");
            GetWindowGeometryPtr = FunctionResolver.Resolve(libraryName, "XPLMGetWindowGeometry");
            SetWindowGeometryPtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowGeometry");
            GetWindowGeometryOSPtr = FunctionResolver.Resolve(libraryName, "XPLMGetWindowGeometryOS");
            SetWindowGeometryOSPtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowGeometryOS");
            GetWindowGeometryVRPtr = FunctionResolver.Resolve(libraryName, "XPLMGetWindowGeometryVR");
            SetWindowGeometryVRPtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowGeometryVR");
            GetWindowIsVisiblePtr = FunctionResolver.Resolve(libraryName, "XPLMGetWindowIsVisible");
            SetWindowIsVisiblePtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowIsVisible");
            WindowIsPoppedOutPtr = FunctionResolver.Resolve(libraryName, "XPLMWindowIsPoppedOut");
            WindowIsInVRPtr = FunctionResolver.Resolve(libraryName, "XPLMWindowIsInVR");
            SetWindowGravityPtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowGravity");
            SetWindowResizingLimitsPtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowResizingLimits");
            SetWindowPositioningModePtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowPositioningMode");
            SetWindowTitlePtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowTitle");
            GetWindowRefConPtr = FunctionResolver.Resolve(libraryName, "XPLMGetWindowRefCon");
            SetWindowRefConPtr = FunctionResolver.Resolve(libraryName, "XPLMSetWindowRefCon");
            TakeKeyboardFocusPtr = FunctionResolver.Resolve(libraryName, "XPLMTakeKeyboardFocus");
            HasKeyboardFocusPtr = FunctionResolver.Resolve(libraryName, "XPLMHasKeyboardFocus");
            BringWindowToFrontPtr = FunctionResolver.Resolve(libraryName, "XPLMBringWindowToFront");
            IsWindowInFrontPtr = FunctionResolver.Resolve(libraryName, "XPLMIsWindowInFront");
            RegisterKeySnifferPtr = FunctionResolver.Resolve(libraryName, "XPLMRegisterKeySniffer");
            UnregisterKeySnifferPtr = FunctionResolver.Resolve(libraryName, "XPLMUnregisterKeySniffer");
            RegisterHotKeyPtr = FunctionResolver.Resolve(libraryName, "XPLMRegisterHotKey");
            UnregisterHotKeyPtr = FunctionResolver.Resolve(libraryName, "XPLMUnregisterHotKey");
            CountHotKeysPtr = FunctionResolver.Resolve(libraryName, "XPLMCountHotKeys");
            GetNthHotKeyPtr = FunctionResolver.Resolve(libraryName, "XPLMGetNthHotKey");
            GetHotKeyInfoPtr = FunctionResolver.Resolve(libraryName, "XPLMGetHotKeyInfo");
            SetHotKeyCombinationPtr = FunctionResolver.Resolve(libraryName, "XPLMSetHotKeyCombination");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int RegisterDrawCallback(DrawCallback inCallback, DrawingPhase inPhase, int inWantsBefore, void *inRefcon)
        {
            IL.DeclareLocals(false);
            int result;
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inCallbackPtr);
            IL.Push(inPhase);
            IL.Push(inWantsBefore);
            IL.Push(inRefcon);
            IL.Push(RegisterDrawCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(IntPtr), typeof(DrawingPhase), typeof(int), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inCallback);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int UnregisterDrawCallback(DrawCallback inCallback, DrawingPhase inPhase, int inWantsBefore, void *inRefcon)
        {
            IL.DeclareLocals(false);
            int result;
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inCallbackPtr);
            IL.Push(inPhase);
            IL.Push(inWantsBefore);
            IL.Push(inRefcon);
            IL.Push(UnregisterDrawCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(IntPtr), typeof(DrawingPhase), typeof(int), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inCallback);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WindowID CreateWindowEx(CreateWindow*inParams)
        {
            IL.DeclareLocals(false);
            WindowID result;
            IL.Push(inParams);
            IL.Push(CreateWindowExPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WindowID), typeof(CreateWindow*)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WindowID CreateWindow(int inLeft, int inTop, int inRight, int inBottom, int inIsVisible, DrawWindowCallback inDrawCallback, HandleKeyCallback inKeyCallback, HandleMouseClickCallback inMouseCallback, void *inRefcon)
        {
            IL.DeclareLocals(false);
            WindowID result;
            IntPtr inDrawCallbackPtr = Marshal.GetFunctionPointerForDelegate(inDrawCallback);
            IntPtr inKeyCallbackPtr = Marshal.GetFunctionPointerForDelegate(inKeyCallback);
            IntPtr inMouseCallbackPtr = Marshal.GetFunctionPointerForDelegate(inMouseCallback);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(inIsVisible);
            IL.Push(inDrawCallbackPtr);
            IL.Push(inKeyCallbackPtr);
            IL.Push(inMouseCallbackPtr);
            IL.Push(inRefcon);
            IL.Push(CreateWindowPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WindowID), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inMouseCallback);
            GC.KeepAlive(inKeyCallback);
            GC.KeepAlive(inDrawCallback);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyWindow(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(DestroyWindowPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetScreenSize(int *outWidth, int *outHeight)
        {
            IL.DeclareLocals(false);
            IL.Push(outWidth);
            IL.Push(outHeight);
            IL.Push(GetScreenSizePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetScreenBoundsGlobal(int *outLeft, int *outTop, int *outRight, int *outBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetScreenBoundsGlobalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int *), typeof(int *), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetAllMonitorBoundsGlobal(ReceiveMonitorBoundsGlobalCallback inMonitorBoundsCallback, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inMonitorBoundsCallbackPtr = Marshal.GetFunctionPointerForDelegate(inMonitorBoundsCallback);
            IL.Push(inMonitorBoundsCallbackPtr);
            IL.Push(inRefcon);
            IL.Push(GetAllMonitorBoundsGlobalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(void *)));
            GC.KeepAlive(inMonitorBoundsCallback);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetAllMonitorBoundsOS(ReceiveMonitorBoundsOSCallback inMonitorBoundsCallback, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inMonitorBoundsCallbackPtr = Marshal.GetFunctionPointerForDelegate(inMonitorBoundsCallback);
            IL.Push(inMonitorBoundsCallbackPtr);
            IL.Push(inRefcon);
            IL.Push(GetAllMonitorBoundsOSPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(IntPtr), typeof(void *)));
            GC.KeepAlive(inMonitorBoundsCallback);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetMouseLocation(int *outX, int *outY)
        {
            IL.DeclareLocals(false);
            IL.Push(outX);
            IL.Push(outY);
            IL.Push(GetMouseLocationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetMouseLocationGlobal(int *outX, int *outY)
        {
            IL.DeclareLocals(false);
            IL.Push(outX);
            IL.Push(outY);
            IL.Push(GetMouseLocationGlobalPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWindowGeometry(WindowID inWindowID, int *outLeft, int *outTop, int *outRight, int *outBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetWindowGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int *), typeof(int *), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowGeometry(WindowID inWindowID, int inLeft, int inTop, int inRight, int inBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(SetWindowGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWindowGeometryOS(WindowID inWindowID, int *outLeft, int *outTop, int *outRight, int *outBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetWindowGeometryOSPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int *), typeof(int *), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowGeometryOS(WindowID inWindowID, int inLeft, int inTop, int inRight, int inBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(SetWindowGeometryOSPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWindowGeometryVR(WindowID inWindowID, int *outWidthBoxels, int *outHeightBoxels)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(outWidthBoxels);
            IL.Push(outHeightBoxels);
            IL.Push(GetWindowGeometryVRPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowGeometryVR(WindowID inWindowID, int widthBoxels, int heightBoxels)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(widthBoxels);
            IL.Push(heightBoxels);
            IL.Push(SetWindowGeometryVRPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetWindowIsVisible(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWindowID);
            IL.Push(GetWindowIsVisiblePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowIsVisible(WindowID inWindowID, int inIsVisible)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(inIsVisible);
            IL.Push(SetWindowIsVisiblePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int WindowIsPoppedOut(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWindowID);
            IL.Push(WindowIsPoppedOutPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int WindowIsInVR(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWindowID);
            IL.Push(WindowIsInVRPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowGravity(WindowID inWindowID, float inLeftGravity, float inTopGravity, float inRightGravity, float inBottomGravity)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(inLeftGravity);
            IL.Push(inTopGravity);
            IL.Push(inRightGravity);
            IL.Push(inBottomGravity);
            IL.Push(SetWindowGravityPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(float), typeof(float), typeof(float), typeof(float)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowResizingLimits(WindowID inWindowID, int inMinWidthBoxels, int inMinHeightBoxels, int inMaxWidthBoxels, int inMaxHeightBoxels)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(inMinWidthBoxels);
            IL.Push(inMinHeightBoxels);
            IL.Push(inMaxWidthBoxels);
            IL.Push(inMaxHeightBoxels);
            IL.Push(SetWindowResizingLimitsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWindowPositioningMode(WindowID inWindowID, WindowPositioningMode inPositioningMode, int inMonitorIndex)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(inPositioningMode);
            IL.Push(inMonitorIndex);
            IL.Push(SetWindowPositioningModePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(WindowPositioningMode), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWindowTitle(WindowID inWindowID, byte *inWindowTitle)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(inWindowTitle);
            IL.Push(SetWindowTitlePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void *GetWindowRefCon(WindowID inWindowID)
        {
            IL.DeclareLocals(false);
            void *result;
            IL.Push(inWindowID);
            IL.Push(GetWindowRefConPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void *), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWindowRefCon(WindowID inWindowID, void *inRefcon)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindowID);
            IL.Push(inRefcon);
            IL.Push(SetWindowRefConPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID), typeof(void *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void TakeKeyboardFocus(WindowID inWindow)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindow);
            IL.Push(TakeKeyboardFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int HasKeyboardFocus(WindowID inWindow)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWindow);
            IL.Push(HasKeyboardFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void BringWindowToFront(WindowID inWindow)
        {
            IL.DeclareLocals(false);
            IL.Push(inWindow);
            IL.Push(BringWindowToFrontPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WindowID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsWindowInFront(WindowID inWindow)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWindow);
            IL.Push(IsWindowInFrontPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WindowID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int RegisterKeySniffer(KeySnifferCallback inCallback, int inBeforeWindows, void *inRefcon)
        {
            IL.DeclareLocals(false);
            int result;
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inCallbackPtr);
            IL.Push(inBeforeWindows);
            IL.Push(inRefcon);
            IL.Push(RegisterKeySnifferPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(IntPtr), typeof(int), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inCallback);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int UnregisterKeySniffer(KeySnifferCallback inCallback, int inBeforeWindows, void *inRefcon)
        {
            IL.DeclareLocals(false);
            int result;
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inCallbackPtr);
            IL.Push(inBeforeWindows);
            IL.Push(inRefcon);
            IL.Push(UnregisterKeySnifferPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(IntPtr), typeof(int), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inCallback);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe HotKeyID RegisterHotKey(byte inVirtualKey, KeyFlags inFlags, byte *inDescription, HotKeyCallback inCallback, void *inRefcon)
        {
            IL.DeclareLocals(false);
            HotKeyID result;
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inVirtualKey);
            IL.Push(inFlags);
            IL.Push(inDescription);
            IL.Push(inCallbackPtr);
            IL.Push(inRefcon);
            IL.Push(RegisterHotKeyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(HotKeyID), typeof(byte), typeof(KeyFlags), typeof(byte *), typeof(IntPtr), typeof(void *)));
            IL.Pop(out result);
            GC.KeepAlive(inCallback);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterHotKey(HotKeyID inHotKey)
        {
            IL.DeclareLocals(false);
            IL.Push(inHotKey);
            IL.Push(UnregisterHotKeyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(HotKeyID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountHotKeys()
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(CountHotKeysPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static HotKeyID GetNthHotKey(int inIndex)
        {
            IL.DeclareLocals(false);
            HotKeyID result;
            IL.Push(inIndex);
            IL.Push(GetNthHotKeyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(HotKeyID), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetHotKeyInfo(HotKeyID inHotKey, byte *outVirtualKey, KeyFlags*outFlags, byte *outDescription, PluginID*outPlugin)
        {
            IL.DeclareLocals(false);
            IL.Push(inHotKey);
            IL.Push(outVirtualKey);
            IL.Push(outFlags);
            IL.Push(outDescription);
            IL.Push(outPlugin);
            IL.Push(GetHotKeyInfoPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(HotKeyID), typeof(byte *), typeof(KeyFlags*), typeof(byte *), typeof(PluginID*)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetHotKeyCombination(HotKeyID inHotKey, byte inVirtualKey, KeyFlags inFlags)
        {
            IL.DeclareLocals(false);
            IL.Push(inHotKey);
            IL.Push(inVirtualKey);
            IL.Push(inFlags);
            IL.Push(SetHotKeyCombinationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(HotKeyID), typeof(byte), typeof(KeyFlags)));
        }
    }
}