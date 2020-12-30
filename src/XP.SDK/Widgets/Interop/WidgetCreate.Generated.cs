using System;

namespace XP.SDK.Widgets.Interop
{
    
    /// <summary>
    /// <para>
    /// This structure contains all of the parameters needed to create a wiget. It
    /// is used with XPUCreateWidgets to create widgets in bulk from an array. All
    /// parameters correspond to those of XPCreateWidget except for the container
    /// index.
    /// </para>
    /// <para>
    /// If the container index is equal to the index of a widget in the array, the
    /// widget in the array passed to XPUCreateWidgets is used as the parent of
    /// this widget. Note that if you pass an index greater than your own position
    /// in the array, the parent you are requesting will not exist yet.
    /// </para>
    /// <para>
    /// If the container index is NO_PARENT, the parent widget is specified as
    /// NULL. If the container index is PARAM_PARENT, the widget passed into
    /// XPUCreateWidgets is used.
    /// </para>
    /// </summary>
    public unsafe partial struct WidgetCreate
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public int visible;
        public byte* descriptor;
        public int isRoot;
        public int containerIndex;
        public WidgetClass widgetClass;
    }
}