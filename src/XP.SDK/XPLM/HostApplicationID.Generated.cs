using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// The plug-in system is based on Austin's cross-platform OpenGL framework and
    /// could theoretically be adapted to run in other apps like WorldMaker. The
    /// plug-in system also runs against a test harness for internal development
    /// and could be adapted to another flight sim (in theory at least). So an ID
    /// is providing allowing plug-ins to indentify what app they are running
    /// under.
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