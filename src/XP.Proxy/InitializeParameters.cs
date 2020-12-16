using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Proxy
{
    internal unsafe struct InitializeParameters
    {
        public readonly delegate* unmanaged[Cdecl]<byte*, void> DebugString;

        public readonly nint StartupPath;

        public delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int> XPluginStart;

        public delegate* unmanaged[Cdecl]<int> XPluginEnable;
        
        public delegate* unmanaged[Cdecl]<int, int, nint, void> XPluginReceiveMessage;
        
        public delegate* unmanaged[Cdecl]<void> XPluginDisable;
        
        public delegate* unmanaged[Cdecl]<void> XPluginStop;
    }
}
