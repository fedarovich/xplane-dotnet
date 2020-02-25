using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.Widgets.Internal
{
    public static partial class Widgets
    {
        private static IntPtr CreateWidgetPtr;
        private static IntPtr CreateCustomWidgetPtr;
        private static IntPtr DestroyWidgetPtr;
        private static IntPtr SendMessageToWidgetPtr;
        private static IntPtr PlaceWidgetWithinPtr;
        private static IntPtr CountChildWidgetsPtr;
        private static IntPtr GetNthChildWidgetPtr;
        private static IntPtr GetParentWidgetPtr;
        private static IntPtr ShowWidgetPtr;
        private static IntPtr HideWidgetPtr;
        private static IntPtr IsWidgetVisiblePtr;
        private static IntPtr FindRootWidgetPtr;
        private static IntPtr BringRootWidgetToFrontPtr;
        private static IntPtr IsWidgetInFrontPtr;
        private static IntPtr GetWidgetGeometryPtr;
        private static IntPtr SetWidgetGeometryPtr;
        private static IntPtr GetWidgetForLocationPtr;
        private static IntPtr GetWidgetExposedGeometryPtr;
        private static IntPtr SetWidgetDescriptorPtr;
        private static IntPtr GetWidgetDescriptorPtr;
        private static IntPtr GetWidgetUnderlyingWindowPtr;
        private static IntPtr SetWidgetPropertyPtr;
        private static IntPtr GetWidgetPropertyPtr;
        private static IntPtr SetKeyboardFocusPtr;
        private static IntPtr LoseKeyboardFocusPtr;
        private static IntPtr GetWidgetWithFocusPtr;
        private static IntPtr AddWidgetCallbackPtr;
        private static IntPtr GetWidgetClassFuncPtr;
        static Widgets()
        {
            const string libraryName = "Widgets";
            CreateWidgetPtr = FunctionResolver.Resolve(libraryName, "XPCreateWidget");
            CreateCustomWidgetPtr = FunctionResolver.Resolve(libraryName, "XPCreateCustomWidget");
            DestroyWidgetPtr = FunctionResolver.Resolve(libraryName, "XPDestroyWidget");
            SendMessageToWidgetPtr = FunctionResolver.Resolve(libraryName, "XPSendMessageToWidget");
            PlaceWidgetWithinPtr = FunctionResolver.Resolve(libraryName, "XPPlaceWidgetWithin");
            CountChildWidgetsPtr = FunctionResolver.Resolve(libraryName, "XPCountChildWidgets");
            GetNthChildWidgetPtr = FunctionResolver.Resolve(libraryName, "XPGetNthChildWidget");
            GetParentWidgetPtr = FunctionResolver.Resolve(libraryName, "XPGetParentWidget");
            ShowWidgetPtr = FunctionResolver.Resolve(libraryName, "XPShowWidget");
            HideWidgetPtr = FunctionResolver.Resolve(libraryName, "XPHideWidget");
            IsWidgetVisiblePtr = FunctionResolver.Resolve(libraryName, "XPIsWidgetVisible");
            FindRootWidgetPtr = FunctionResolver.Resolve(libraryName, "XPFindRootWidget");
            BringRootWidgetToFrontPtr = FunctionResolver.Resolve(libraryName, "XPBringRootWidgetToFront");
            IsWidgetInFrontPtr = FunctionResolver.Resolve(libraryName, "XPIsWidgetInFront");
            GetWidgetGeometryPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetGeometry");
            SetWidgetGeometryPtr = FunctionResolver.Resolve(libraryName, "XPSetWidgetGeometry");
            GetWidgetForLocationPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetForLocation");
            GetWidgetExposedGeometryPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetExposedGeometry");
            SetWidgetDescriptorPtr = FunctionResolver.Resolve(libraryName, "XPSetWidgetDescriptor");
            GetWidgetDescriptorPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetDescriptor");
            GetWidgetUnderlyingWindowPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetUnderlyingWindow");
            SetWidgetPropertyPtr = FunctionResolver.Resolve(libraryName, "XPSetWidgetProperty");
            GetWidgetPropertyPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetProperty");
            SetKeyboardFocusPtr = FunctionResolver.Resolve(libraryName, "XPSetKeyboardFocus");
            LoseKeyboardFocusPtr = FunctionResolver.Resolve(libraryName, "XPLoseKeyboardFocus");
            GetWidgetWithFocusPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetWithFocus");
            AddWidgetCallbackPtr = FunctionResolver.Resolve(libraryName, "XPAddWidgetCallback");
            GetWidgetClassFuncPtr = FunctionResolver.Resolve(libraryName, "XPGetWidgetClassFunc");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WidgetID CreateWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, byte *inDescriptor, int inIsRoot, WidgetID inContainer, WidgetClass inClass)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(inVisible);
            IL.Push(inDescriptor);
            IL.Push(inIsRoot);
            IL.Push(inContainer);
            IL.Push(inClass);
            IL.Push(CreateWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(byte *), typeof(int), typeof(WidgetID), typeof(WidgetClass)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe WidgetID CreateCustomWidget(int inLeft, int inTop, int inRight, int inBottom, int inVisible, byte *inDescriptor, int inIsRoot, WidgetID inContainer, WidgetFuncCallback inCallback)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(inVisible);
            IL.Push(inDescriptor);
            IL.Push(inIsRoot);
            IL.Push(inContainer);
            IL.Push(inCallbackPtr);
            IL.Push(CreateCustomWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(byte *), typeof(int), typeof(WidgetID), typeof(IntPtr)));
            IL.Pop(out result);
            GC.KeepAlive(inCallback);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyWidget(WidgetID inWidget, int inDestroyChildren)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inDestroyChildren);
            IL.Push(DestroyWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int SendMessageToWidget(WidgetID inWidget, WidgetMessage inMessage, DispatchMode inMode, IntPtr inParam1, IntPtr inParam2)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(inMessage);
            IL.Push(inMode);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(SendMessageToWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID), typeof(WidgetMessage), typeof(DispatchMode), typeof(IntPtr), typeof(IntPtr)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void PlaceWidgetWithin(WidgetID inSubWidget, WidgetID inContainer)
        {
            IL.DeclareLocals(false);
            IL.Push(inSubWidget);
            IL.Push(inContainer);
            IL.Push(PlaceWidgetWithinPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(WidgetID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountChildWidgets(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(CountChildWidgetsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID GetNthChildWidget(WidgetID inWidget, int inIndex)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inWidget);
            IL.Push(inIndex);
            IL.Push(GetNthChildWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID GetParentWidget(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inWidget);
            IL.Push(GetParentWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ShowWidget(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(ShowWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void HideWidget(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(HideWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsWidgetVisible(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(IsWidgetVisiblePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID FindRootWidget(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inWidget);
            IL.Push(FindRootWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void BringRootWidgetToFront(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(BringRootWidgetToFrontPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int IsWidgetInFront(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(IsWidgetInFrontPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWidgetGeometry(WidgetID inWidget, int *outLeft, int *outTop, int *outRight, int *outBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetWidgetGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int *), typeof(int *), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWidgetGeometry(WidgetID inWidget, int inLeft, int inTop, int inRight, int inBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(SetWidgetGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int), typeof(int), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID GetWidgetForLocation(WidgetID inContainer, int inXOffset, int inYOffset, int inRecursive, int inVisibleOnly)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inContainer);
            IL.Push(inXOffset);
            IL.Push(inYOffset);
            IL.Push(inRecursive);
            IL.Push(inVisibleOnly);
            IL.Push(GetWidgetForLocationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID), typeof(int), typeof(int), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void GetWidgetExposedGeometry(WidgetID inWidgetID, int *outLeft, int *outTop, int *outRight, int *outBottom)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidgetID);
            IL.Push(outLeft);
            IL.Push(outTop);
            IL.Push(outRight);
            IL.Push(outBottom);
            IL.Push(GetWidgetExposedGeometryPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int *), typeof(int *), typeof(int *), typeof(int *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void SetWidgetDescriptor(WidgetID inWidget, byte *inDescriptor)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inDescriptor);
            IL.Push(SetWidgetDescriptorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(byte *)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int GetWidgetDescriptor(WidgetID inWidget, byte *outDescriptor, int inMaxDescLength)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inWidget);
            IL.Push(outDescriptor);
            IL.Push(inMaxDescLength);
            IL.Push(GetWidgetDescriptorPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetID), typeof(byte *), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WindowID GetWidgetUnderlyingWindow(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            WindowID result;
            IL.Push(inWidget);
            IL.Push(GetWidgetUnderlyingWindowPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WindowID), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetWidgetProperty(WidgetID inWidget, WidgetPropertyID inProperty, IntPtr inValue)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inProperty);
            IL.Push(inValue);
            IL.Push(SetWidgetPropertyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(WidgetPropertyID), typeof(IntPtr)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr GetWidgetProperty(WidgetID inWidget, WidgetPropertyID inProperty, int *inExists)
        {
            IL.DeclareLocals(false);
            IntPtr result;
            IL.Push(inWidget);
            IL.Push(inProperty);
            IL.Push(inExists);
            IL.Push(GetWidgetPropertyPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(IntPtr), typeof(WidgetID), typeof(WidgetPropertyID), typeof(int *)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID SetKeyboardFocus(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(inWidget);
            IL.Push(SetKeyboardFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID), typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void LoseKeyboardFocus(WidgetID inWidget)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(LoseKeyboardFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetID GetWidgetWithFocus()
        {
            IL.DeclareLocals(false);
            WidgetID result;
            IL.Push(GetWidgetWithFocusPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void AddWidgetCallback(WidgetID inWidget, WidgetFuncCallback inNewCallback)
        {
            IL.DeclareLocals(false);
            IntPtr inNewCallbackPtr = Marshal.GetFunctionPointerForDelegate(inNewCallback);
            IL.Push(inWidget);
            IL.Push(inNewCallbackPtr);
            IL.Push(AddWidgetCallbackPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(IntPtr)));
            GC.KeepAlive(inNewCallback);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static WidgetFuncCallback GetWidgetClassFunc(WidgetClass inWidgetClass)
        {
            IL.DeclareLocals(false);
            WidgetFuncCallback result;
            IL.Push(inWidgetClass);
            IL.Push(GetWidgetClassFuncPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(WidgetFuncCallback), typeof(WidgetClass)));
            IL.Pop(out result);
            return result;
        }
    }
}