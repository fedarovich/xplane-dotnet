using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// You pass an XPLMFeatureEnumerator_f to get a list of all features supported
    /// by a given version running version of X-Plane.  This routine is called once
    /// for each feature.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void FeatureEnumeratorCallback(byte* inFeature, void* inRef);
}