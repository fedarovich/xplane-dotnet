using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// While the plug-in SDK is only accessible to plugins running inside X-Plane,
    /// the original authors considered extending the API to other applications
    /// that shared basic infrastructure with X-Plane. These enumerations are
    /// hold-overs from that original roadmap; all values other than X-Plane are
    /// deprecated. Your plugin should never need this enumeration.
    /// </para>
    /// </summary>
    public enum HostApplicationID
    {
        Unknown = 0,
        XPlane = 1
    }
}