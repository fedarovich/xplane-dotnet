using System;

namespace XP.SDK.XPLM.Internal
{
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