using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// This constant indicates which part of drawing we are in.  Drawing is done
    /// from the back to the front.  We get a callback before or after each item.
    /// Metaphases provide access to the beginning and end of the 3d (scene) and
    /// 2d (cockpit) drawing in a manner that is independent of new phases added
    /// via X-Plane implementation.
    /// </para>
    /// <para>
    /// **NOTE**: As of XPLM302 the legacy 3D drawing phases (xplm_Phase_FirstScene
    /// to xplm_Phase_LastScene) are deprecated. When running under X-Plane 11.50
    /// with the modern Vulkan or Metal backend, X-Plane will no longer call
    /// these drawing phases. There is a new drawing phase, xplm_Phase_Modern3D,
    /// which is supported under OpenGL and Vulkan which is called out roughly
    /// where the old before xplm_Phase_Airplanes phase was for blending. This
    /// phase is *NOT* supported under Metal and comes with potentially
    /// substantial performance overhead. Please do *NOT* opt into this phase if
    /// you don't do any actual drawing that requires the depth buffer in some
    /// way!
    /// </para>
    /// <para>
    /// **WARNING**: As X-Plane's scenery evolves, some drawing phases may cease to
    /// exist and new ones may be invented.  If you need a particularly specific
    /// use of these codes, consult Austin and/or be prepared to revise your code
    /// as X-Plane evolves.
    /// </para>
    /// </summary>
    public enum DrawingPhase
    {
        Modern3D = 31,
        FirstCockpit = 35,
        Panel = 40,
        Gauges = 45,
        Window = 50,
        LastCockpit = 55,
        LocalMap3D = 100,
        LocalMap2D = 101,
        LocalMapProfile = 102
    }
}