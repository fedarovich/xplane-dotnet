using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// XPLMCursorStatus describes how you would like X-Plane to manage the cursor.
    /// See XPLMHandleCursor_f for more info.
    /// </para>
    /// </summary>
    public enum CursorStatus
    {
        Default = 0,
        Hidden = 1,
        Arrow = 2,
        Custom = 3
    }
}