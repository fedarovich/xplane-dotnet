using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// These bitfields define modifier keys in a platform independent way. When a
    /// key is pressed, a series of messages are sent to your plugin.  The down
    /// flag is set in the first of these messages, and the up flag in the last.
    /// While the key is held down, messages are sent with neither to indicate that
    /// the key is being held down as a repeated character.
    /// </para>
    /// <para>
    /// The control flag is mapped to the control flag on Macintosh and PC.
    /// Generally X-Plane uses the control key and not the command key on
    /// Macintosh, providing a consistent interface across platforms that does not
    /// necessarily match the Macintosh user interface guidelines.  There is not
    /// yet a way for plugins to access the Macintosh control keys without using
    /// #ifdefed code.
    /// </para>
    /// </summary>
    [Flags]
    public enum KeyFlags
    {
        ShiftFlag = 1,
        OptionAltFlag = 2,
        ControlFlag = 4,
        DownFlag = 8,
        UpFlag = 16
    }
}