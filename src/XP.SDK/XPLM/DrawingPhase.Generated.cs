using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// This constant indicates which part of drawing we are in.  Drawing is done
    /// from the back to the front.  We get a callback before or after each item.
    /// Metaphases provide access to the beginning and end of the 3d (scene) and 2d
    /// (cockpit) drawing in a manner that is independent of new phases added  via
    /// X-Plane implementation.
    /// </para>
    /// <para>
    /// WARNING: As X-Plane's scenery evolves, some drawing phases may cease to
    /// exist and new ones may be invented.  If you need a particularly specific
    /// use of these codes, consult Austin and/or be prepared to revise your code
    /// as X-Plane evolves.
    /// </para>
    /// </summary>
    public enum DrawingPhase
    {
        FirstScene = 0,
        Terrain = 5,
        Airports = 10,
        Vectors = 15,
        Objects = 20,
        Airplanes = 25,
        LastScene = 30,
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