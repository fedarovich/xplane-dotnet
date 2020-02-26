using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// When the mouse is clicked, your mouse click routine is called repeatedly.
    /// It is first called with the mouse down message.  It is then called zero or
    /// more times with the mouse-drag message, and finally it is called once with
    /// the mouse up message.  All of these messages will be directed to the same
    /// window; you are guaranteed to not receive a drag or mouse-up event without
    /// first receiving the corresponding mouse-down.
    /// </para>
    /// </summary>
    public enum MouseStatus
    {
        Down = 1,
        Drag = 2,
        Up = 3
    }
}