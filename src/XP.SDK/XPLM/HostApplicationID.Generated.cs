using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// While the plug-in SDK is currently only accessible to plugins running
    /// inside X-Plane, the original authors considered extending the API to other
    /// applications that shared basic infrastructure with X-Plane. These
    /// enumerations defined some of those applications.  As of this writing, only
    /// X-Plane is available as a host application.
    /// </para>
    /// </summary>
    public enum HostApplicationID
    {
        Unknown = 0,
        XPlane = 1,
        PlaneMaker = 2,
        WorldMaker = 3,
        Briefer = 4,
        PartMaker = 5,
        YoungsMod = 6,
        XAuto = 7
    }
}