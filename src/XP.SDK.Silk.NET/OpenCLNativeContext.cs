using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Silk.NET.Core.Contexts;

namespace XP.SDK.Silk.NET
{
    /// <summary>
    /// Provides Open CL native library bindings.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class OpenCLNativeContext : INativeContext
    {
        private readonly IntPtr _opencl;

        /// <summary>
        /// Initializes a new instance of <see cref="OpenCLNativeContext"/>.
        /// </summary>
        public OpenCLNativeContext()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _opencl = NativeLibrary.Load("opencl.dll");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _opencl = NativeLibrary.Load("libOpenCL.so");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                _opencl = NativeLibrary.Load("/System/Library/Frameworks/OpenCL.framework/OpenCL");
            else
                throw new PlatformNotSupportedException();
        }

        /// <inheritdoc />
        public IntPtr GetProcAddress(string procName)
        {
            NativeLibrary.TryGetExport(_opencl, procName, out var address);
            return address;
        }
    }
}