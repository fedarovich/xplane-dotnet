namespace XP.SDK.XPLM
{
    /// <summary>
    /// Encapsulates the arguments for the <see cref="Window.CursorRequested"/> event.
    /// </summary>
    public struct CursorRequestEventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CursorRequestEventArgs"/>.
        /// </summary>
        /// <param name="x">Cursor's X coordinate.</param>
        /// <param name="y">Cursor's Y coordinate.</param>
        public CursorRequestEventArgs(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Cursor's X coordinate.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Cursor's Y coordinate.
        /// </summary>
        public readonly int Y;

        /// <summary>
        /// Gets or sets the current cursor.
        /// </summary>
        public CursorStatus Cursor;
    }
}
