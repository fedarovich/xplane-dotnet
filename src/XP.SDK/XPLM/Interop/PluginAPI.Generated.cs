using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;

namespace XP.SDK.XPLM.Interop
{
    public static partial class PluginAPI
    {
        
        /// <summary>
        /// <para>
        /// This routine returns the plugin ID of the calling plug-in.  Call this to
        /// get your own ID.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetMyID", ExactSpelling = true)]
        public static extern PluginID GetMyID();

        
        /// <summary>
        /// <para>
        /// This routine returns the total number of plug-ins that are loaded, both
        /// disabled and enabled.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMCountPlugins", ExactSpelling = true)]
        public static extern int CountPlugins();

        
        /// <summary>
        /// <para>
        /// This routine returns the ID of a plug-in by index.  Index is 0 based from 0
        /// to XPLMCountPlugins-1, inclusive. Plugins may be returned in any arbitrary
        /// order.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetNthPlugin", ExactSpelling = true)]
        public static extern PluginID GetNthPlugin(int inIndex);

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose file exists at the
        /// passed in absolute file system path.  XPLM_NO_PLUGIN_ID is returned if the
        /// path does not point to a currently loaded plug-in.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMFindPluginByPath", ExactSpelling = true)]
        public static extern unsafe PluginID FindPluginByPath(byte* inPath);

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose file exists at the
        /// passed in absolute file system path.  XPLM_NO_PLUGIN_ID is returned if the
        /// path does not point to a currently loaded plug-in.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe PluginID FindPluginByPath(in XP.SDK.Utf8String inPath)
        {
            fixed (byte* inPathPtr = inPath)
                return FindPluginByPath(inPathPtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose file exists at the
        /// passed in absolute file system path.  XPLM_NO_PLUGIN_ID is returned if the
        /// path does not point to a currently loaded plug-in.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        public static unsafe PluginID FindPluginByPath(in ReadOnlySpan<char> inPath)
        {
            int inPathUtf8Len = inPath.Length * 3 + 4;
            Span<byte> inPathUtf8 = inPathUtf8Len <= 4096 ? stackalloc byte[inPathUtf8Len] : GC.AllocateUninitializedArray<byte>(inPathUtf8Len);
            var inPathUtf8Str = Utf8String.FromUtf16Unsafe(inPath, inPathUtf8);
            return FindPluginByPath(inPathUtf8Str);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose signature matches
        /// what is passed in or XPLM_NO_PLUGIN_ID if no running plug-in has this
        /// signature.  Signatures are the best way to identify another plug-in as they
        /// are independent of the file system path of a plug-in or the human-readable
        /// plug-in name, and should be unique for all plug-ins.  Use this routine to
        /// locate another plugin that your plugin interoperates with
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMFindPluginBySignature", ExactSpelling = true)]
        public static extern unsafe PluginID FindPluginBySignature(byte* inSignature);

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose signature matches
        /// what is passed in or XPLM_NO_PLUGIN_ID if no running plug-in has this
        /// signature.  Signatures are the best way to identify another plug-in as they
        /// are independent of the file system path of a plug-in or the human-readable
        /// plug-in name, and should be unique for all plug-ins.  Use this routine to
        /// locate another plugin that your plugin interoperates with
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe PluginID FindPluginBySignature(in XP.SDK.Utf8String inSignature)
        {
            fixed (byte* inSignaturePtr = inSignature)
                return FindPluginBySignature(inSignaturePtr);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns the plug-in ID of the plug-in whose signature matches
        /// what is passed in or XPLM_NO_PLUGIN_ID if no running plug-in has this
        /// signature.  Signatures are the best way to identify another plug-in as they
        /// are independent of the file system path of a plug-in or the human-readable
        /// plug-in name, and should be unique for all plug-ins.  Use this routine to
        /// locate another plugin that your plugin interoperates with
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        public static unsafe PluginID FindPluginBySignature(in ReadOnlySpan<char> inSignature)
        {
            int inSignatureUtf8Len = inSignature.Length * 3 + 4;
            Span<byte> inSignatureUtf8 = inSignatureUtf8Len <= 4096 ? stackalloc byte[inSignatureUtf8Len] : GC.AllocateUninitializedArray<byte>(inSignatureUtf8Len);
            var inSignatureUtf8Str = Utf8String.FromUtf16Unsafe(inSignature, inSignatureUtf8);
            return FindPluginBySignature(inSignatureUtf8Str);
        }

        
        /// <summary>
        /// <para>
        /// This routine returns information about a plug-in.  Each parameter should be
        /// a pointer to a buffer of at least
        /// 256 characters, or NULL to not receive the information.
        /// </para>
        /// <para>
        /// outName - the human-readable name of the plug-in. outFilePath - the
        /// absolute file path to the file that contains this plug-in. outSignature - a
        /// unique string that identifies this plug-in. outDescription - a
        /// human-readable description of this plug-in.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMGetPluginInfo", ExactSpelling = true)]
        public static extern unsafe void GetPluginInfo(PluginID inPlugin, byte* outName, byte* outFilePath, byte* outSignature, byte* outDescription);

        
        /// <summary>
        /// <para>
        /// Returns whether the specified plug-in is enabled for running.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMIsPluginEnabled", ExactSpelling = true)]
        public static extern int IsPluginEnabled(PluginID inPluginID);

        
        /// <summary>
        /// <para>
        /// This routine enables a plug-in if it is not already enabled. It returns 1
        /// if the plugin was enabled or successfully enables itself, 0 if it does not.
        /// Plugins may fail to enable (for example, if resources cannot be acquired)
        /// by returning 0 from their XPluginEnable callback.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMEnablePlugin", ExactSpelling = true)]
        public static extern int EnablePlugin(PluginID inPluginID);

        
        /// <summary>
        /// <para>
        /// This routine disableds an enabled plug-in.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMDisablePlugin", ExactSpelling = true)]
        public static extern void DisablePlugin(PluginID inPluginID);

        
        /// <summary>
        /// <para>
        /// This routine reloads all plug-ins.  Once this routine is called and you
        /// return from the callback you were within (e.g. a menu select callback) you
        /// will receive your XPluginDisable and XPluginStop callbacks and your DLL
        /// will be unloaded, then the start process happens as if the sim was starting
        /// up.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMReloadPlugins", ExactSpelling = true)]
        public static extern void ReloadPlugins();

        
        /// <summary>
        /// <para>
        /// This function sends a message to another plug-in or X-Plane.  Pass
        /// XPLM_NO_PLUGIN_ID to broadcast to all plug-ins.  Only enabled plug-ins with
        /// a message receive function receive the message.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMSendMessageToPlugin", ExactSpelling = true)]
        public static extern unsafe void SendMessageToPlugin(PluginID inPlugin, int inMessage, void* inParam);

        
        /// <summary>
        /// <para>
        /// This returns 1 if the given installation of X-Plane supports a feature, or
        /// 0 if it does not.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMHasFeature", ExactSpelling = true)]
        public static extern unsafe int HasFeature(byte* inFeature);

        
        /// <summary>
        /// <para>
        /// This returns 1 if the given installation of X-Plane supports a feature, or
        /// 0 if it does not.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int HasFeature(in XP.SDK.Utf8String inFeature)
        {
            fixed (byte* inFeaturePtr = inFeature)
                return HasFeature(inFeaturePtr);
        }

        
        /// <summary>
        /// <para>
        /// This returns 1 if the given installation of X-Plane supports a feature, or
        /// 0 if it does not.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        public static unsafe int HasFeature(in ReadOnlySpan<char> inFeature)
        {
            int inFeatureUtf8Len = inFeature.Length * 3 + 4;
            Span<byte> inFeatureUtf8 = inFeatureUtf8Len <= 4096 ? stackalloc byte[inFeatureUtf8Len] : GC.AllocateUninitializedArray<byte>(inFeatureUtf8Len);
            var inFeatureUtf8Str = Utf8String.FromUtf16Unsafe(inFeature, inFeatureUtf8);
            return HasFeature(inFeatureUtf8Str);
        }

        
        /// <summary>
        /// <para>
        /// This returns 1 if a feature is currently enabled for your plugin, or 0 if
        /// it is not enabled.  It is an error to call this routine with an unsupported
        /// feature.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMIsFeatureEnabled", ExactSpelling = true)]
        public static extern unsafe int IsFeatureEnabled(byte* inFeature);

        
        /// <summary>
        /// <para>
        /// This returns 1 if a feature is currently enabled for your plugin, or 0 if
        /// it is not enabled.  It is an error to call this routine with an unsupported
        /// feature.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IsFeatureEnabled(in XP.SDK.Utf8String inFeature)
        {
            fixed (byte* inFeaturePtr = inFeature)
                return IsFeatureEnabled(inFeaturePtr);
        }

        
        /// <summary>
        /// <para>
        /// This returns 1 if a feature is currently enabled for your plugin, or 0 if
        /// it is not enabled.  It is an error to call this routine with an unsupported
        /// feature.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        public static unsafe int IsFeatureEnabled(in ReadOnlySpan<char> inFeature)
        {
            int inFeatureUtf8Len = inFeature.Length * 3 + 4;
            Span<byte> inFeatureUtf8 = inFeatureUtf8Len <= 4096 ? stackalloc byte[inFeatureUtf8Len] : GC.AllocateUninitializedArray<byte>(inFeatureUtf8Len);
            var inFeatureUtf8Str = Utf8String.FromUtf16Unsafe(inFeature, inFeatureUtf8);
            return IsFeatureEnabled(inFeatureUtf8Str);
        }

        
        /// <summary>
        /// <para>
        /// This routine enables or disables a feature for your plugin.  This will
        /// change the running behavior of X-Plane and your plugin in some way,
        /// depending on the feature.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMEnableFeature", ExactSpelling = true)]
        public static extern unsafe void EnableFeature(byte* inFeature, int inEnable);

        
        /// <summary>
        /// <para>
        /// This routine enables or disables a feature for your plugin.  This will
        /// change the running behavior of X-Plane and your plugin in some way,
        /// depending on the feature.
        /// </para>
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EnableFeature(in XP.SDK.Utf8String inFeature, int inEnable)
        {
            fixed (byte* inFeaturePtr = inFeature)
                EnableFeature(inFeaturePtr, inEnable);
        }

        
        /// <summary>
        /// <para>
        /// This routine enables or disables a feature for your plugin.  This will
        /// change the running behavior of X-Plane and your plugin in some way,
        /// depending on the feature.
        /// </para>
        /// </summary>
        [SkipLocalsInitAttribute]
        public static unsafe void EnableFeature(in ReadOnlySpan<char> inFeature, int inEnable)
        {
            int inFeatureUtf8Len = inFeature.Length * 3 + 4;
            Span<byte> inFeatureUtf8 = inFeatureUtf8Len <= 4096 ? stackalloc byte[inFeatureUtf8Len] : GC.AllocateUninitializedArray<byte>(inFeatureUtf8Len);
            var inFeatureUtf8Str = Utf8String.FromUtf16Unsafe(inFeature, inFeatureUtf8);
            EnableFeature(inFeatureUtf8Str, inEnable);
        }

        
        /// <summary>
        /// <para>
        /// This routine calls your enumerator callback once for each feature that this
        /// running version of X-Plane supports. Use this routine to determine all of
        /// the features that X-Plane can support.
        /// </para>
        /// </summary>
        [DllImportAttribute(Lib.Name, EntryPoint = "XPLMEnumerateFeatures", ExactSpelling = true)]
        public static extern unsafe void EnumerateFeatures(delegate* unmanaged<byte*, void*, void> inEnumerator, void* inRef);
    }
}