using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Interop;

#nullable enable

namespace XP.SDK.XPLM
{
    public sealed class SharedData : IDisposable
    {
        private readonly bool _registerCallback;

        private GCHandle _handle;
        private int _disposed;

        private SharedData(string name, DataTypeID dataTypes, bool registerCallback)
        {
            DataTypes = dataTypes;
            _registerCallback = registerCallback;
            Name = name;
        }

        public static unsafe SharedData? GetOrCreate(string name, DataTypeID dataTypes, bool registerCallback = true)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be an empty string.", nameof(name));

            var sharedData = new SharedData(name, dataTypes, registerCallback);
            var handle = GCHandle.Alloc(sharedData);
            int result = DataAccessAPI.ShareData(name, dataTypes, 
                registerCallback ? &OnDataChanged : null, 
                GCHandle.ToIntPtr(handle).ToPointer());
            if (result != 0)
            {
                sharedData._handle = handle;
                return sharedData;
            }

            handle.Free();
            return null;
        }

        public string Name { get; }

        public DataTypeID DataTypes { get; }

        public event TypedEventHandler<SharedData>? Changed;

        public unsafe void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                DataAccessAPI.UnshareData(Name, DataTypes,
                    _registerCallback ? &OnDataChanged : null,
                    GCHandle.ToIntPtr(_handle).ToPointer());
                _handle.Free();
                Changed = null;
            }
        }

        [UnmanagedCallersOnly]
        static unsafe void OnDataChanged(void* inrefcon)
        {
            var sharedData = Utils.TryGetObject<SharedData>(inrefcon);
            if (sharedData != null)
            {
                sharedData.Changed?.Invoke(sharedData, EventArgs.Empty);
            }
        }
    }
}
