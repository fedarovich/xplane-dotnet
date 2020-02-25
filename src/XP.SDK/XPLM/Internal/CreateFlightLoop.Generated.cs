using System;
using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM.Internal
{
    public unsafe partial struct CreateFlightLoop
    {
        public int structSize;
        public FlightLoopPhaseType phase;
        [ManagedTypeAttribute(typeof(FlightLoopCallback))]
        public IntPtr callbackFunc;
        public void *refcon;
    }
}