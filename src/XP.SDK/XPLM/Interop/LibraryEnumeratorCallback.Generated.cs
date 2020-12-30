using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Interop
{
    
    /// <summary>
    /// <para>
    /// An XPLMLibraryEnumerator_f is a callback you provide that is called once
    /// for each library element that is located. The returned paths will be
    /// relative to the X-System folder.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void LibraryEnumeratorCallback(byte* inFilePath, void* inRef);
}