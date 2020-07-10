using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;

#nullable enable

namespace XP.Proxy
{
    internal class PluginContext : AssemblyLoadContext
    {
        private readonly AssemblyLoadContext _parentContext;
        private readonly AssemblyDependencyResolver _resolver;

        public PluginContext(AssemblyLoadContext parentContext, string path) : this(parentContext, path, MustBeCollectible(path))
        {
        }

        protected PluginContext(AssemblyLoadContext parentContext, string path, bool isCollectible) : base(nameof(PluginContext), isCollectible)
        {
            _parentContext = parentContext;
            _resolver = new AssemblyDependencyResolver(path);
        }

        private static bool MustBeCollectible(string path)
        {
            var configPath = Path.ChangeExtension(path, ".runtimeconfig.json");
            if (File.Exists(configPath))
            {
                using var configStream = File.OpenRead(configPath);
                using var config = JsonDocument.Parse(configStream);
                if (config.RootElement.TryGetProperty("runtimeOptions", out var runtimeOptions) &&
                    runtimeOptions.TryGetProperty("configProperties", out var configProperties) &&
                    configProperties.TryGetProperty("XP.Proxy.UseCollectibleContext", out var isCollectible))
                {
                    return isCollectible.GetBoolean();
                }
            }

            // Use a collectible context by default.
            return true;
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
