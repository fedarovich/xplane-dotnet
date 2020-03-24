using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK
{
    internal static class GlobalContext
    {
        internal static string StartupPath { get; set; }

        internal static WeakReference<PluginBase> CurrentPlugin { get; set; }
    }
}
