using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// These enumerations define the various 'check' states for an X-Plane menu.
    /// 'checking' in X-Plane actually appears as a light which may or may not be
    /// lit.  So there are three possible states.
    /// </para>
    /// </summary>
    public enum MenuCheck
    {
        NoCheck = 0,
        Unchecked = 1,
        Checked = 2
    }
}