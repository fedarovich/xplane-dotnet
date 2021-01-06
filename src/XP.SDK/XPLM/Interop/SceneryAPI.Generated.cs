using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Interop
{
    public static partial class SceneryAPI
    {
        
        /// <summary>
        /// <para>
        /// Creates a new probe object of a given type and returns.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCreateProbe", ExactSpelling = true)]
        public static extern ProbeRef CreateProbe(ProbeType inProbeType);

        
        /// <summary>
        /// <para>
        /// Deallocates an existing probe object.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDestroyProbe", ExactSpelling = true)]
        public static extern void DestroyProbe(ProbeRef inProbe);

        
        /// <summary>
        /// <para>
        /// Probes the terrain. Pass in the XYZ coordinate of the probe point, a probe
        /// object, and an XPLMProbeInfo_t struct that has its structSize member set
        /// properly. Other fields are filled in if we hit terrain, and a probe result
        /// is returned.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMProbeTerrainXYZ", ExactSpelling = true)]
        public static extern unsafe ProbeResult ProbeTerrainXYZ(ProbeRef inProbe, float inX, float inY, float inZ, ProbeInfo* outInfo);

        
        /// <summary>
        /// <para>
        /// Returns X-Plane's simulated magnetic variation (declination) at the
        /// indication latitude and longitude.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetMagneticVariation", ExactSpelling = true)]
        public static extern float GetMagneticVariation(double latitude, double longitude);

        
        /// <summary>
        /// <para>
        /// Converts a heading in degrees relative to true north into a value relative
        /// to magnetic north at the user's current location.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDegTrueToDegMagnetic", ExactSpelling = true)]
        public static extern float DegTrueToDegMagnetic(float headingDegreesTrue);

        
        /// <summary>
        /// <para>
        /// Converts a heading in degrees relative to magnetic north at the user's
        /// current location into a value relative to true north.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDegMagneticToDegTrue", ExactSpelling = true)]
        public static extern float DegMagneticToDegTrue(float headingDegreesMagnetic);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMLoadObject", ExactSpelling = true)]
        public static extern unsafe ObjectRef LoadObject(byte* inPath);

        
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
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe ObjectRef LoadObject(in ReadOnlySpan<char> inPath)
        {
            int inPathUtf8Len = inPath.Length * 3 + 4;
            Span<byte> inPathUtf8 = inPathUtf8Len <= 4096 ? stackalloc byte[inPathUtf8Len] : new byte[inPathUtf8Len];
            Utils.ToUtf8(inPath, inPathUtf8);
            fixed (byte* inPathPtr = inPathUtf8)
                return LoadObject(inPathPtr);
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
        public static unsafe ObjectRef LoadObject(in XP.SDK.Utf8String inPath)
        {
            fixed (byte* inPathPtr = inPath)
                return LoadObject(inPathPtr);
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMLoadObjectAsync", ExactSpelling = true)]
        public static extern unsafe void LoadObjectAsync(byte* inPath, delegate* unmanaged[Cdecl]<ObjectRef, void*, void> inCallback, void* inRefcon);

        
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
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void LoadObjectAsync(in ReadOnlySpan<char> inPath, delegate* unmanaged[Cdecl]<ObjectRef, void*, void> inCallback, void* inRefcon)
        {
            int inPathUtf8Len = inPath.Length * 3 + 4;
            Span<byte> inPathUtf8 = inPathUtf8Len <= 4096 ? stackalloc byte[inPathUtf8Len] : new byte[inPathUtf8Len];
            Utils.ToUtf8(inPath, inPathUtf8);
            fixed (byte* inPathPtr = inPathUtf8)
                LoadObjectAsync(inPathPtr, inCallback, inRefcon);
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
        public static unsafe void LoadObjectAsync(in XP.SDK.Utf8String inPath, delegate* unmanaged[Cdecl]<ObjectRef, void*, void> inCallback, void* inRefcon)
        {
            fixed (byte* inPathPtr = inPath)
                LoadObjectAsync(inPathPtr, inCallback, inRefcon);
        }

        
        /// <summary>
        /// <para>
        /// This routine marks an object as no longer being used by your plugin.
        /// Objects are reference counted: once no plugins are using an object, it is
        /// purged from memory. Make sure to call XPLMUnloadObject once for each
        /// successful call to XPLMLoadObject.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMUnloadObject", ExactSpelling = true)]
        public static extern void UnloadObject(ObjectRef inObject);

        
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
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMLookupObjects", ExactSpelling = true)]
        public static extern unsafe int LookupObjects(byte* inPath, float inLatitude, float inLongitude, delegate* unmanaged[Cdecl]<byte*, void*, void> enumerator, void* @ref);

        
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
        [SkipLocalsInitAttribute]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LookupObjects(in ReadOnlySpan<char> inPath, float inLatitude, float inLongitude, delegate* unmanaged[Cdecl]<byte*, void*, void> enumerator, void* @ref)
        {
            int inPathUtf8Len = inPath.Length * 3 + 4;
            Span<byte> inPathUtf8 = inPathUtf8Len <= 4096 ? stackalloc byte[inPathUtf8Len] : new byte[inPathUtf8Len];
            Utils.ToUtf8(inPath, inPathUtf8);
            fixed (byte* inPathPtr = inPathUtf8)
                return LookupObjects(inPathPtr, inLatitude, inLongitude, enumerator, @ref);
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
        public static unsafe int LookupObjects(in XP.SDK.Utf8String inPath, float inLatitude, float inLongitude, delegate* unmanaged[Cdecl]<byte*, void*, void> enumerator, void* @ref)
        {
            fixed (byte* inPathPtr = inPath)
                return LookupObjects(inPathPtr, inLatitude, inLongitude, enumerator, @ref);
        }
    }
}