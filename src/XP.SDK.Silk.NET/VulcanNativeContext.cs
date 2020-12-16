using System;
using System.Runtime.InteropServices;
using Silk.NET.Core.Contexts;

namespace XP.SDK.Silk.NET
{
    /// <summary>
    /// Provides Vulcan native library bindings.
    /// </summary>
    public class VulcanNativeContext : INativeContext
    {
        private readonly IntPtr _vulcan;

        /// <summary>
        /// Initializes a new instance of <see cref="VulcanNativeContext"/>.
        /// </summary>
        public VulcanNativeContext()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _vulcan = NativeLibrary.Load("vulkan-1.dll");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _vulcan = NativeLibrary.Load("libvulkan.so.1");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                _vulcan = NativeLibrary.Load("libMoltenVK.dylib");
            else
                throw new PlatformNotSupportedException();
        }

        /// <inheritdoc />
        public IntPtr GetProcAddress(string procName)
        {
            NativeLibrary.TryGetExport(_vulcan, procName, out var address);
            return address;
        }
    }
}
