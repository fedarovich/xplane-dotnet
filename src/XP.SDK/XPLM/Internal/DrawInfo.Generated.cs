using System;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// The XPLMDrawInfo_t structure contains positioning info for one object that
    /// is to be drawn. Be sure to set structSize to the size of the structure for
    /// future expansion.
    /// </para>
    /// </summary>
    public partial struct DrawInfo
    {
        public int structSize;
        public float x;
        public float y;
        public float z;
        public float pitch;
        public float heading;
        public float roll;
    }
}