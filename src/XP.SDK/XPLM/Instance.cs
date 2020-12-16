using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public sealed class Instance : IDisposable
    {
        private readonly int _dataRefCount;
        private InstanceRef _instanceRef;
        private int _disposed;

        public Instance(SceneryObject obj, params string[] dataRefs) : this(obj.Ref, dataRefs)
        {
        }

        public unsafe Instance(ObjectRef objectRef, params string[] dataRefs)
        {
            if (dataRefs == null)
                throw new ArgumentNullException(nameof(dataRefs));

            _dataRefCount = dataRefs.Length;

            var refs = stackalloc byte*[dataRefs.Length + 1];
            for (int i = 0; i < dataRefs.Length; i++)
            {
                refs[i] = (byte*) Marshal.StringToCoTaskMemUTF8(dataRefs[i]);
            }

            refs[dataRefs.Length] = null;

            _instanceRef = InstanceAPI.CreateInstance(objectRef, refs);

            for (int i = dataRefs.Length - 1; i >= 0; i--)
            {
                Marshal.FreeCoTaskMem((IntPtr) refs[i]);
            }
        }

        public unsafe void SetPosition(ref DrawInfo newPosition, in ReadOnlySpan<float> data)
        {
            if (data.Length != _dataRefCount)
                throw new ArgumentException($"Invalid length of data: {_dataRefCount} items were expected.", nameof(data));

            fixed (DrawInfo* pPos = &newPosition)
            {
                fixed (float* pData = data)
                {
                    InstanceAPI.InstanceSetPosition(_instanceRef, pPos, pData);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public unsafe void SetPosition(float x, float y, float z, float pitch, float heading, float roll, in ReadOnlySpan<float> data)
        {
            if (data.Length != _dataRefCount)
                throw new ArgumentException($"Invalid length of data: {_dataRefCount} items were expected.", nameof(data));

            var newPositions = new DrawInfo
            {
                structSize = sizeof(DrawInfo),
                x = x,
                y = y,
                z = z,
                pitch = pitch,
                heading = heading,
                roll = roll,
            };
            SetPosition(ref newPositions, data);
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                InstanceAPI.DestroyInstance(_instanceRef);
                _instanceRef = default;
            }
        }
    }
}
