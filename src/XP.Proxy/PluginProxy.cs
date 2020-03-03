using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using XP.SDK;
using XP.SDK.XPLM.Internal;

namespace XP.Proxy
{
    public static class PluginProxy
    {
        private static PluginContext _context;
        private static PluginBase _plugin;
        private static DebugStringDelegate DebugString;

        public static int XPluginStart(ref StartParameters parameters)
        {
            var debugString = parameters.DebugString;
            if (debugString == null)
                return 0;

            DebugString = debugString;

            var pluginPath = Marshal.PtrToStringUTF8(parameters.GetPluginPath?.Invoke() ?? default);
            if (string.IsNullOrEmpty(pluginPath))
            {
                DebugString($"Plugin path is null.");
                return 0;
            }

            var assemblyPath = Path.ChangeExtension(pluginPath, ".dll");
            var context = new PluginContext(assemblyPath);
            try
            {
                var assembly = context.LoadFromAssemblyPath(pluginPath);
                var attr = assembly.GetCustomAttribute<PluginAttribute>();
                if (attr == null)
                {
                    DebugString($"Plugin assembly {assemblyPath} does not have '{typeof(PluginAttribute).FullName}' attribute defined.");
                    return 0;
                }

                _plugin = (PluginBase) Activator.CreateInstance(attr.PluginType);
                WriteUtf8String(_plugin.Name, parameters.Name);
                WriteUtf8String(_plugin.Signature, parameters.Sig);
                WriteUtf8String(_plugin.Description, parameters.Desc);
                return _plugin.Start() ? 1 : 0;
            }
            catch (Exception ex)
            {
                DebugString(ex.ToString());
                return 0;
            }

            static unsafe void WriteUtf8String(string str, IntPtr dest, int length = 256)
            {
                Span<byte> buffer = new Span<byte>(dest.ToPointer(), length);
                Utf8.FromUtf16(str, buffer, out _, out var count);
                buffer[count] = 0;
            }
        }

        public static void XPPluginStop()
        {
            _plugin.Stop();
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
                DebugString("Failed to unload plugin assembly.");
            }
        }

        public static int XPPluginEnable()
        {
            return _plugin.Enable() ? 1 : 0;
        }

        public static void XPPluginDisable()
        {
            _plugin.Disable();
        }

        public static void XPluginReceiveMessage(int pluginId, int message, IntPtr param)
        {
            _plugin.ReceiveMessage(pluginId, message, param);
        }
    }
}
