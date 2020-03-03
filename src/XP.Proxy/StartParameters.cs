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
        public IntPtr XplmHandle;
        public IntPtr WidgetsHandle;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string PluginPath;
    }
}
