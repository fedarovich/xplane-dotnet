using System;
using System.Runtime.InteropServices;

namespace XP.SDK.XPLM.Internal
{
    
    /// <summary>
    /// <para>
    /// You provide this callback when loading an object asynchronously; it will be
    /// called once the object is loaded. Your refcon is passed back. The object
    /// ref passed in is the newly loaded object (ready for use) or NULL if an
    /// error occured.
    /// </para>
    /// <para>
    /// If your plugin is disabled, this callback will be delivered as soon as the
    /// plugin is re-enabled. If your plugin is unloaded before this callback is
    /// ever called, the SDK will release the object handle for you.
    /// </para>
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl, BestFitMapping = false, SetLastError = false)]
    public unsafe delegate void ObjectLoadedCallback(ObjectRef inObject, void* inRefcon);
}