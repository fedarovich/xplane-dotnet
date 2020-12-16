using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Silk.NET.Core.Contexts;

namespace XP.SDK.Silk.NET
{
    /// <summary>
    /// Provides Open AL native library bindings.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class OpenALNativeContext : INativeContext
    {
        private readonly IntPtr _openal;

        /// <summary>
        /// Initializes a new instance of <see cref="OpenALNativeContext"/>.
        /// </summary>
        public OpenALNativeContext()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _openal = NativeLibrary.Load("openal32.dll");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _openal = NativeLibrary.Load("libopenal.so.1");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                _openal = NativeLibrary.Load("/System/Library/Frameworks/OpenAL.framework/OpenAL");
            else
                throw new PlatformNotSupportedException();
        }

        /// <inheritdoc />
        public IntPtr GetProcAddress(string procName)
        {
            NativeLibrary.TryGetExport(_openal, procName, out var address);
            return address;
        }
    }
}
