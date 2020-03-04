using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Internal
{
    public static partial class Map
    {
        private static IntPtr CreateMapLayerPtr;
        private static IntPtr DestroyMapLayerPtr;
        private static IntPtr RegisterMapCreationHookPtr;
        private static IntPtr MapExistsPtr;
        private static IntPtr DrawMapIconFromSheetPtr;
        private static IntPtr DrawMapLabelPtr;
        private static IntPtr MapProjectPtr;
        private static IntPtr MapUnprojectPtr;
        private static IntPtr MapScaleMeterPtr;
        private static IntPtr MapGetNorthHeadingPtr;

        static Map()
        {
            const string libraryName = "XPLM";
            CreateMapLayerPtr = Lib.GetExport("XPLMCreateMapLayer");
            DestroyMapLayerPtr = Lib.GetExport("XPLMDestroyMapLayer");
            RegisterMapCreationHookPtr = Lib.GetExport("XPLMRegisterMapCreationHook");
            MapExistsPtr = Lib.GetExport("XPLMMapExists");
            DrawMapIconFromSheetPtr = Lib.GetExport("XPLMDrawMapIconFromSheet");
            DrawMapLabelPtr = Lib.GetExport("XPLMDrawMapLabel");
            MapProjectPtr = Lib.GetExport("XPLMMapProject");
            MapUnprojectPtr = Lib.GetExport("XPLMMapUnproject");
            MapScaleMeterPtr = Lib.GetExport("XPLMMapScaleMeter");
            MapGetNorthHeadingPtr = Lib.GetExport("XPLMMapGetNorthHeading");
        }

        
        /// <summary>
        /// <para>
        /// This routine creates a new map layer. You pass in an XPLMCreateMapLayer_t
        /// structure with all of the fields set in.  You must set the structSize of
        /// the structure to the size of the actual structure you used.
        /// </para>
        /// <para>
        /// Returns NULL if the layer creation failed. This happens most frequently
        /// because the map you specified in your
        /// XPLMCreateMapLayer_t::mapToCreateLayerIn field doesn't exist (that is, if
        /// XPLMMapExists() returns 0 for the specified map). You can use
        /// XPLMRegisterMapCreationHook() to get a notification each time a new map is
        /// opened in X-Plane, at which time you can create layers in it.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MapLayerID CreateMapLayer(CreateMapLayer* inParams)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(CreateMapLayerPtr);
            MapLayerID result;
            IL.Push(inParams);
            IL.Push(CreateMapLayerPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(MapLayerID), typeof(CreateMapLayer*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Destroys a map layer you created (calling your
        /// XPLMMapWillBeDeletedCallback_f if applicable). Returns true if a deletion
        /// took place.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int DestroyMapLayer(MapLayerID inLayer)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DestroyMapLayerPtr);
            int result;
            IL.Push(inLayer);
            IL.Push(DestroyMapLayerPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(MapLayerID)));
            IL.Pop(out result);
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static unsafe void RegisterMapCreationHookPrivate(IntPtr callback, void* refcon)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(RegisterMapCreationHookPtr);
            IL.Push(callback);
            IL.Push(refcon);
            IL.Push(RegisterMapCreationHookPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapCreatedCallback), typeof(void*)));
        }

        
        /// <summary>
        /// <para>
        /// Registers your callback to receive a notification each time a new map is
        /// constructed in X-Plane. This callback is the best time to add your custom
        /// map layer using XPLMCreateMapLayer().
        /// </para>
        /// <para>
        /// Note that you will not be notified about any maps that already exist---you
        /// can use XPLMMapExists() to check for maps that were created previously.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RegisterMapCreationHook(MapCreatedCallback callback, void* refcon)
        {
            IL.DeclareLocals(false);
            IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
            RegisterMapCreationHookPrivate(callbackPtr, refcon);
            GC.KeepAlive(callbackPtr);
        }

        
        /// <summary>
        /// <para>
        /// Returns 1 if the map with the specified identifier already exists in
        /// X-Plane. In that case, you can safely call XPLMCreateMapLayer() specifying
        /// that your layer should be added to that map.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int MapExists(byte* mapIdentifier)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(MapExistsPtr);
            int result;
            IL.Push(mapIdentifier);
            IL.Push(MapExistsPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(int), typeof(byte*)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Returns 1 if the map with the specified identifier already exists in
        /// X-Plane. In that case, you can safely call XPLMCreateMapLayer() specifying
        /// that your layer should be added to that map.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int MapExists(in ReadOnlySpan<char> mapIdentifier)
        {
            IL.DeclareLocals(false);
            Span<byte> mapIdentifierUtf8 = stackalloc byte[(mapIdentifier.Length << 1) | 1];
            var mapIdentifierPtr = Utils.ToUtf8Unsafe(mapIdentifier, mapIdentifierUtf8);
            return MapExists(mapIdentifierPtr);
        }

        
        /// <summary>
        /// <para>
        /// Enables plugin-created map layers to draw PNG icons using X-Plane's
        /// built-in icon drawing functionality. Only valid from within an
        /// XPLMIconDrawingCallback_t (but you can request an arbitrary number of icons
        /// to be drawn from within your callback).
        /// </para>
        /// <para>
        /// X-Plane will automatically manage the memory for your texture so that it
        /// only has to be loaded from disk once as long as you continue drawing it
        /// per-frame. (When you stop drawing it, the memory may purged in a "garbage
        /// collection" pass, require a load from disk in the future.)
        /// </para>
        /// <para>
        /// Instead of having X-Plane draw a full PNG, this method allows you to use UV
        /// coordinates to request a portion of the image to be drawn. This allows you
        /// to use a single texture load (of an icon sheet, for example) to draw many
        /// icons. Doing so is much more efficient than drawing a dozen different small
        /// PNGs.
        /// </para>
        /// <para>
        /// The UV coordinates used here treat the texture you load as being comprised
        /// of a number of identically sized "cells." You specify the width and height
        /// in cells (ds and dt, respectively), as well as the coordinates within the
        /// cell grid for the sub-image you'd like to draw.
        /// </para>
        /// <para>
        /// Note that you can use different ds and dt values in subsequent calls with
        /// the same texture sheet. This enables you to use icons of different sizes in
        /// the same sheet if you arrange them properly in the PNG.
        /// </para>
        /// <para>
        /// This function is only valid from within an XPLMIconDrawingCallback_t (but
        /// you can request an arbitrary number of icons to be drawn from within your
        /// callback).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawMapIconFromSheet(MapLayerID layer, byte* inPngPath, int s, int t, int ds, int dt, float mapX, float mapY, MapOrientation orientation, float rotationDegrees, float mapWidth)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DrawMapIconFromSheetPtr);
            IL.Push(layer);
            IL.Push(inPngPath);
            IL.Push(s);
            IL.Push(t);
            IL.Push(ds);
            IL.Push(dt);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(orientation);
            IL.Push(rotationDegrees);
            IL.Push(mapWidth);
            IL.Push(DrawMapIconFromSheetPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapLayerID), typeof(byte*), typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(float), typeof(MapOrientation), typeof(float), typeof(float)));
        }

        
        /// <summary>
        /// <para>
        /// Enables plugin-created map layers to draw PNG icons using X-Plane's
        /// built-in icon drawing functionality. Only valid from within an
        /// XPLMIconDrawingCallback_t (but you can request an arbitrary number of icons
        /// to be drawn from within your callback).
        /// </para>
        /// <para>
        /// X-Plane will automatically manage the memory for your texture so that it
        /// only has to be loaded from disk once as long as you continue drawing it
        /// per-frame. (When you stop drawing it, the memory may purged in a "garbage
        /// collection" pass, require a load from disk in the future.)
        /// </para>
        /// <para>
        /// Instead of having X-Plane draw a full PNG, this method allows you to use UV
        /// coordinates to request a portion of the image to be drawn. This allows you
        /// to use a single texture load (of an icon sheet, for example) to draw many
        /// icons. Doing so is much more efficient than drawing a dozen different small
        /// PNGs.
        /// </para>
        /// <para>
        /// The UV coordinates used here treat the texture you load as being comprised
        /// of a number of identically sized "cells." You specify the width and height
        /// in cells (ds and dt, respectively), as well as the coordinates within the
        /// cell grid for the sub-image you'd like to draw.
        /// </para>
        /// <para>
        /// Note that you can use different ds and dt values in subsequent calls with
        /// the same texture sheet. This enables you to use icons of different sizes in
        /// the same sheet if you arrange them properly in the PNG.
        /// </para>
        /// <para>
        /// This function is only valid from within an XPLMIconDrawingCallback_t (but
        /// you can request an arbitrary number of icons to be drawn from within your
        /// callback).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawMapIconFromSheet(MapLayerID layer, in ReadOnlySpan<char> inPngPath, int s, int t, int ds, int dt, float mapX, float mapY, MapOrientation orientation, float rotationDegrees, float mapWidth)
        {
            IL.DeclareLocals(false);
            Span<byte> inPngPathUtf8 = stackalloc byte[(inPngPath.Length << 1) | 1];
            var inPngPathPtr = Utils.ToUtf8Unsafe(inPngPath, inPngPathUtf8);
            DrawMapIconFromSheet(layer, inPngPathPtr, s, t, ds, dt, mapX, mapY, orientation, rotationDegrees, mapWidth);
        }

        
        /// <summary>
        /// <para>
        /// Enables plugin-created map layers to draw text labels using X-Plane's
        /// built-in labeling functionality. Only valid from within an
        /// XPLMMapLabelDrawingCallback_f (but you can request an arbitrary number of
        /// text labels to be drawn from within your callback).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawMapLabel(MapLayerID layer, byte* inText, float mapX, float mapY, MapOrientation orientation, float rotationDegrees)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(DrawMapLabelPtr);
            IL.Push(layer);
            IL.Push(inText);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(orientation);
            IL.Push(rotationDegrees);
            IL.Push(DrawMapLabelPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapLayerID), typeof(byte*), typeof(float), typeof(float), typeof(MapOrientation), typeof(float)));
        }

        
        /// <summary>
        /// <para>
        /// Enables plugin-created map layers to draw text labels using X-Plane's
        /// built-in labeling functionality. Only valid from within an
        /// XPLMMapLabelDrawingCallback_f (but you can request an arbitrary number of
        /// text labels to be drawn from within your callback).
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DrawMapLabel(MapLayerID layer, in ReadOnlySpan<char> inText, float mapX, float mapY, MapOrientation orientation, float rotationDegrees)
        {
            IL.DeclareLocals(false);
            Span<byte> inTextUtf8 = stackalloc byte[(inText.Length << 1) | 1];
            var inTextPtr = Utils.ToUtf8Unsafe(inText, inTextUtf8);
            DrawMapLabel(layer, inTextPtr, mapX, mapY, orientation, rotationDegrees);
        }

        
        /// <summary>
        /// <para>
        /// Projects a latitude/longitude into map coordinates. This is the inverse of
        /// XPLMMapUnproject().
        /// </para>
        /// <para>
        /// Only valid from within a map layer callback (one of
        /// XPLMMapPrepareCacheCallback_f, XPLMMapDrawingCallback_f,
        /// XPLMMapIconDrawingCallback_f, or XPLMMapLabelDrawingCallback_f.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void MapProject(MapProjectionID projection, double latitude, double longitude, float* outX, float* outY)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(MapProjectPtr);
            IL.Push(projection);
            IL.Push(latitude);
            IL.Push(longitude);
            IL.Push(outX);
            IL.Push(outY);
            IL.Push(MapProjectPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapProjectionID), typeof(double), typeof(double), typeof(float*), typeof(float*)));
        }

        
        /// <summary>
        /// <para>
        /// Transforms map coordinates back into a latitude and longitude. This is the
        /// inverse of XPLMMapProject().
        /// </para>
        /// <para>
        /// Only valid from within a map layer callback (one of
        /// XPLMMapPrepareCacheCallback_f, XPLMMapDrawingCallback_f,
        /// XPLMMapIconDrawingCallback_f, or XPLMMapLabelDrawingCallback_f.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void MapUnproject(MapProjectionID projection, float mapX, float mapY, double* outLatitude, double* outLongitude)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(MapUnprojectPtr);
            IL.Push(projection);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(outLatitude);
            IL.Push(outLongitude);
            IL.Push(MapUnprojectPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(void), typeof(MapProjectionID), typeof(float), typeof(float), typeof(double*), typeof(double*)));
        }

        
        /// <summary>
        /// <para>
        /// Returns the number of map units that correspond to a distance of one meter
        /// at a given set of map coordinates.
        /// </para>
        /// <para>
        /// Only valid from within a map layer callback (one of
        /// XPLMMapPrepareCacheCallback_f, XPLMMapDrawingCallback_f,
        /// XPLMMapIconDrawingCallback_f, or XPLMMapLabelDrawingCallback_f.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float MapScaleMeter(MapProjectionID projection, float mapX, float mapY)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(MapScaleMeterPtr);
            float result;
            IL.Push(projection);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(MapScaleMeterPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(MapProjectionID), typeof(float), typeof(float)));
            IL.Pop(out result);
            return result;
        }

        
        /// <summary>
        /// <para>
        /// Returns the heading (in degrees clockwise from "up") that corresponds to
        /// north at a given point on the map. In other words, if your runway has a
        /// true heading of 360, you would use "north" as the Cartesian angle at which
        /// to draw the runway on the map. (You would add the result of
        /// XPLMMapGetNorthHeading() to your true heading to get the map angle.)
        /// </para>
        /// <para>
        /// This is necessary becuase X-Plane's map can be rotated to match your
        /// aircraft's orientation; north is not always "up."
        /// </para>
        /// <para>
        /// Only valid from within a map layer callback (one of
        /// XPLMMapPrepareCacheCallback_f, XPLMMapDrawingCallback_f,
        /// XPLMMapIconDrawingCallback_f, or XPLMMapLabelDrawingCallback_f.)
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float MapGetNorthHeading(MapProjectionID projection, float mapX, float mapY)
        {
            IL.DeclareLocals(false);
            Guard.NotNull(MapGetNorthHeadingPtr);
            float result;
            IL.Push(projection);
            IL.Push(mapX);
            IL.Push(mapY);
            IL.Push(MapGetNorthHeadingPtr);
            IL.Emit.Calli(new StandAloneMethodSig(CallingConvention.Cdecl, typeof(float), typeof(MapProjectionID), typeof(float), typeof(float)));
            IL.Pop(out result);
            return result;
        }
    }
}