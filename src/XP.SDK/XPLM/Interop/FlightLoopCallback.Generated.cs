using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// This is your flight loop callback. Each time the flight loop is iterated
    /// through, you receive this call at the end.
    /// </para>
    /// <para>
    /// Flight loop callbacks receive a number of input timing parameters. These
    /// input timing parameters are not particularly useful; you may need to track
    /// your own timing data (e.g. by reading datarefs). The input parameters are:
    /// </para>
    /// <para>
    /// - inElapsedSinceLastCall: the wall time since your last callback.
    /// - inElapsedTimeSinceLastFlightLoop: the wall time since any flight loop was
    /// dispatched.
    /// - inCounter: a monotonically increasing counter, bumped once per flight
    /// loop dispatch from the sim.
    /// - inRefcon: your own ptr constant from when you regitered yor callback.
    /// </para>
    /// <para>
    /// Your return value controls when you will next be called.
    /// </para>
    /// <para>
    /// - Return 0 to stop receiving callbacks.
    /// - Pass a positive number to specify how many seconds until the next
    /// callback. (You will be called at or after this time, not before.)
    /// - Pass a negative number to specify how many loops must go by until you
    /// are called. For example, -1.0 means call me the very next loop.
    /// </para>
    /// <para>
    /// Try to run your flight loop as infrequently as is practical, and suspend it
    /// (using return value 0) when you do not need it; lots of flight loop
    /// callbacks that do nothing lowers X-Plane's frame rate.
    /// </para>
    /// <para>
    /// Your callback will NOT be unregistered if you return 0; it will merely be
    /// inactive.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate float FlightLoopCallback(float inElapsedSinceLastCall, float inElapsedTimeSinceLastFlightLoop, int inCounter, void* inRefcon);
}