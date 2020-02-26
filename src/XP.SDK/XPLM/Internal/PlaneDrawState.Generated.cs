using System;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// This structure contains additional plane parameter info to be passed to
    /// draw plane.  Make sure to fill in the size of the structure field with
    /// sizeof(XPLMDrawPlaneState_t) so that the XPLM can tell how many fields you
    /// knew about when compiling your plugin (since more fields may be added
    /// later).
    /// </para>
    /// <para>
    /// Most of these fields are ratios from 0 to 1 for control input.  X-Plane
    /// calculates what the actual controls look like based on the .acf file for
    /// that airplane.  Note for the yoke inputs, this is what the pilot of the
    /// plane has commanded (post artificial stability system if there were one)
    /// and affects aelerons, rudder, etc.  It is not  necessarily related to the
    /// actual position of the plane!
    /// </para>
    /// </summary>
    public partial struct PlaneDrawState
    {
        public int structSize;
        public float gearPosition;
        public float flapRatio;
        public float spoilerRatio;
        public float speedBrakeRatio;
        public float slatRatio;
        public float wingSweep;
        public float thrust;
        public float yokePitch;
        public float yokeHeading;
        public float yokeRoll;
    }
}