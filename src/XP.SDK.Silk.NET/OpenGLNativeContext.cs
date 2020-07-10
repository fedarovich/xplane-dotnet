using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Silk.NET.Core.Contexts;

namespace XP.SDK.Silk.NET
{
    /// <summary>
    /// Provides Open GL native library bindings.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class OpenGLNativeContext : INativeContext
    {
        private readonly IntPtr _opengl;

        /// <summary>
        /// Initializes a new instance of <see cref="OpenGLNativeContext"/>.
        /// </summary>
        public OpenGLNativeContext()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _opengl = NativeLibrary.Load("opengl32.dll");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                _opengl = NativeLibrary.Load("libGL.so.1");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                _opengl = NativeLibrary.Load("/System/Library/Frameworks/OpenGL.framework/OpenGL");
            else
                throw new PlatformNotSupportedException();
        }

        /// <inheritdoc />
        public IntPtr GetProcAddress(string procName)
        {
            NativeLibrary.TryGetExport(_opengl, procName, out var address);
            return address;
        }
    }
}
