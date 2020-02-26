using System;

namespace XP.SDK.Widgets
{
    
    /// <summary>
    /// <para>
    /// Properties are values attached to instances of your widgets. A property is
    /// identified by a 32-bit ID and its value is the width of a pointer.
    /// </para>
    /// <para>
    /// Each widget instance may have a property or not have it. When you set a
    /// property on a widget for the first time, the property is added to the
    /// widget; it then stays there for the life of the widget.
    /// </para>
    /// <para>
    /// Some property IDs are predefined by the widget package; you can make up
    /// your own property IDs as well.
    /// </para>
    /// </summary>
    public enum WidgetPropertyID
    {
        Refcon = 0,
        Dragging = 1,
        DragXOff = 2,
        DragYOff = 3,
        Hilited = 4,
        Object = 5,
        Clip = 6,
        Enabled = 7,
        UserStart = 10000
    }
}