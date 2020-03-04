using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

#nullable enable

namespace XP.Proxy
{
    public class PluginContext : AssemblyLoadContext
    {
        private readonly AssemblyLoadContext _parentContext;
        private readonly AssemblyDependencyResolver _resolver;

        public PluginContext(AssemblyLoadContext parentContext, string path) : base(nameof(PluginContext), true)
        {
            _parentContext = parentContext;
            _resolver = new AssemblyDependencyResolver(path);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var path = _resolver.ResolveAssemblyToPath(assemblyName);
            if (path != null)
            {
                return LoadFromAssemblyPath(path);
            }

            return _parentContext?.Assemblies.FirstOrDefault(x => x.FullName == assemblyName.FullName);
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
