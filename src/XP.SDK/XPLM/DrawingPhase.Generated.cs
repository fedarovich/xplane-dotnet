using System;

namespace XP.SDK.XPLM
{
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