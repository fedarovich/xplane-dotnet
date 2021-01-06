using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public abstract class DataRefSource : IDisposable
    {
        private GCHandle _handle;
        private DataRef _dataRef;
        private int _disposed;

        protected unsafe DataRefSource(string name, DataTypeID dataTypes, bool isWriteable)
        {
            _handle = GCHandle.Alloc(this);
            void* refcon = GCHandle.ToIntPtr(_handle).ToPointer();

            _dataRef = DataAccessAPI.RegisterDataAccessor(
                name,
                dataTypes,
                isWriteable.ToInt(),
                dataTypes.HasFlag(DataTypeID.Int) ? &GetDatai : null,
                dataTypes.HasFlag(DataTypeID.Int) && isWriteable ? &SetDatai : null,
                dataTypes.HasFlag(DataTypeID.Float) ? &GetDataf : null,
                dataTypes.HasFlag(DataTypeID.Float) && isWriteable ? &SetDataf : null,
                dataTypes.HasFlag(DataTypeID.Double) ? &GetDatad : null,
                dataTypes.HasFlag(DataTypeID.Double) && isWriteable ? &SetDatad : null,
                dataTypes.HasFlag(DataTypeID.IntArray) ? &GetDatavi : null,
                dataTypes.HasFlag(DataTypeID.IntArray) && isWriteable ? &SetDatavi : null,
                dataTypes.HasFlag(DataTypeID.FloatArray) ? &GetDatavf : null,
                dataTypes.HasFlag(DataTypeID.FloatArray) && isWriteable ? &SetDatavf : null,
                dataTypes.HasFlag(DataTypeID.Data) ? &GetDatab : null,
                dataTypes.HasFlag(DataTypeID.Data) && isWriteable ? &SetDatab : null,
                refcon,
                isWriteable ? refcon : null);

            [UnmanagedCallersOnly]
            static int GetDatai(void* inrefcon) =>
                Utils.TryGetObject<DataRefSource>(inrefcon)?.Int32Value ?? default;

            [UnmanagedCallersOnly]
            static void SetDatai(void* inrefcon, int invalue)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    obj.Int32Value = invalue;
                }
            }

            [UnmanagedCallersOnly]
            static float GetDataf(void* inrefcon) =>
                Utils.TryGetObject<DataRefSource>(inrefcon)?.SingleValue ?? default;

            [UnmanagedCallersOnly]
            static void SetDataf(void* inrefcon, float invalue)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    obj.SingleValue = invalue;
                }
            }

            [UnmanagedCallersOnly]
            static double GetDatad(void* inrefcon) =>
                Utils.TryGetObject<DataRefSource>(inrefcon)?.DoubleValue ?? default;

            [UnmanagedCallersOnly]
            static void SetDatad(void* inrefcon, double invalue)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    obj.DoubleValue = invalue;
                }
            }

            [UnmanagedCallersOnly]
            static int GetDatavi(void* inrefcon, int* outvalues, int inoffset, int inmax)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    var buffer = new Span<int>(outvalues, inmax);
                    return obj.ReadValues(buffer, inoffset);
                }

                return 0;
            }

            [UnmanagedCallersOnly]
            static void SetDatavi(void* inrefcon, int* invalues, int inoffset, int incount)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    var buffer = new ReadOnlySpan<int>(invalues, incount);
                    obj.WriteValues(buffer, inoffset);
                }
            }

            [UnmanagedCallersOnly]
            static int GetDatavf(void* inrefcon, float* outvalues, int inoffset, int inmax)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    var buffer = new Span<float>(outvalues, inmax);
                    return obj.ReadValues(buffer, inoffset);
                }

                return 0;
            }

            [UnmanagedCallersOnly]
            static void SetDatavf(void* inrefcon, float* invalues, int inoffset, int incount)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    var buffer = new ReadOnlySpan<float>(invalues, incount);
                    obj.WriteValues(buffer, inoffset);
                }
            }

            [UnmanagedCallersOnly]
            static int GetDatab(void* inrefcon, void* outvalues, int inoffset, int inmax)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    var buffer = new Span<byte>(outvalues, inmax);
                    return obj.ReadValues(buffer, inoffset);
                }

                return 0;
            }

            [UnmanagedCallersOnly]
            static void SetDatab(void* inrefcon, void* invalues, int inoffset, int incount)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    var buffer = new ReadOnlySpan<byte>(invalues, incount);
                    obj.WriteValues(buffer, inoffset);
                }
            }
        }

        protected virtual int Int32Value
        {
            get => 0;
            set { }
        }

        protected virtual float SingleValue
        {
            get => 0;
            set { }
        }

        protected virtual double DoubleValue
        {
            get => 0;
            set { }
        }

        protected virtual int ReadValues(in Span<int> buffer, int offset) => 0;

        protected virtual int ReadValues(in Span<float> buffer, int offset) => 0;
        
        protected virtual int ReadValues(in Span<byte> buffer, int offset) => 0;
        
        protected virtual void WriteValues(in ReadOnlySpan<int> buffer, int offset)
        {
        }
        
        protected virtual void WriteValues(in ReadOnlySpan<float> buffer, int offset)
        {
        }

        protected virtual void WriteValues(in ReadOnlySpan<byte> buffer, int offset)
        {
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                try
                {
                    Dispose(true);
                }
                finally
                {
                    DataAccessAPI.UnregisterDataAccessor(_dataRef);
                    _dataRef = default;
                    _handle.Free();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }

    public abstract class DataRefSource<TData> : DataRefSource
        where TData : unmanaged
    {
        protected DataRefSource(string name, DataTypeID dataTypes, bool isWriteable) : base(name, dataTypes, isWriteable)
        {
        }

        protected sealed override int ReadValues(in Span<byte> buffer, int offset)
        {
            return ReadValues(MemoryMarshal.Cast<byte, TData>(buffer), offset);
        }

        protected sealed override void WriteValues(in ReadOnlySpan<byte> buffer, int offset)
        {
            WriteValues(MemoryMarshal.Cast<byte, TData>(buffer), offset);
        }

        protected abstract int ReadValues<T>(in Span<T> buffer, int offset);

        protected abstract void WriteValues<T>(in ReadOnlySpan<T> buffer, int offset);
    }
}
