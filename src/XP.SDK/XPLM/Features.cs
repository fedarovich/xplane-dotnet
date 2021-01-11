#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// This class provides capabilities to enumerate and enable/disable X-Plane features.
    /// </summary>
    public static class Features
    {
        /// <summary>
        /// Available in the SDK 2.0 and later for X-Plane 9, enabling this capability causes your plugin
        /// to receive drawing hook callbacks when X-Plane builds its off-screen reflection and shadow rendering passes.
        /// Plugins should enable this and examine the dataref sim/graphics/view/plane_render_type
        /// to determine whether the drawing callback is for a reflection, shadow calculation, or the main screen.
        /// Rendering can be simplified or omitted for reflections, and non-solid drawing should be skipped for shadow calculations.
        /// </summary>
        /// <returns>
        /// The UTF-8 literal <c>XPLM_WANTS_REFLECTIONS</c>.
        /// </returns>
        [Obsolete("Direct drawing via draw callbacks is not recommended; use the Instance class to create object models instead.")]
        public static Utf8String WantsReflections => new(new byte[] { 88, 80, 76, 77, 95, 87, 65, 78, 84, 83, 95, 82, 69, 70, 76, 69, 67, 84, 73, 79, 78, 83, 0 }, 22);

        /// <summary>
        /// Modifies the plugin system to use Unix-style paths on all operating systems. Enabled by default.
        /// </summary>
        /// <returns>
        /// The UTF-8 literal <c>XPLM_USE_NATIVE_PATHS</c>.
        /// </returns>
        public static Utf8String UseNativePaths => new(new byte[] { 88, 80, 76, 77, 95, 85, 83, 69, 95, 78, 65, 84, 73, 86, 69, 95, 80, 65, 84, 72, 83, 0 }, 21);

        /// <summary>
        /// <para>
        /// This capability tells the widgets library to use new, modern X-Plane windows to anchor all widget trees. Without it, widgets will always use legacy windows.
        /// </para>
        /// <para>
        /// Plugins should enable this to allow their widget hierarchies to respond to the user’s UI size settings and to map widget-based windwos to a VR HMD.
        /// </para>
        /// <para>
        /// Before enabling this, make sure any custom widget code in your plugin is prepared to cope with the UI coordinate system not being th same as the OpenGL window coordinate system.
        /// </para>
        /// </summary>
        /// <returns>
        /// The UTF-8 literal <c>XPLM_USE_NATIVE_WIDGET_WINDOWS</c>.
        /// </returns>
        /// <remarks>
        /// This feature is enabled by the managed plugin host by default.
        /// </remarks>
        public static Utf8String UseNativeWidgetWindows => new (new byte[] { 88, 80, 76, 77, 95, 85, 83, 69, 95, 78, 65, 84, 73, 86, 69, 95, 87, 73, 68, 71, 69, 84, 95, 87, 73, 78, 68, 79, 87, 83, 0 }, 30);

        /// <summary>
        /// Returns the value indicating whether the given installation of X-Plane supports the feature.
        /// </summary>
        public static bool HasFeature(in Utf8String feature) => PluginAPI.HasFeature(feature) != 0;

        /// <summary>
        /// Returns the value indicating whether the given installation of X-Plane supports the feature.
        /// </summary>
        public static bool HasFeature(string feature) => PluginAPI.HasFeature(feature) != 0;

        /// <summary>
        /// Gets the value indicating whether the feature is currently enabled for your plugin.
        /// It is an error to call this routine with an unsupported  feature.
        /// </summary>
        public static bool IsFeatureEnabled(in Utf8String feature) => PluginAPI.IsFeatureEnabled(feature) != 0;

        /// <summary>
        /// Gets the value indicating whether the feature is currently enabled for your plugin.
        /// It is an error to call this routine with an unsupported  feature.
        /// </summary>
        public static bool IsFeatureEnabled(string feature) => PluginAPI.IsFeatureEnabled(feature) != 0;

        /// <summary>
        /// This routine enables or disables a feature for your plugin.
        /// This will change the running behavior of X-Plane and your plugin in some way, depending on the feature.
        /// </summary>
        public static void EnableFeature(in Utf8String feature, bool enable = true) => PluginAPI.EnableFeature(feature, enable.ToInt());


        /// <summary>
        /// This routine enables or disables a feature for your plugin.
        /// This will change the running behavior of X-Plane and your plugin in some way, depending on the feature.
        /// </summary>
        public static void EnableFeature(string feature, bool enable = true) => PluginAPI.EnableFeature(feature, enable.ToInt());

        /// <summary>
        /// Gets all features that this running version of X-Plane supports.
        /// </summary>
        /// <returns></returns>
        public static unsafe IReadOnlyCollection<string> GetFeatures()
        {
            var features = new HashSet<string>();
            var handle = GCHandle.Alloc(features);
            try
            {
                PluginAPI.EnumerateFeatures(&Callback, GCHandle.ToIntPtr(handle).ToPointer());
                return features;
            }
            finally
            {
                handle.Free();
            }

            [UnmanagedCallersOnly]
            static void Callback(byte* infeature, void* inref) =>
                Utils.TryGetObject<HashSet<string>>(inref)?.Add(Marshal.PtrToStringUTF8(new IntPtr(infeature))!);
        }
    }
}
