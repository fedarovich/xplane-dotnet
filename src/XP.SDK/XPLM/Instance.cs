#nullable enable
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// <para>
    /// Provides instanced drawing of X-Plane objects (.obj files).
    /// In contrast to old drawing APIs, which required you to draw your own objects per-frame,
    /// the instancing API allows you to simply register an OBJ for drawing, then move or manipulate it later (as needed).
    /// </para>
    /// <para>
    /// This provides one tremendous benefit: it keeps all dataref operations for your object in one place.
    /// Because datarefs are main thread only, allowing dataref access anywhere is a serious performance bottleneck
    /// for the simulator—the whole simulator has to pause and wait for each dataref access.
    /// This performance penalty will only grow worse as X-Plane moves toward an ever more heavily multithreaded engine.
    /// </para>
    /// <para>
    /// The instancing API allows X-Plane to isolate all dataref manipulations for all plugin object drawing to one place, potentially providing huge performance gains.
    /// </para>
    /// <para>
    /// Here’s how it works:
    /// </para>
    /// <para>
    /// When an instance is created, it provides a list of all datarefs you want to manipulate in for the OBJ in the future.
    /// This list of datarefs replaces the ad-hoc collections of dataref objects previously used by art assets. Then, per-frame,
    /// you can manipulate the instance by passing in a “block” of packed floats representing the current values of the datarefs for your instance.
    /// (Note that the ordering of this set of packed floats must exactly match the ordering of the datarefs when you created your instance.)
    /// </para>
    /// </summary>
    public sealed class Instance : IDisposable
    {
        private readonly int _dataRefCount;
        private InstanceRef _instanceRef;
        private int _disposed;

        /// <summary>
        /// Creates a new instance, managed by your plug-in.
        /// </summary>
        /// <param name="obj"> The scenery object.</param>
        /// <param name="dataRefs">The array of custom datarefs.</param>
        /// <remarks>
        /// <para>The object passed in must be fully loaded and returned from the XPLM before you can create your instance; you cannot pass a null obj ref, nor can you change the ref later.</para>
        /// <para>If you use any custom datarefs in your object, they must be registered before the object is loaded. This is true even if their data will be provided via the instance dataref list.</para>
        /// </remarks>
        public Instance(SceneryObject obj, params string[] dataRefs) : this(obj.Ref, dataRefs)
        {
        }

        /// <summary>
        /// Creates a new instance, managed by your plug-in.
        /// </summary>
        /// <param name="objectRef">The object reference.</param>
        /// <param name="dataRefs"></param>
        /// <remarks>
        /// <para>The object passed in must be fully loaded and returned from the XPLM before you can create your instance; you cannot pass a null obj ref, nor can you change the ref later.</para>
        /// <para>If you use any custom datarefs in your object, they must be registered before the object is loaded. This is true even if their data will be provided via the instance dataref list.</para>
        /// </remarks>
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

        /// <summary>
        /// <para>
        /// Updates both the position of the instance and all datarefs you registered
        /// for it.  Call this from a flight loop callback or UI callback.
        /// </para>
        /// <para>
        /// DO NOT call this method from a drawing callback; the whole
        /// point of instancing is that you do not need any drawing callbacks. Setting
        /// instance data from a drawing callback may have undefined consequences, and
        /// the drawing callback hurts FPS unnecessarily.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// The memory pointed to by the data pointer must be large enough to hold one
        /// float for every data ref you have registered, and must contain valid
        /// floating point data.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">The length of <paramref name="data"/> is not equal to the number of datarefs used to initialize this instance.</exception>
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

        /// <summary>
        /// <para>
        /// Updates both the position of the instance and all datarefs you registered
        /// for it.  Call this from a flight loop callback or UI callback.
        /// </para>
        /// <para>
        /// DO NOT call this method from a drawing callback; the whole
        /// point of instancing is that you do not need any drawing callbacks. Setting
        /// instance data from a drawing callback may have undefined consequences, and
        /// the drawing callback hurts FPS unnecessarily.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// The memory pointed to by the data pointer must be large enough to hold one
        /// float for every data ref you have registered, and must contain valid
        /// floating point data.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">The length of <paramref name="data"/> is not equal to the number of datarefs used to initialize this instance.</exception>
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

        /// <inheritdoc />
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
