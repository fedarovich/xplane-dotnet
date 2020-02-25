using System;

namespace XP.SDK.XPLM
{
    [Flags]
    public enum NavType
    {
        Unknown = 0,
        Airport = 1,
        NDB = 2,
        VOR = 4,
        ILS = 8,
        Localizer = 16,
        GlideSlope = 32,
        OuterMarker = 64,
        MiddleMarker = 128,
        InnerMarker = 256,
        Fix = 512,
        DME = 1024,
        LatLon = 2048
    }
}