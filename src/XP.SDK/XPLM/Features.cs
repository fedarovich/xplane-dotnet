using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public static class Features
    {
        /// <summary>
        /// Available in the SDK 2.0 and later for X-Plane 9, enabling this capability causes your plugin
        /// to receive drawing hook callbacks when X-Plane builds its off-screen reflection and shadow rendering passes.
        /// Plugins should enable this and examine the dataref sim/graphics/view/plane_render_type
        /// to determine whether the drawing callback is for a reflection, shadow calculation, or the main screen.
        /// Rendering can be simplified or omitted for reflections, and non-solid drawing should be skipped for shadow calculations.
        /// </summary>
        [Obsolete("Direct drawing via draw callbacks is not recommended; use the Instance class to create object models instead.")]
        public const string WantsReflections = "XPLM_WANTS_REFLECTIONS";

        /// <summary>
        /// Modifies the plugin system to use Unix-style paths on all operating systems. Enabled by default.
        /// </summary>
        public const string UseNativePaths = "XPLM_USE_NATIVE_PATHS";

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
        public const string UseNativeWidgetWindows = "XPLM_USE_NATIVE_WIDGET_WINDOWS";

        public static bool HasFeature(string feature) => PluginAPI.HasFeature(feature) != 0;

        public static bool IsFeatureEnabled(string feature) => PluginAPI.IsFeatureEnabled(feature) != 0;

        public static void EnableFeature(string feature, bool enable = true) => PluginAPI.EnableFeature(feature, enable ? 1 : 0);

        public static unsafe IReadOnlyCollection<string> GetFeatures()
        {
            var features = new HashSet<string>();
            var handle = GCHandle.Alloc(features);
            FeatureEnumeratorCallback callback = OnFeatureEnumeratorCallback;
            PluginAPI.EnumerateFeatures(callback, GCHandle.ToIntPtr(handle).ToPointer());
            GC.KeepAlive(callback);
            return features;

            static void OnFeatureEnumeratorCallback(byte* infeature, void* inref) => 
                Utils.TryGetObject<HashSet<string>>(inref)?.Add(Marshal.PtrToStringUTF8(new IntPtr(infeature)));
        }

        
    }
}
