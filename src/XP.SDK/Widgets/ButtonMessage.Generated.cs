using System;

namespace XP.SDK.Widgets
{
    
    /// <summary>
    /// <para>
    /// Button Messages
    /// </para>
    /// <para>
    /// These messages are sent by the button to itself and then up the widget
    /// chain when the button is clicked. (You may intercept them by providing a
    /// widget handler for the button itself or by providing a handler in a parent
    /// widget.)
    /// </para>
    /// </summary>
    public enum ButtonMessage
    {
        PushButtonPressed = 1300,
        ButtonStateChanged = 1301
    }
}