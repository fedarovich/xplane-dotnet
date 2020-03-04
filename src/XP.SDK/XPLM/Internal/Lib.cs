using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace XP.SDK.XPLM.Internal
{
    public static class Lib
    {
        private static readonly IntPtr _handle;

        static Lib()
        {
            string libraryName;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                libraryName = "XPLM_64.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                libraryName = "XPLM_64.so";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                libraryName = Path.Combine("XPLM.framework", "XPLM");
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
