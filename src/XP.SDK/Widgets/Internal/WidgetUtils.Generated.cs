using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.Widgets.Internal
{
    public static partial class WidgetUtils
    {
        private static IntPtr CreateWidgetsPtr;
        private static IntPtr MoveWidgetByPtr;
        private static IntPtr FixedLayoutPtr;
        private static IntPtr SelectIfNeededPtr;
        private static IntPtr DefocusKeyboardPtr;
        private static IntPtr DragWidgetPtr;

        static WidgetUtils()
        {
            const string libraryName = "Widgets";
            CreateWidgetsPtr = FunctionResolver.Resolve(libraryName, "XPUCreateWidgets");
            MoveWidgetByPtr = FunctionResolver.Resolve(libraryName, "XPUMoveWidgetBy");
            FixedLayoutPtr = FunctionResolver.Resolve(libraryName, "XPUFixedLayout");
            SelectIfNeededPtr = FunctionResolver.Resolve(libraryName, "XPUSelectIfNeeded");
            DefocusKeyboardPtr = FunctionResolver.Resolve(libraryName, "XPUDefocusKeyboard");
            DragWidgetPtr = FunctionResolver.Resolve(libraryName, "XPUDragWidget");
        }

        
        /// <summary>
        /// <para>
        /// This function creates a series of widgets from a table...see
        /// XPCreateWidget_t above. Pass in an array of widget creation structures and
        /// an array of widget IDs that will receive each widget.
        /// </para>
        /// <para>
        /// Widget parents are specified by index into the created widget table,
        /// allowing you to create nested widget structures. You can create multiple
        /// widget trees in one table. Generally you should create widget trees from
        /// the top down.
        /// </para>
        /// <para>
        /// You can also pass in a widget ID that will be used when the widget's parent
        /// is listed as PARAM_PARENT; this allows you to embed widgets created with
        /// XPUCreateWidgets in a widget created previously.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void CreateWidgets(WidgetCreate* inWidgetDefs, int inCount, WidgetID inParamParent, WidgetID* ioWidgets)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidgetDefs);
            IL.Push(inCount);
            IL.Push(inParamParent);
            IL.Push(ioWidgets);
            IL.Push(CreateWidgetsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetCreate*), typeof(int), typeof(WidgetID), typeof(WidgetID*)));
        }

        
        /// <summary>
        /// <para>
        /// Simply moves a widget by an amount, +x = right, +y=up, without resizing the
        /// widget.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void MoveWidgetBy(WidgetID inWidget, int inDeltaX, int inDeltaY)
        {
            IL.DeclareLocals(false);
            IL.Push(inWidget);
            IL.Push(inDeltaX);
            IL.Push(inDeltaY);
            IL.Push(MoveWidgetByPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(WidgetID), typeof(int), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This function causes the widget to maintain its children in fixed position
        /// relative to itself as it is resized. Use this on the top level 'window'
        /// widget for your window.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int FixedLayout(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMessage);
            IL.Push(inWidget);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(FixedLayoutPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetMessage), typeof(WidgetID), typeof(IntPtr), typeof(IntPtr)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This causes the widget to bring its window to the foreground if it is not
        /// already. inEatClick specifies whether clicks in the background should be
        /// consumed by bringin the window to the foreground.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int SelectIfNeeded(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inEatClick)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMessage);
            IL.Push(inWidget);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(inEatClick);
            IL.Push(SelectIfNeededPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetMessage), typeof(WidgetID), typeof(IntPtr), typeof(IntPtr), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This causes a click in the widget to send keyboard focus back to X-Plane.
        /// This stops editing of any text fields, etc.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int DefocusKeyboard(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inEatClick)
        {
            IL.DeclareLocals(false);
            int result;
            IL.Push(inMessage);
            IL.Push(inWidget);
            IL.Push(inParam1);
            IL.Push(inParam2);
            IL.Push(inEatClick);
            IL.Push(DefocusKeyboardPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetMessage), typeof(WidgetID), typeof(IntPtr), typeof(IntPtr), typeof(int)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// XPUDragWidget drags the widget in response to mouse clicks. Pass in not
        /// only the event, but the global coordinates of the drag region, which might
        /// be a sub-region of your widget (for example, a title bar).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int DragWidget(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inLeft, int inTop, int inRight, int inBottom)
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
            IL.Push(DragWidgetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(WidgetMessage), typeof(WidgetID), typeof(IntPtr), typeof(IntPtr), typeof(int), typeof(int), typeof(int), typeof(int)));
            IL.Pop(out result);
            return result;
        }
    }
}