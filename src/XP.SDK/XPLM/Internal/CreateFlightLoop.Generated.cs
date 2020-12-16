using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// XPLMCreateFlightLoop_t contains the parameters to create a new flight loop
    /// callback. The strsucture can be expanded in future SDKs - always set
    /// structSize to the size of your structure in bytes.
    /// </para>
    /// </summary>
    public unsafe partial struct CreateFlightLoop
    {
        public int structSize;
        public FlightLoopPhaseType phase;
        [ManagedTypeAttribute(typeof(FlightLoopCallback))]
        public delegate* unmanaged[Cdecl]<float, float, int, void*, float> callbackFunc;
        public void* refcon;
    }
}