using System;

namespace XP.SDK.XPLM.Internal
{
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