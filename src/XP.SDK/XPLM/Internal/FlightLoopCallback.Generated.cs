using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// This is your flight loop callback. Each time the flight loop is iterated
    /// through, you receive this call at the end. You receive a time since you
    /// were last called and a time since the last loop, as well as a loop counter.
    /// The 'phase' parameter is deprecated and should be ignored.
    /// </para>
    /// <para>
    /// Your return value controls when you will next be called. Return 0 to stop
    /// receiving callbacks. Pass a positive number to specify how many seconds
    /// until the next callback. (You will be called at or after this time, not
    /// before.) Pass a negative number to specify how many loops must go by until
    /// you are called. For example, -1.0 means call me the very next loop. Try to
    /// run your flight loop as infrequently as is practical, and suspend it (using
    /// return value 0) when you do not need it; lots of flight loop callbacks that
    /// do nothing lowers X-Plane's frame rate.
    /// </para>
    /// <para>
    /// Your callback will NOT be unregistered if you return 0; it will merely be
    /// inactive.
    /// </para>
    /// <para>
    /// The reference constant you passed to your loop is passed back to you.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate float FlightLoopCallback(float inElapsedSinceLastCall, float inElapsedTimeSinceLastFlightLoop, int inCounter, void* inRefcon);
}