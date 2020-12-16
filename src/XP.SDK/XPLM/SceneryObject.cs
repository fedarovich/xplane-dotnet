using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using XP.SDK.XPLM.Internal;

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

        public SceneryObject Load(in ReadOnlySpan<char> path)
        {
            var objectRef = SceneryAPI.LoadObject(path);
            return objectRef != default ? new SceneryObject(objectRef) : null;
        }

        public unsafe Task<SceneryObject> LoadAsync(in ReadOnlySpan<char> path)
        {
            var tcs = new TaskCompletionSource<SceneryObject>();
            var handle = GCHandle.Alloc(tcs);
            SceneryAPI.LoadObjectAsync(path, &OnObjectLoaded, GCHandle.ToIntPtr(handle).ToPointer());
            return tcs.Task;

            [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
            static void OnObjectLoaded(ObjectRef objectRef, void* inrefcon)
            {
                var handle = GCHandle.FromIntPtr(new IntPtr(inrefcon));
                var tcs = (TaskCompletionSource<SceneryObject>)handle.Target;
                tcs.TrySetResult(objectRef != default ? new SceneryObject(objectRef) : null);
                handle.Free();
            }
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                SceneryAPI.UnloadObject(_objectRef);
                _objectRef = default;
            }
        }

        public static unsafe IReadOnlyList<string> LookupObjects(in ReadOnlySpan<char> path, float latitude, float longitude)
        {
            var list = new List<string>();
            var handle = GCHandle.Alloc(list);
            try
            {
                SceneryAPI.LookupObjects(path, latitude, longitude, &Callback, GCHandle.ToIntPtr(handle).ToPointer());
                return list;
            }
            finally
            {
                handle.Free();
            }

            [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
            static void Callback(byte* filePath, void* inref)
            {
                var list = (List<string>) GCHandle.FromIntPtr(new IntPtr(inref)).Target;
                list.Add(Marshal.PtrToStringUTF8(new IntPtr(filePath)));
            }
        }
    }
}
