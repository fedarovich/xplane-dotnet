using System;
using System.IO;
using System.Runtime.InteropServices;

namespace XP.SDK.Widgets.Internal
{
    public static class Lib
    {
        private static readonly IntPtr _handle;

        static Lib()
        {
            string libraryName;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                libraryName = "XPWidgets_64.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                libraryName = "XPWidgets_64.so";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                libraryName = Path.Combine("XPWidgets.framework", "XPWidgets");
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            _handle = NativeLibrary.Load(Path.Combine(GlobalContext.StartupPath, "Resources", "plugins", libraryName));
        }

        public static IntPtr GetExport(string name)
        {
            NativeLibrary.TryGetExport(_handle, name, out var result);
            return result;
        }
    }
}
