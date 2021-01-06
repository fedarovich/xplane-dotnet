namespace XP.Proxy
{
    internal unsafe struct InitializeParameters
    {
#pragma warning disable 649
        public readonly delegate* unmanaged<byte*, void> DebugString;

        public readonly nint StartupPath;
#pragma warning restore 649

        public delegate* unmanaged<byte*, byte*, byte*, int> XPluginStart;

        public delegate* unmanaged<int> XPluginEnable;
        
        public delegate* unmanaged<int, int, nint, void> XPluginReceiveMessage;
        
        public delegate* unmanaged<void> XPluginDisable;
        
        public delegate* unmanaged<void> XPluginStop;
    }
}
