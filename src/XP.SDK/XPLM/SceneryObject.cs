using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public sealed class SceneryObject : IDisposable
    {
        private ObjectRef _objectRef;
        private int _disposed;

        private SceneryObject(ObjectRef objectRef)
        {
            _objectRef = objectRef;
        }

        public ObjectRef Ref => _objectRef;

        public SceneryObject Load(in Utf8String path)
        {
            var objectRef = SceneryAPI.LoadObject(path);
            return objectRef != default ? new SceneryObject(objectRef) : null;
        }

        public SceneryObject Load(in ReadOnlySpan<char> path)
        {
            var objectRef = SceneryAPI.LoadObject(path);
            return objectRef != default ? new SceneryObject(objectRef) : null;
        }

        public unsafe Task<SceneryObject> LoadAsync(in Utf8String path)
        {
            var tcs = new TaskCompletionSource<SceneryObject>();
            var handle = GCHandle.Alloc(tcs);
            SceneryAPI.LoadObjectAsync(path, &OnObjectLoaded, GCHandle.ToIntPtr(handle).ToPointer());
            return tcs.Task;
        }

        public unsafe Task<SceneryObject> LoadAsync(in ReadOnlySpan<char> path)
        {
            var tcs = new TaskCompletionSource<SceneryObject>();
            var handle = GCHandle.Alloc(tcs);
            SceneryAPI.LoadObjectAsync(path, &OnObjectLoaded, GCHandle.ToIntPtr(handle).ToPointer());
            return tcs.Task;
        }

        [UnmanagedCallersOnly]
        private static unsafe void OnObjectLoaded(ObjectRef objectRef, void* inrefcon)
        {
            var handle = GCHandle.FromIntPtr(new IntPtr(inrefcon));
            var tcs = (TaskCompletionSource<SceneryObject>)handle.Target;
            tcs.TrySetResult(objectRef != default ? new SceneryObject(objectRef) : null);
            handle.Free();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                SceneryAPI.UnloadObject(_objectRef);
                _objectRef = default;
            }
        }

        public static unsafe IReadOnlyList<string> LookupObjects(in Utf8String path, float latitude, float longitude)
        {
            var list = new List<string>();
            var handle = GCHandle.Alloc(list);
            try
            {
                SceneryAPI.LookupObjects(path, latitude, longitude, &LookupObjectsCallback, GCHandle.ToIntPtr(handle).ToPointer());
                return list;
            }
            finally
            {
                handle.Free();
            }
        }

        public static unsafe IReadOnlyList<string> LookupObjects(in ReadOnlySpan<char> path, float latitude, float longitude)
        {
            var list = new List<string>();
            var handle = GCHandle.Alloc(list);
            try
            {
                SceneryAPI.LookupObjects(path, latitude, longitude, &LookupObjectsCallback, GCHandle.ToIntPtr(handle).ToPointer());
                return list;
            }
            finally
            {
                handle.Free();
            }
        }

        [UnmanagedCallersOnly]
        private static unsafe void LookupObjectsCallback(byte* filePath, void* inref)
        {
            var list = (List<string>) GCHandle.FromIntPtr(new IntPtr(inref)).Target;
            list.Add(Marshal.PtrToStringUTF8(new IntPtr(filePath)));
        }
    }
}
