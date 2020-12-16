using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text.Unicode;
using XP.SDK;
using XP.SDK.XPLM;

namespace XP.Proxy
{
    internal static unsafe class PluginProxy
    {
        private static PluginContext _context;
        private static PluginBase _plugin;
        private static delegate* unmanaged[Cdecl]<byte*, void> _debugString;

        [SkipLocalsInit]
        private static void Log(in ReadOnlySpan<char> str)
        {
            Span<byte> buffer = stackalloc byte[(str.Length << 1) + 3];
            Utf8.FromUtf16(str, buffer, out _, out var strLen);
            Utf8.FromUtf16(Environment.NewLine, buffer[strLen..], out _, out var newLineLen);
            buffer[strLen + newLineLen] = 0;
            fixed (byte* pBuffer = buffer)
            {
                _debugString(pBuffer);
            }
        }

        public static int Initialize(ref InitializeParameters parameters)
        {
            try
            {
                _debugString = parameters.DebugString;
                var startupPath = Marshal.PtrToStringUTF8(parameters.StartupPath);

                NativeLibrary.SetDllImportResolver(typeof(PluginAttribute).Assembly, (name, _, _) =>
                {
                    return name switch
                    {
                        SDK.XPLM.Internal.Lib.Name => NativeLibrary.TryLoad(Path.Combine(startupPath!, XPLMPath), out var handle) ? handle : default,
                        SDK.Widgets.Internal.Lib.Name => NativeLibrary.TryLoad(Path.Combine(startupPath!, XPWidgetsPath), out var handle) ? handle : default,
                        _ => default
                    };
                });

                parameters.XPluginStart = &XPluginStart;
                parameters.XPluginEnable = &XPluginEnable;
                parameters.XPluginReceiveMessage = &XPluginReceiveMessage;
                parameters.XPluginDisable = &XPluginDisable;
                parameters.XPluginStop = &XPluginStop;
                
                return 1;
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                return 0;
            }
        }

        [UnmanagedCallersOnly(CallConvs = new [] {typeof(CallConvCdecl)})]
        public static int XPluginStart(byte* outName, byte* outSig, byte* outDesc)
        {
            var pluginPath = PluginInfo.ThisPlugin.FilePath;
            if (string.IsNullOrEmpty(pluginPath))
            {
                Log("Plugin path is null.");
                return 0;
            }

            var assemblyPath = Path.ChangeExtension(pluginPath, ".dll");
            var currentContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
           
            _context = new PluginContext(currentContext, assemblyPath);
            try
            {
                var assembly = _context.LoadFromAssemblyPath(assemblyPath);
                var attr = assembly.GetCustomAttribute<PluginAttribute>();
                if (attr == null)
                {
                    Log($"Plugin assembly {assemblyPath} does not have '{typeof(PluginAttribute).FullName}' attribute defined.");
                    return 0;
                }

                _plugin = (PluginBase) Activator.CreateInstance(attr.PluginType);
                WriteUtf8String(_plugin.Name, outName);
                WriteUtf8String(_plugin.Signature, outSig);
                WriteUtf8String(_plugin.Description, outDesc);
                return _plugin.Start() ? 1 : 0;
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                Unload();
                return 0;
            }

            static void WriteUtf8String(string str, byte* dest, int length = 256)
            {
                Span<byte> buffer = new Span<byte>(dest, length);
                Utf8.FromUtf16(str, buffer, out _, out var count);
                buffer[count] = 0;
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void XPluginStop()
        {
            _plugin.Stop();
            Unload();
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int XPluginEnable()
        {
            return _plugin.Enable() ? 1 : 0;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void XPluginDisable()
        {
            _plugin.Disable();
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void XPluginReceiveMessage(int pluginId, int message, nint param)
        {
            _plugin.ReceiveMessage(pluginId, message, param);
        }

        private static void Unload()
        {
            if (_context.IsCollectible)
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
                    {
                        Log("Unloaded the plugin assembly.");
                        return;
                    }
                }

                if (weakRef.IsAlive)
                {
                    Log("Failed to unload the plugin assembly.");
                }
            }
            else
            {
                _context = null;
                _plugin = null;
                Log("The plugin assembly context is not collectible. The plugin assembly will not be unloaded.");
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static string XPLMPath
        {
            get
            {
                if (OperatingSystem.IsWindows())
                    return Path.Combine("Resources", "plugins", "XPLM_64.dll");

                if (OperatingSystem.IsLinux())
                    return Path.Combine("Resources", "plugins", "XPLM_64.so");

                if (OperatingSystem.IsMacOS())
                    return Path.Combine("Resources", "plugins", "XPLM.framework", "XPLM");
                
                throw new PlatformNotSupportedException();
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static string XPWidgetsPath
        {
            get
            {
                if (OperatingSystem.IsWindows())
                    return Path.Combine("Resources", "plugins", "XPWidgets_64.dll");

                if (OperatingSystem.IsLinux())
                    return Path.Combine("Resources", "plugins", "XPWidgets_64.so");

                if (OperatingSystem.IsMacOS())
                    return Path.Combine("Resources", "plugins", "XPWidgets.framework", "XPWidgets");

                throw new PlatformNotSupportedException();
            }
        }
    }
}
