using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using OpenToolkit;

namespace XP.SDK.OpenToolkit.Graphics
{
    /// <summary>
    /// <see cref="IBindingsContext"/> implementation that can be used to bound OpenGL functions to the OpenToolkit.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class OpenGLBindingsContext : IBindingsContext, IDisposable
    {
        private IntPtr _opengl;

        /// <summary>
        /// Creates a new instance of OpenGLBindingsContext.
        /// </summary>
        public OpenGLBindingsContext()
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

        /// <inheritdoc />
        public void Dispose()
        {
            var opengl = _opengl;
            if (opengl != IntPtr.Zero)
            {
                opengl = Interlocked.CompareExchange(ref _opengl, new IntPtr(0), opengl);
                if (opengl != IntPtr.Zero)
                {
                    NativeLibrary.Free(opengl);
                }
            }
        }
    }
}
