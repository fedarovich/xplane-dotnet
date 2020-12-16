namespace XP.SDK.XPLM
{
    public struct CursorRequestEventArgs
    {
        public CursorRequestEventArgs(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public readonly int X;
        public readonly int Y;
        public CursorStatus Cursor;
    }
}
