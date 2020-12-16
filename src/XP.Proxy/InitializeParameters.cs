namespace XP.Proxy
{
    internal unsafe struct InitializeParameters
    {
#pragma warning disable 649
        public readonly delegate* unmanaged[Cdecl]<byte*, void> DebugString;

        public readonly nint StartupPath;
#pragma warning restore 649

        public delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int> XPluginStart;

        public delegate* unmanaged[Cdecl]<int> XPluginEnable;
        
        public delegate* unmanaged[Cdecl]<int, int, nint, void> XPluginReceiveMessage;
        
        public delegate* unmanaged[Cdecl]<void> XPluginDisable;
        
        public delegate* unmanaged[Cdecl]<void> XPluginStop;
    }
}
