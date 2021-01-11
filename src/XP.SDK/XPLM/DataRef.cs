using System;
using System.Runtime.CompilerServices;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public partial struct DataRef
    {
        #region Data Accessors

        /// <summary>
        /// Gets the types of the Data Ref for accessor use.
        /// </summary>
        public DataTypeID Types
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.GetDataRefTypes(this);
        }

        /// <summary>
        /// Gets the value indicating whether the Data Ref is writable.
        /// </summary>
        public bool CanWrite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.CanWriteDataRef(this) != 0;
        }

        /// <summary>
        /// Gets the <see cref="Int32"/> value.
        /// </summary>
        public int Int32Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.GetDatai(this);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => DataAccessAPI.SetDatai(this, value);
        }

        /// <summary>
        /// Gets the <see cref="Single"/> value.
        /// </summary>
        public float SingleValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.GetDataf(this);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => DataAccessAPI.SetDataf(this, value);
        }

        /// <summary>
        /// Gets the <see cref="Double"/> value.
        /// </summary>
        public double DoubleValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DataAccessAPI.GetDatad(this);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => DataAccessAPI.SetDatad(this, value);
        }

        /// <summary>
        /// Fills the <paramref name="buffer"/> with <see cref="Int32"/> values
        /// read from the Data Ref supporting <see cref="DataTypeID.IntArray"/> data type
        /// starting from the specified offset.
        /// </summary>
        /// <param name="buffer">The buffer to fill.</param>
        /// <param name="offset">The offset from the start of Data Ref's data.</param>
        /// <returns>The number of actually copied values if the <paramref name="buffer" /> is not empty; the size of the data otherwise.</returns>
        /// <remarks>
        /// <para>This method can be used to read data from Data Ref supporting <see cref="DataTypeID.IntArray"/> type.</para>
        /// <para>Calling this method IS NOT THE SAME as calling <see cref="ReadValues{Int32}"/> as the latter reads the data of <see cref="DataTypeID.Data"/> type.</para>
        /// </remarks>
        public unsafe int ReadValues(in Span<int> buffer, int offset)
        {
            fixed (int* pData = buffer)
            {
                return DataAccessAPI.GetDatavi(this, pData, offset, buffer.Length);
            }
        }

        /// <summary>
        /// Fills the <paramref name="buffer"/> with <see cref="Single"/> values
        /// read from the Data Ref supporting <see cref="DataTypeID.FloatArray"/> data type
        /// starting from the specified offset.
        /// </summary>
        /// <param name="buffer">The buffer to fill.</param>
        /// <param name="offset">The offset from the start of Data Ref's data.</param>
        /// <returns>The number of actually copied values if the <paramref name="buffer" /> is not empty; the size of the data otherwise.</returns>
        /// <remarks>
        /// <para>This method can be used to read data from Data Ref supporting <see cref="DataTypeID.FloatArray"/> type.</para>
        /// <para>Calling this method IS NOT THE SAME as calling <see cref="ReadValues{Single}"/> as the latter reads the data of <see cref="DataTypeID.Data"/> type.</para>
        /// </remarks>
        public unsafe int ReadValues(in Span<float> buffer, int offset)
        {
            fixed (float* pData = buffer)
            {
                return DataAccessAPI.GetDatavf(this, pData, offset, buffer.Length);
            }
        }

        /// <summary>
        /// Fills the <paramref name="buffer"/> with the values of type <typeparamref name="T"/>
        /// read from the Data Ref supporting <see cref="DataTypeID.Data"/> data type
        /// starting from the specified offset.
        /// </summary>
        /// <param name="buffer">The buffer to fill.</param>
        /// <param name="offset">The offset (in Ts) from the start of Data Ref's data. The actual byte offset is given as <c>offset * sizeof(T)</c>.</param>
        /// <returns>The number of actually copied values if the <paramref name="buffer" /> is not empty; the size of the data otherwise.</returns>
        /// <remarks>
        /// <para>This method can be used to read data from Data Ref supporting <see cref="DataTypeID.Data"/> type.</para>
        /// </remarks>
        public unsafe int ReadValues<T>(in Span<T> buffer, int offset) where T : unmanaged
        {
            fixed (T* pData = buffer)
            {
                return DataAccessAPI.GetDatab(this, pData, offset, buffer.Length * sizeof(T)) / sizeof(T);
            }
        }

        /// <summary>
        /// Writes <see cref="Int32"/> values from the <paramref name="buffer"/>
        /// to the Data Ref supporting <see cref="DataTypeID.IntArray"/> type
        /// starting from the specified offset.
        /// </summary>
        /// <param name="buffer">The source buffer.</param>
        /// <param name="offset">The offset from the start of Data Ref's data.</param>
        /// <remarks>
        /// <para>This method can be used to read data from Data Ref supporting <see cref="DataTypeID.IntArray"/> type.</para>
        /// <para>Calling this method IS NOT THE SAME as calling <see cref="WriteValues{Int32}"/> as the latter writes the data of <see cref="DataTypeID.Data"/> type.</para>
        /// </remarks>
        public unsafe void WriteValues(in ReadOnlySpan<int> buffer, int offset)
        {
            fixed (int* pData = buffer)
            {
                DataAccessAPI.SetDatavi(this, pData, offset, buffer.Length);
            }
        }

        /// <summary>
        /// Writes <see cref="Single"/> values from the <paramref name="buffer"/>
        /// to the Data Ref supporting <see cref="DataTypeID.FloatArray"/> type
        /// starting from the specified offset.
        /// </summary>
        /// <param name="buffer">The source buffer.</param>
        /// <param name="offset">The offset from the start of Data Ref's data.</param>
        /// <remarks>
        /// <para>This method can be used to read data from Data Ref supporting <see cref="DataTypeID.FloatArray"/> type.</para>
        /// <para>Calling this method IS NOT THE SAME as calling <see cref="WriteValues{Single}"/> as the latter writes the data of <see cref="DataTypeID.Data"/> type.</para>
        /// </remarks>
        public unsafe void WriteValues(in ReadOnlySpan<float> buffer, int offset)
        {
            fixed (float* pData = buffer)
            {
                DataAccessAPI.SetDatavf(this, pData, offset, buffer.Length);
            }
        }

        /// <summary>
        /// Writes the values of type <typeparamref name="T"/> from the <paramref name="buffer"/>
        /// to the Data Ref supporting <see cref="DataTypeID.Data"/> type
        /// starting from the specified offset.
        /// </summary>
        /// <param name="buffer">The source buffer.</param>
        /// <param name="offset">The offset (in Ts) from the start of Data Ref's data. The actual byte offset is given as <c>offset * sizeof(T)</c>.</param>
        /// <remarks>
        /// <para>This method can be used to read data from Data Ref supporting <see cref="DataTypeID.Data"/> type.</para>
        /// </remarks>
        public unsafe void WriteValues<T>(in ReadOnlySpan<T> buffer, int offset) where T : unmanaged
        {
            fixed (T* pData = buffer)
            {
                DataAccessAPI.SetDatab(this, pData, offset, buffer.Length * sizeof(T));
            }
        }

        #endregion

        /// <summary>
        /// <para>
        /// This function returns <see langword="true"/> if the passed in handle is a valid dataref that
        /// is not orphaned.
        /// </para>
        /// <para>
        /// Note: there is normally no need to call this function; datarefs returned by
        /// <see cref="Find"/> remain valid (but possibly orphaned) unless there is a
        /// complete plugin reload (in which case your plugin is reloaded anyway).
        /// Orphaned datarefs can be safely read and return 0. Therefore you never need
        /// to call this function to 'check' the safety of a dataref.
        /// (it performs some slow checking of the handle validity, so
        /// it has a performance cost.)
        /// </para>
        /// </summary>
        public bool CheckIsGood() => DataAccessAPI.IsDataRefGood(this) != 0;

        /// <summary>
        /// <para>
        /// This routine looks up the actual <see cref="DataRef"/> that you use to read and write the data by the name.
        /// The string names for Data Refs are published on the X-Plane SDK web site.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// NOTE: this function is relatively expensive; save the <see cref="DataRef"/> this
        /// function returns for future use. Do not look up your data ref by string
        /// every time you need to read or write it.
        /// </para>
        /// </remarks>
        public static DataRef Find(in Utf8String name) => DataAccessAPI.FindDataRef(name);

        /// <summary>
        /// <para>
        /// This routine looks up the actual <see cref="DataRef"/> that you use to read and write the data by the name.
        /// The string names for Data Refs are published on the X-Plane SDK web site.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// NOTE: this function is relatively expensive; save the <see cref="DataRef"/> this
        /// function returns for future use. Do not look up your data ref by string
        /// every time you need to read or write it.
        /// </para>
        /// </remarks>
        public static DataRef Find(in ReadOnlySpan<char> name) => DataAccessAPI.FindDataRef(name);
    }
}
