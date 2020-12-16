using System;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.Widgets.Internal
{
    public static partial class WidgetUtilsAPI
    {
        
        /// <summary>
        /// <para>
        /// This function creates a series of widgets from a table (see
        /// XPCreateWidget_t above). Pass in an array of widget creation structures and
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPUCreateWidgets", ExactSpelling = true)]
        public static extern unsafe void CreateWidgets(WidgetCreate* inWidgetDefs, int inCount, WidgetID inParamParent, WidgetID* ioWidgets);

        
        /// <summary>
        /// <para>
        /// Simply moves a widget by an amount, +x = right, +y=up, without resizing the
        /// widget.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPUMoveWidgetBy", ExactSpelling = true)]
        public static extern void MoveWidgetBy(WidgetID inWidget, int inDeltaX, int inDeltaY);

        
        /// <summary>
        /// <para>
        /// This function causes the widget to maintain its children in fixed position
        /// relative to itself as it is resized. Use this on the top level 'window'
        /// widget for your window.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPUFixedLayout", ExactSpelling = true)]
        public static extern int FixedLayout(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2);

        
        /// <summary>
        /// <para>
        /// This causes the widget to bring its window to the foreground if it is not
        /// already. inEatClick specifies whether clicks in the background should be
        /// consumed by bringin the window to the foreground.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPUSelectIfNeeded", ExactSpelling = true)]
        public static extern int SelectIfNeeded(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inEatClick);

        
        /// <summary>
        /// <para>
        /// This causes a click in the widget to send keyboard focus back to X-Plane.
        /// This stops editing of any text fields, etc.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPUDefocusKeyboard", ExactSpelling = true)]
        public static extern int DefocusKeyboard(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inEatClick);

        
        /// <summary>
        /// <para>
        /// XPUDragWidget drags the widget in response to mouse clicks. Pass in not
        /// only the event, but the global coordinates of the drag region, which might
        /// be a sub-region of your widget (for example, a title bar).
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPUDragWidget", ExactSpelling = true)]
        public static extern int DragWidget(WidgetMessage inMessage, WidgetID inWidget, IntPtr inParam1, IntPtr inParam2, int inLeft, int inTop, int inRight, int inBottom);
    }
}