using System.Runtime.InteropServices;

namespace XP.Proxy
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    internal delegate int InitializeDelegate(ref InitializeParameters parameters);
}
