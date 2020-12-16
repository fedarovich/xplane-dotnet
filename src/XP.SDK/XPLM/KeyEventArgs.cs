namespace XP.SDK.XPLM
{
    public readonly struct KeyEventArgs
    {
        public KeyEventArgs(KeyFlags flags, byte key, byte virtualKey, bool losingFocus)
        {
            Flags = flags;
            Key = key;
            VirtualKey = virtualKey;
            LosingFocus = losingFocus;
        }

        public readonly KeyFlags Flags;
        public readonly byte Key;
        public readonly byte VirtualKey;
        public readonly bool LosingFocus;
    }
}
