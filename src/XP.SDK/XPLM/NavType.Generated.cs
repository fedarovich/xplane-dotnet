using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// These enumerations define the different types of navaids.  They are each
    /// defined with a separate bit so that they may be bit-wise added together to
    /// form sets of nav-aid types.
    /// </para>
    /// <para>
    /// NOTE: xplm_Nav_LatLon is a specific lat-lon coordinate entered into the
    /// FMS. It will not exist in the database, and cannot be programmed into the
    /// FMS. Querying the FMS for navaids will return it.  Use
    /// XPLMSetFMSEntryLatLon to set a lat/lon waypoint.
    /// </para>
    /// </summary>
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