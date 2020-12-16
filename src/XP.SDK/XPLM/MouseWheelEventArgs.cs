using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM
{
    public struct MouseWheelEventArgs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MouseWheelEventArgs(int x, int y, MouseWheel wheel, int clicks) : this()
        {
            X = x;
            Y = y;
            Wheel = wheel;
            Clicks = clicks;
        }

        public readonly int X;
        public readonly int Y;
        public readonly MouseWheel Wheel;
        public readonly int Clicks;
        public bool PassThrough;
    }
}
