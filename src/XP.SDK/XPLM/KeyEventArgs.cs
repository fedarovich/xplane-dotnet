namespace XP.SDK.XPLM
{
    /// <summary>
    /// Encapsulates the arguments for the <see cref="Window.KeyEvent"/> event.
    /// </summary>
    public readonly struct KeyEventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="KeyEventArgs"/>.
        /// </summary>
        /// <param name="flags">The key flags.</param>
        /// <param name="key">The key.</param>
        /// <param name="virtualKey">The virtual key.</param>
        /// <param name="losingFocus">If <see langword="true"/>, you are losing the keyboard focus; otherwise a key was pressed and <paramref name="key"/> contains its character.</param>
        public KeyEventArgs(KeyFlags flags, byte key, byte virtualKey, bool losingFocus)
        {
            Flags = flags;
            Key = key;
            VirtualKey = virtualKey;
            LosingFocus = losingFocus;
        }

        /// <summary>
        /// Get the key flags.
        /// </summary>
        public readonly KeyFlags Flags;
        
        /// <summary>
        /// Gets the key.
        /// </summary>
        public readonly byte Key;
        
        /// <summary>
        /// Get the virtual key.
        /// </summary>
        public readonly byte VirtualKey;

        /// <summary>
        /// If <see langword="true"/>, you are losing the keyboard focus; otherwise a key was pressed and <see cref="Key"/> contains its character.
        /// </summary>
        public readonly bool LosingFocus;
    }
}
