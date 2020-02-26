using System;

namespace XP.SDK.Widgets
{
    
    /// <summary>
    /// <para>
    /// Widgets receive 32-bit messages indicating what action is to be taken or
    /// notifications of events. The list of messages may be expanded.
    /// </para>
    /// </summary>
    public enum WidgetMessage
    {
        None = 0,
        Create = 1,
        Destroy = 2,
        Paint = 3,
        Draw = 4,
        KeyPress = 5,
        KeyTakeFocus = 6,
        KeyLoseFocus = 7,
        MouseDown = 8,
        MouseDrag = 9,
        MouseUp = 10,
        Reshape = 11,
        ExposedChanged = 12,
        AcceptChild = 13,
        LoseChild = 14,
        AcceptParent = 15,
        Shown = 16,
        Hidden = 17,
        DescriptorChanged = 18,
        PropertyChanged = 19,
        MouseWheel = 20,
        CursorAdjust = 21,
        UserStart = 10000
    }
}