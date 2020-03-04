using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace XP.Proxy
{
    public struct StartParameters
    {
        public IntPtr Name;
        public IntPtr Sig;
        public IntPtr Desc;
        public IntPtr StartupPath;
        public IntPtr PluginPath;
    }
}
