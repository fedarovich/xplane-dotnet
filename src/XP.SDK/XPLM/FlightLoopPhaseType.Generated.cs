using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// You can register a flight loop callback to run either before or after the
    /// flight model is integrated by X-Plane.
    /// </para>
    /// </summary>
    public enum FlightLoopPhaseType
    {
        BeforeFlightModel = 0,
        AfterFlightModel = 1
    }
}