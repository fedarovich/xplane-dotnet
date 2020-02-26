using System;
using XP.SDK.XPLM;

namespace XP.SDK.Widgets.Internal
{
    
    /// <summary>
    /// <para>
    /// When a key is pressed, a pointer to this struct is passed to your widget
    /// function.
    /// </para>
    /// </summary>
    public partial struct KeyState
    {
        public byte key;
        public KeyFlags flags;
        public byte vkey;
    }
}