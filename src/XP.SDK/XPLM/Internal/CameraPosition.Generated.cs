using System;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// This structure contains a full specification of the camera. X, Y, and Z are
    /// the camera's position in OpenGL coordiantes; pitch, roll, and yaw are
    /// rotations from a camera facing flat north in degrees. Positive pitch means
    /// nose up, positive roll means roll right, and positive yaw means yaw right,
    /// all in degrees. Zoom is a zoom factor, with 1.0 meaning normal zoom and 2.0
    /// magnifying by 2x (objects appear larger).
    /// </para>
    /// </summary>
    public partial struct CameraPosition
    {
        public float x;
        public float y;
        public float z;
        public float pitch;
        public float heading;
        public float roll;
        public float zoom;
    }
}