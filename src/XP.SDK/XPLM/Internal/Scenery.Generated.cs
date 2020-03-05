using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Scenery
    {
        private static IntPtr CreateProbePtr;
        private static IntPtr DestroyProbePtr;
        private static IntPtr ProbeTerrainXYZPtr;
        private static IntPtr GetMagneticVariationPtr;
        private static IntPtr DegTrueToDegMagneticPtr;
        private static IntPtr DegMagneticToDegTruePtr;
        private static IntPtr LoadObjectPtr;
        private static IntPtr LoadObjectAsyncPtr;
        private static IntPtr DrawObjectsPtr;
        private static IntPtr UnloadObjectPtr;
        private static IntPtr LookupObjectsPtr;

        static Scenery()
        {
            CreateProbePtr = Lib.GetExport("XPLMCreateProbe");
            DestroyProbePtr = Lib.GetExport("XPLMDestroyProbe");
            ProbeTerrainXYZPtr = Lib.GetExport("XPLMProbeTerrainXYZ");
            GetMagneticVariationPtr = Lib.GetExport("XPLMGetMagneticVariation");
            DegTrueToDegMagneticPtr = Lib.GetExport("XPLMDegTrueToDegMagnetic");
            DegMagneticToDegTruePtr = Lib.GetExport("XPLMDegMagneticToDegTrue");
            LoadObjectPtr = Lib.GetExport("XPLMLoadObject");
            LoadObjectAsyncPtr = Lib.GetExport("XPLMLoadObjectAsync");
            DrawObjectsPtr = Lib.GetExport("XPLMDrawObjects");
            UnloadObjectPtr = Lib.GetExport("XPLMUnloadObject");
            LookupObjectsPtr = Lib.GetExport("XPLMLookupObjects");
        }

        
        /// <summary>
        /// <para>
        /// Creates a new probe object of a given type and returns.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ProbeRef CreateProbe(ProbeType inProbeType)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CreateProbePtr);
            ProbeRef result;
            IL.Push(inProbeType);
            IL.Push(CreateProbePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ProbeRef), typeof(ProbeType)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Deallocates an existing probe object.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void DestroyProbe(ProbeRef inProbe)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DestroyProbePtr);
            IL.Push(inProbe);
            IL.Push(DestroyProbePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ProbeRef)));
        }

        
        /// <summary>
        /// <para>
        /// Probes the terrain. Pass in the XYZ coordinate of the probe point, a probe
        /// object, and an XPLMProbeInfo_t struct that has its structSize member set
        /// properly. Other fields are filled in if we hit terrain, and a probe result
        /// is returned.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe ProbeResult ProbeTerrainXYZ(ProbeRef inProbe, float inX, float inY, float inZ, ProbeInfo* outInfo)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(ProbeTerrainXYZPtr);
            ProbeResult result;
            IL.Push(inProbe);
            IL.Push(inX);
            IL.Push(inY);
            IL.Push(inZ);
            IL.Push(outInfo);
            IL.Push(ProbeTerrainXYZPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ProbeResult), typeof(ProbeRef), typeof(float), typeof(float), typeof(float), typeof(ProbeInfo*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Returns X-Plane's simulated magnetic variation (declination) at the
        /// indication latitude and longitude.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float GetMagneticVariation(double latitude, double longitude)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(GetMagneticVariationPtr);
            float result;
            IL.Push(latitude);
            IL.Push(longitude);
            IL.Push(GetMagneticVariationPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(double), typeof(double)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Converts a heading in degrees relative to true north into a value relative
        /// to magnetic north at the user's current location.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float DegTrueToDegMagnetic(float headingDegreesTrue)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DegTrueToDegMagneticPtr);
            float result;
            IL.Push(headingDegreesTrue);
            IL.Push(DegTrueToDegMagneticPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(float)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Converts a heading in degrees relative to magnetic north at the user's
        /// current location into a value relative to true north.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float DegMagneticToDegTrue(float headingDegreesMagnetic)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DegMagneticToDegTruePtr);
            float result;
            IL.Push(headingDegreesMagnetic);
            IL.Push(DegMagneticToDegTruePtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(float)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine loads an OBJ file and returns a handle to it. If X-Plane has
        /// already loaded the object, the handle to the existing object is returned.
        /// Do not assume you will get the same handle back twice, but do make sure to
        /// call unload once for every load to avoid "leaking" objects. The object will
        /// be purged from memory when no plugins and no scenery are using it.
        /// </para>
        /// <para>
        /// The path for the object must be relative to the X-System base folder. If
        /// the path is in the root of the X-System folder you may need to prepend ./
        /// to it; loading objects in the root of the X-System folder is STRONGLY
        /// discouraged - your plugin should not dump art resources in the root folder!
        /// </para>
        /// <para>
        /// XPLMLoadObject will return NULL if the object cannot be loaded (either
        /// because it is not found or the file is misformatted). This routine will
        /// load any object that can be used in the X-Plane scenery system.
        /// </para>
        /// <para>
        /// It is important that the datarefs an object uses for animation already be
        /// loaded before you load the object. For this reason it may be necessary to
        /// defer object loading until the sim has fully started.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe ObjectRef LoadObject(byte* inPath)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(LoadObjectPtr);
            ObjectRef result;
            IL.Push(inPath);
            IL.Push(LoadObjectPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(ObjectRef), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine loads an OBJ file and returns a handle to it. If X-Plane has
        /// already loaded the object, the handle to the existing object is returned.
        /// Do not assume you will get the same handle back twice, but do make sure to
        /// call unload once for every load to avoid "leaking" objects. The object will
        /// be purged from memory when no plugins and no scenery are using it.
        /// </para>
        /// <para>
        /// The path for the object must be relative to the X-System base folder. If
        /// the path is in the root of the X-System folder you may need to prepend ./
        /// to it; loading objects in the root of the X-System folder is STRONGLY
        /// discouraged - your plugin should not dump art resources in the root folder!
        /// </para>
        /// <para>
        /// XPLMLoadObject will return NULL if the object cannot be loaded (either
        /// because it is not found or the file is misformatted). This routine will
        /// load any object that can be used in the X-Plane scenery system.
        /// </para>
        /// <para>
        /// It is important that the datarefs an object uses for animation already be
        /// loaded before you load the object. For this reason it may be necessary to
        /// defer object loading until the sim has fully started.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe ObjectRef LoadObject(in ReadOnlySpan<char> inPath)
        {
            IL.DeclareLocals(false);
            Span<byte> inPathUtf8 = stackalloc byte[(inPath.Length << 1) | 1];
            var inPathPtr = Utils.ToUtf8Unsafe(inPath, inPathUtf8);
            return LoadObject(inPathPtr);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void LoadObjectAsyncPrivate(byte* inPath, IntPtr inCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(LoadObjectAsyncPtr);
            IL.Push(inPath);
            IL.Push(inCallback);
            IL.Push(inRefcon);
            IL.Push(LoadObjectAsyncPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(byte*), typeof(ObjectLoadedCallback), typeof(void*)));
        }

        
        /// <summary>
        /// <para>
        /// This routine loads an object asynchronously; control is returned to you
        /// immediately while X-Plane loads the object. The sim will not stop flying
        /// while the object loads. For large objects, it may be several seconds before
        /// the load finishes.
        /// </para>
        /// <para>
        /// You provide a callback function that is called once the load has completed.
        /// Note that if the object cannot be loaded, you will not find out until the
        /// callback function is called with a NULL object handle.
        /// </para>
        /// <para>
        /// There is no way to cancel an asynchronous object load; you must wait for
        /// the load to complete and then release the object if it is no longer
        /// desired.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void LoadObjectAsync(byte* inPath, ObjectLoadedCallback inCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            IntPtr inCallbackPtr = Marshal.GetFunctionPointerForDelegate(inCallback);
            LoadObjectAsyncPrivate(inPath, inCallbackPtr, inRefcon);
            GC.KeepAlive(inCallbackPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine loads an object asynchronously; control is returned to you
        /// immediately while X-Plane loads the object. The sim will not stop flying
        /// while the object loads. For large objects, it may be several seconds before
        /// the load finishes.
        /// </para>
        /// <para>
        /// You provide a callback function that is called once the load has completed.
        /// Note that if the object cannot be loaded, you will not find out until the
        /// callback function is called with a NULL object handle.
        /// </para>
        /// <para>
        /// There is no way to cancel an asynchronous object load; you must wait for
        /// the load to complete and then release the object if it is no longer
        /// desired.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void LoadObjectAsync(in ReadOnlySpan<char> inPath, ObjectLoadedCallback inCallback, void* inRefcon)
        {
            IL.DeclareLocals(false);
            Span<byte> inPathUtf8 = stackalloc byte[(inPath.Length << 1) | 1];
            var inPathPtr = Utils.ToUtf8Unsafe(inPath, inPathUtf8);
            LoadObjectAsync(inPathPtr, inCallback, inRefcon);
        }

        
        /// <summary>
        /// <para>
        /// XPLMDrawObjects draws an object from an OBJ file one or more times. You
        /// pass in the object and an array of XPLMDrawInfo_t structs, one for each
        /// place you would like the object to be drawn.
        /// </para>
        /// <para>
        /// X-Plane will attempt to cull the objects based on LOD and visibility, and
        /// will pick the appropriate LOD.
        /// </para>
        /// <para>
        /// Lighting is a boolean; pass 1 to show the night version of object with
        /// night-only lights lit up. Pass 0 to show the daytime version of the object.
        /// </para>
        /// <para>
        /// earth_relative controls the coordinate system. If this is 1, the rotations
        /// you specify are applied to the object after its coordinate system is
        /// transformed from local to earth-relative coordinates -- that is, an object
        /// with no rotations will point toward true north and the Y axis will be up
        /// against gravity. If this is 0, the object is drawn with your rotations from
        /// local coordanates -- that is, an object with no rotations is drawn pointing
        /// down the -Z axis and the Y axis of the object matches the local coordinate
        /// Y axis.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawObjects(ObjectRef inObject, int inCount, DrawInfo* inLocations, int lighting, int earth_relative)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DrawObjectsPtr);
            IL.Push(inObject);
            IL.Push(inCount);
            IL.Push(inLocations);
            IL.Push(lighting);
            IL.Push(earth_relative);
            IL.Push(DrawObjectsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ObjectRef), typeof(int), typeof(DrawInfo*), typeof(int), typeof(int)));
        }

        
        /// <summary>
        /// <para>
        /// This routine marks an object as no longer being used by your plugin.
        /// Objects are reference counted: once no plugins are using an object, it is
        /// purged from memory. Make sure to call XPLMUnloadObject once for each
        /// successful call to XPLMLoadObject.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void UnloadObject(ObjectRef inObject)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(UnloadObjectPtr);
            IL.Push(inObject);
            IL.Push(UnloadObjectPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(ObjectRef)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe int LookupObjectsPrivate(byte* inPath, float inLatitude, float inLongitude, IntPtr enumerator, void* @ref)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(LookupObjectsPtr);
            int result;
            IL.Push(inPath);
            IL.Push(inLatitude);
            IL.Push(inLongitude);
            IL.Push(enumerator);
            IL.Push(@ref);
            IL.Push(LookupObjectsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte*), typeof(float), typeof(float), typeof(LibraryEnumeratorCallback), typeof(void*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine looks up a virtual path in the library system and returns all
        /// matching elements. You provide a callback - one virtual path may match many
        /// objects in the library. XPLMLookupObjects returns the number of objects
        /// found.
        /// </para>
        /// <para>
        /// The latitude and longitude parameters specify the location the object will
        /// be used. The library system allows for scenery packages to only provide
        /// objects to certain local locations. Only objects that are allowed at the
        /// latitude/longitude you provide will be returned.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LookupObjects(byte* inPath, float inLatitude, float inLongitude, LibraryEnumeratorCallback enumerator, void* @ref)
        {
            IL.DeclareLocals(false);
            IntPtr enumeratorPtr = Marshal.GetFunctionPointerForDelegate(enumerator);
            int result = LookupObjectsPrivate(inPath, inLatitude, inLongitude, enumeratorPtr, @ref);
            GC.KeepAlive(enumeratorPtr);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// This routine looks up a virtual path in the library system and returns all
        /// matching elements. You provide a callback - one virtual path may match many
        /// objects in the library. XPLMLookupObjects returns the number of objects
        /// found.
        /// </para>
        /// <para>
        /// The latitude and longitude parameters specify the location the object will
        /// be used. The library system allows for scenery packages to only provide
        /// objects to certain local locations. Only objects that are allowed at the
        /// latitude/longitude you provide will be returned.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LookupObjects(in ReadOnlySpan<char> inPath, float inLatitude, float inLongitude, LibraryEnumeratorCallback enumerator, void* @ref)
        {
            IL.DeclareLocals(false);
            Span<byte> inPathUtf8 = stackalloc byte[(inPath.Length << 1) | 1];
            var inPathPtr = Utils.ToUtf8Unsafe(inPath, inPathUtf8);
            return LookupObjects(inPathPtr, inLatitude, inLongitude, enumerator, @ref);
        }
    }
}