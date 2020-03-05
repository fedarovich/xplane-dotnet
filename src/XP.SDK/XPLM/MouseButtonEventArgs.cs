using System.Runtime.CompilerServices;

namespace XP.SDK.XPLM
{
    public struct MouseButtonEventArgs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MouseButtonEventArgs(int x, int y, MouseStatus mouseStatus) : this()
        {
            X = x;
            Y = y;
            MouseStatus = mouseStatus;
        }

        public readonly int X;
        public readonly int Y;
        public readonly MouseStatus MouseStatus;
        public bool PassThrough;
    }
}