using System;
using System.Runtime.InteropServices;
using Silk.NET.Core.Loader;

namespace XP.SDK.Silk.NET
{
    /// <summary>
    /// Symbol loader.
    /// </summary>
    public class SymbolLoader : GLSymbolLoader
    {
        /// <inheritdoc />
        protected override IntPtr CoreLoadFunctionPointer(IntPtr handle, string functionName)
        {
            NativeLibrary.TryGetExport(handle, functionName, out var address);
            return address;
        }
    }
}
