using System;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// XPLMProbeInfo_t contains the results of a probe call. Make sure to set
    /// structSize to the size of the struct before using it.
    /// </para>
    /// </summary>
    public partial struct ProbeInfo
    {
        public int structSize;
        public float locationX;
        public float locationY;
        public float locationZ;
        public float normalX;
        public float normalY;
        public float normalZ;
        public float velocityX;
        public float velocityY;
        public float velocityZ;
        public int is_wet;
    }
}