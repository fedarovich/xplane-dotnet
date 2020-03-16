using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public partial struct DataRef
    {
        #region Data Accessors

        public DataTypeID Types
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.GetDataRefTypes(this);
        }

        public bool CanWrite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.CanWriteDataRef(this) != 0;
        }

        public int Int32Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.GetDatai(this);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => DataAccessAPI.SetDatai(this, value);
        }

        public float SingleValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.GetDataf(this);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => DataAccessAPI.SetDataf(this, value);
        }

        public double DoubleValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.GetDatad(this);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => DataAccessAPI.SetDatad(this, value);
        }

        public unsafe int ReadValues(in Span<int> buffer, int offset)
        {
            fixed (int* pData = buffer)
            {
                return DataAccessAPI.GetDatavi(this, pData, offset, buffer.Length);
            }
        }

        public unsafe int ReadValues(in Span<float> buffer, int offset)
        {
            fixed (float* pData = buffer)
            {
                return DataAccessAPI.GetDatavf(this, pData, offset, buffer.Length);
            }
        }

        public unsafe int ReadValues<T>(in Span<T> buffer, int offset) where T : unmanaged
        {
            fixed (T* pData = buffer)
            {
                return DataAccessAPI.GetDatab(this, pData, offset, buffer.Length * sizeof(T));
            }
        }

        public unsafe void WriteValues(in ReadOnlySpan<int> buffer, int offset)
        {
            fixed (int* pData = buffer)
            {
                DataAccessAPI.SetDatavi(this, pData, offset, buffer.Length);
            }
        }

        public unsafe void WriteValues(in ReadOnlySpan<float> buffer, int offset)
        {
            fixed (float* pData = buffer)
            {
                DataAccessAPI.SetDatavf(this, pData, offset, buffer.Length);
            }
        }

        public unsafe void WriteValues<T>(in ReadOnlySpan<T> buffer, int offset) where T : unmanaged
        {
            fixed (T* pData = buffer)
            {
                DataAccessAPI.SetDatab(this, pData, offset, buffer.Length * sizeof(T));
            }
        }

        #endregion

        public bool CheckIsGood() => DataAccessAPI.IsDataRefGood(this) != 0;

        public static DataRef Find(in ReadOnlySpan<char> name) => DataAccessAPI.FindDataRef(name);
    }
}
