using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// This enumeration states how long you want to retain control of the camera.
    /// You can retain it indefinitely or until the user selects a new view.
    /// </para>
    /// </summary>
    public enum CameraControlDuration
    {
        UntilViewChanges = 1,
        Forever = 2
    }
}