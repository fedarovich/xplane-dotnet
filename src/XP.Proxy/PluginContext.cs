using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

#nullable enable

namespace XP.Proxy
{
    public class PluginContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginContext(string path) : base(nameof(PluginContext), true)
        {
            _resolver = new AssemblyDependencyResolver(path);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var path = _resolver.ResolveAssemblyToPath(assemblyName);
            if (path != null)
            {
                return LoadFromAssemblyPath(path);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var path = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (path != null)
            {
                return LoadUnmanagedDllFromPath(path);
            }

            return IntPtr.Zero;
        }
    }
}
