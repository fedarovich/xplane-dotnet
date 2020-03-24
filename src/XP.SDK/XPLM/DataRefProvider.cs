using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public abstract class DataRefSource : IDisposable
    {
        private static readonly GetDataiCallback _getDatai;
        private static readonly SetDataiCallback _setDatai;
        private static readonly GetDatafCallback _getDataf;
        private static readonly SetDatafCallback _setDataf;
        private static readonly GetDatadCallback _getDatad;
        private static readonly SetDatadCallback _setDatad;
        private static readonly GetDataviCallback _getDatavi;
        private static readonly SetDataviCallback _setDatavi;
        private static readonly GetDatavfCallback _getDatavf;
        private static readonly SetDatavfCallback _setDatavf;
        private static readonly GetDatabCallback _getDatab;
        private static readonly SetDatabCallback _setDatab;

        private GCHandle _handle;
        private DataRef _dataRef;
        private int _disposed;

        static unsafe DataRefSource()
        {
            _getDatai = GetDatai;
            _setDatai = SetDatai;
            _getDataf = GetDataf;
            _setDataf = SetDataf;
            _getDatad = GetDatad;
            _setDatad = SetDatad;
            _getDatavi = GetDatavi;
            _setDatavi = SetDatavi;
            _getDatavf = GetDatavf;
            _setDatavf = SetDatavf;
            _getDatab = GetDatab;
            _setDatab = SetDatab;

            static int GetDatai(void* inrefcon) =>
                Utils.TryGetObject<DataRefSource>(inrefcon)?.Int32Value ?? default;

            static void SetDatai(void* inrefcon, int invalue)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    obj.Int32Value = invalue;
                }
            }

            static float GetDataf(void* inrefcon) =>
                Utils.TryGetObject<DataRefSource>(inrefcon)?.SingleValue ?? default;

            static void SetDataf(void* inrefcon, float invalue)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    obj.SingleValue = invalue;
                }
            }

            static double GetDatad(void* inrefcon) =>
                Utils.TryGetObject<DataRefSource>(inrefcon)?.DoubleValue ?? default;

            static void SetDatad(void* inrefcon, double invalue)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    obj.DoubleValue = invalue;
                }
            }

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

            static void SetDatavi(void* inrefcon, int* invalues, int inoffset, int incount)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    var buffer = new ReadOnlySpan<int>(invalues, incount);
                    obj.WriteValues(buffer, inoffset);
                }
            }

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

            static void SetDatavf(void* inrefcon, float* invalues, int inoffset, int incount)
            {
                var obj = Utils.TryGetObject<DataRefSource>(inrefcon);
                if (obj != null)
                {
                    var buffer = new ReadOnlySpan<float>(invalues, incount);
                    obj.WriteValues(buffer, inoffset);
                }
            }

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

        protected unsafe DataRefSource(string name, DataTypeID dataTypes, bool isWriteable)
        {
            _handle = GCHandle.Alloc(this);
            void* refcon = GCHandle.ToIntPtr(_handle).ToPointer();

            _dataRef = DataAccessAPI.RegisterDataAccessor(
                name,
                dataTypes,
                isWriteable.ToInt(),
                Getter(DataTypeID.Int, _getDatai),
                Setter(DataTypeID.Int, _setDatai),
                Getter(DataTypeID.Float, _getDataf),
                Setter(DataTypeID.Float, _setDataf),
                Getter(DataTypeID.Double, _getDatad),
                Setter(DataTypeID.Double, _setDatad),
                Getter(DataTypeID.IntArray, _getDatavi),
                Setter(DataTypeID.IntArray, _setDatavi),
                Getter(DataTypeID.FloatArray, _getDatavf),
                Setter(DataTypeID.FloatArray, _setDatavf),
                Getter(DataTypeID.Data, _getDatab),
                Setter(DataTypeID.Data, _setDatab),
                refcon,
                isWriteable ? refcon : null);

            T Getter<T>(DataTypeID type, T getter) where T : Delegate => dataTypes.HasFlag(type) ? getter : null;

            T Setter<T>(DataTypeID type, T setter) where T : Delegate => (dataTypes.HasFlag(type) && isWriteable) ? setter : null;
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
