using InlineIL;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XP.SDK.Widgets.Internal
{
    public static partial class WidgetUtils
    {
        private static IntPtr UCreateWidgetsPtr;
        private static IntPtr UMoveWidgetByPtr;
        private static IntPtr UFixedLayoutPtr;
        private static IntPtr USelectIfNeededPtr;
        private static IntPtr UDefocusKeyboardPtr;
        private static IntPtr UDragWidgetPtr;
        static WidgetUtils()
        {
            const string libraryName = "Widgets";
            UCreateWidgetsPtr = FunctionResolver.Resolve(libraryName, "XPUCreateWidgets");
            UMoveWidgetByPtr = FunctionResolver.Resolve(libraryName, "XPUMoveWidgetBy");
            UFixedLayoutPtr = FunctionResolver.Resolve(libraryName, "XPUFixedLayout");
            USelectIfNeededPtr = FunctionResolver.Resolve(libraryName, "XPUSelectIfNeeded");
            UDefocusKeyboardPtr = FunctionResolver.Resolve(libraryName, "XPUDefocusKeyboard");
            UDragWidgetPtr = FunctionResolver.Resolve(libraryName, "XPUDragWidget");
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void UCreateWidgets(WidgetCreate*inWidgetDefs, int inCount, WidgetID inParamParent, WidgetID*ioWidgets)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidgetDefs);
            IL.Push(inCount);
            IL.Push(inParamParent);
            IL.Push(ioWidgets);
            IL.Push(UCreateWidgetsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetCreate*), typeof(int), typeof(WidgetID), typeof(WidgetID*)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void UMoveWidgetBy(WidgetID inWidget, int inDeltaX, int inDeltaY)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inDeltaX);
            IL.Push(inDeltaY);
            IL.Push(UMoveWidgetByPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int), typeof(int)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int UFixedLayout(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMessage);
            IL.Push(inWidget);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(UFixedLayoutPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetMessage), typeof(WidgetID), typeof(IntPtr), typeof(IntPtr)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int USelectIfNeeded(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inEatClick)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMessage);
            IL.Push(inWidget);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(inEatClick);
            IL.Push(USelectIfNeededPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetMessage), typeof(WidgetID), typeof(IntPtr), typeof(IntPtr), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int UDefocusKeyboard(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inEatClick)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMessage);
            IL.Push(inWidget);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(inEatClick);
            IL.Push(UDefocusKeyboardPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetMessage), typeof(WidgetID), typeof(IntPtr), typeof(IntPtr), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int UDragWidget(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inLeft, int inTop, int inRight, int inBottom)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMessage);
            IL.Push(inWidget);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(inLeft);
            IL.Push(inTop);
            IL.Push(inRight);
            IL.Push(inBottom);
            IL.Push(UDragWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetMessage), typeof(WidgetID), typeof(IntPtr), typeof(IntPtr), typeof(int), typeof(int), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }
    }
}