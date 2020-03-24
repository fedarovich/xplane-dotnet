using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public sealed class SceneryObject : IDisposable
    {
        private static ObjectLoadedCallback _objectLoadedCallback;
        
        private ObjectRef _objectRef;
        private int _disposed;

        static unsafe SceneryObject()
        {
            _objectLoadedCallback = OnObjectLoaded;

            static void OnObjectLoaded(ObjectRef objectRef, void* inrefcon)
            {
                var handle = GCHandle.FromIntPtr(new IntPtr(inrefcon));
                var tcs = (TaskCompletionSource<SceneryObject>)handle.Target;
                tcs.TrySetResult(objectRef != default ? new SceneryObject(objectRef) : null);
                handle.Free();
            }
        }

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
            SceneryAPI.LoadObjectAsync(path, _objectLoadedCallback, GCHandle.ToIntPtr(handle).ToPointer());
            return tcs.Task;
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
            LibraryEnumeratorCallback callback = Callback;
            SceneryAPI.LookupObjects(path, latitude, longitude, callback, GCHandle.ToIntPtr(handle).ToPointer());
            handle.Free();
            return list;

            static void Callback(byte* filePath, void* inref)
            {
                var list = (List<string>) GCHandle.FromIntPtr(new IntPtr(inref)).Target;
                list.Add(Marshal.PtrToStringUTF8(new IntPtr(filePath)));
            }
        }
    }
}
