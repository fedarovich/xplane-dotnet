using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace XP.SDK
{
    public static class FunctionResolver
    {
        private static IntPtr _xplm;
        private static IntPtr _widgets;

        public static void Initialize(IntPtr xplm, IntPtr widgets)
        {
            _xplm = xplm;
            _widgets = widgets;
        }

        public static IntPtr Resolve(string libraryName, string functionName)
        {
            var lib = libraryName switch
            {
                "XPLM" => _xplm,
                "Widgets" => _widgets,
                _ => IntPtr.Zero
            };
            NativeLibrary.TryGetExport(lib, functionName, out var addr);
            return addr;
        }
    }
}
