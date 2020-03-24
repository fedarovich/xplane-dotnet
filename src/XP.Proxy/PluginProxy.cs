using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text;
using System.Text.Unicode;
using XP.SDK;
using XP.SDK.XPLM.Internal;

namespace XP.Proxy
{
    internal static class PluginProxy
    {
        private static PluginContext _context;
        private static PluginBase _plugin;
        private static bool _resolverInitialized;

        public static int XPluginStart(ref StartParameters parameters)
        {
            GlobalContext.StartupPath = Marshal.PtrToStringUTF8(parameters.StartupPath);

            var pluginPath = Marshal.PtrToStringUTF8(parameters.PluginPath);
            if (string.IsNullOrEmpty(pluginPath))
            {
                UtilitiesAPI.DebugString($"Plugin path is null.");
                return 0;
            }

            var assemblyPath = Path.ChangeExtension(pluginPath, ".dll");
            var currentContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
            if (!_resolverInitialized)
            {
                currentContext.ResolvingUnmanagedDll += ResolveUnmanagedDll;
                _resolverInitialized = true;
            }
           
            _context = new PluginContext(currentContext, assemblyPath);
            try
            {
                var assembly = _context.LoadFromAssemblyPath(assemblyPath);
                var attr = assembly.GetCustomAttribute<PluginAttribute>();
                if (attr == null)
                {
                    UtilitiesAPI.DebugString($"Plugin assembly {assemblyPath} does not have '{typeof(PluginAttribute).FullName}' attribute defined.");
                    return 0;
                }

                _plugin = (PluginBase) Activator.CreateInstance(attr.PluginType);
                WriteUtf8String(_plugin.Name, parameters.Name);
                WriteUtf8String(_plugin.Signature, parameters.Sig);
                WriteUtf8String(_plugin.Description, parameters.Desc);
                GlobalContext.CurrentPlugin = new WeakReference<PluginBase>(_plugin);
                return _plugin.Start() ? 1 : 0;
            }
            catch (Exception ex)
            {
                UtilitiesAPI.DebugString(ex.ToString());
                Unload();
                return 0;
            }

            static unsafe void WriteUtf8String(string str, IntPtr dest, int length = 256)
            {
                Span<byte> buffer = new Span<byte>(dest.ToPointer(), length);
                Utf8.FromUtf16(str, buffer, out _, out var count);
                buffer[count] = 0;
            }
        }

        public static void XPluginStop()
        {
            _plugin.Stop();
            Unload();
        }

        public static int XPluginEnable()
        {
            return _plugin.Enable() ? 1 : 0;
        }

        public static void XPluginDisable()
        {
            _plugin.Disable();
        }

        public static void XPluginReceiveMessage(int pluginId, int message, IntPtr param)
        {
            _plugin.ReceiveMessage(pluginId, message, param);
        }


        private static IntPtr ResolveUnmanagedDll(Assembly assembly, string name)
        {
            return IntPtr.Zero;
        }

        private static void Unload()
        {
            var weakRef = new WeakReference(_context, true);
            _context.Unload();
            _context = null;
            _plugin = null;
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (!weakRef.IsAlive)
                    return;
            }

            if (weakRef.IsAlive)
            {
                UtilitiesAPI.DebugString("Failed to unload plugin assembly.");
            }
        }
    }
}
