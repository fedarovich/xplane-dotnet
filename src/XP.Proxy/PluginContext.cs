using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;

namespace XP.Proxy
{
    public class PluginContext : AssemblyLoadContext
    {
        public PluginContext() : base(nameof(PluginContext), true)
        {
        }
    }
}
