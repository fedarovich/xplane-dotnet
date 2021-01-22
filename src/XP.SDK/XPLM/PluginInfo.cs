#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Provides information about a plugin.
    /// </summary>
    public readonly struct PluginInfo
    {
        private readonly int _idPlusOne;

        public PluginID Id => unchecked(_idPlusOne - 1);

        /// <summary>
        /// Initializes a new instance of <see cref="PluginInfo"/>.
        /// </summary>
        /// <param name="id">Plugin ID.</param>
        public PluginInfo(PluginID id)
        {
            _idPlusOne = unchecked((int) id + 1);
        }

        /// <summary>
        /// Gets the information about this plugin.
        /// </summary>
        public static PluginInfo ThisPlugin => new PluginInfo(PluginAPI.GetMyID());

        /// <summary>
        /// Gets the list of plugins.
        /// </summary>
        public static IReadOnlyList<PluginInfo> AllPlugins { get; } = new PluginInfoList();

        /// <summary>
        /// Gets the human-readable name of the plug-in.
        /// </summary>
        /// <seealso cref="GetName"/>
        public unsafe string Name
        {
            get
            {
                byte* buffer = stackalloc byte[256];
                PluginAPI.GetPluginInfo(Id, buffer, null, null, null);
                return Marshal.PtrToStringUTF8((nint) buffer)!;
            }
        }

        /// <summary>
        /// Gets the absolute file path to the file that contains the plug-in.
        /// </summary>
        /// <seealso cref="GetFilePath"/>
        public unsafe string FilePath
        {
            get
            {
                byte* buffer = stackalloc byte[256];
                PluginAPI.GetPluginInfo(Id, null, buffer, null, null);
                return Marshal.PtrToStringUTF8((nint) buffer)!;
            }
        }

        /// <summary>
        /// Gets the unique string that identifies the plug-in.
        /// </summary>
        /// <seealso cref="GetSignature"/>
        public unsafe string Signature
        {
            get
            {
                byte* buffer = stackalloc byte[256];
                PluginAPI.GetPluginInfo(Id, null, null, buffer, null);
                return Marshal.PtrToStringUTF8((nint) buffer)!;
            }
        }

        /// <summary>
        /// Get the human-readable description of the plug-in.
        /// </summary>
        /// <seealso cref="GetDescription"/>
        public unsafe string Description
        {
            get
            {
                byte* buffer = stackalloc byte[256];
                PluginAPI.GetPluginInfo(Id, null, null, null, buffer);
                return Marshal.PtrToStringUTF8((nint) buffer)!;
            }
        }

        /// <summary>
        /// Gets the human-readable name of the plug-in.
        /// </summary>
        /// <seealso cref="Name"/>
        public unsafe Utf8String GetName()
        {
            var buffer = GC.AllocateUninitializedArray<byte>(256);
            fixed (byte* pBuffer = buffer)
            {
                PluginAPI.GetPluginInfo(Id, pBuffer, null, null, null);
            }
            return new Utf8String(buffer);
        }

        /// <summary>
        /// Gets the absolute file path to the file that contains the plug-in.
        /// </summary>
        /// <seealso cref="FilePath"/>
        public unsafe Utf8String GetFilePath()
        {
            var buffer = GC.AllocateUninitializedArray<byte>(256);
            fixed (byte* pBuffer = buffer)
            {
                PluginAPI.GetPluginInfo(Id, null, pBuffer, null, null);
            }
            return new Utf8String(buffer);
        }

        /// <summary>
        /// Gets the unique string that identifies the plug-in.
        /// </summary>
        /// <seealso cref="Signature"/>
        public unsafe Utf8String GetSignature()
        {
            var buffer = GC.AllocateUninitializedArray<byte>(256);
            fixed (byte* pBuffer = buffer)
            {
                PluginAPI.GetPluginInfo(Id, null, null, pBuffer, null);
            }
            return new Utf8String(buffer);
        }

        /// <summary>
        /// Get the human-readable description of the plug-in.
        /// </summary>
        /// <seealso cref="Description"/>
        public unsafe Utf8String GetDescription()
        {
            var buffer = GC.AllocateUninitializedArray<byte>(256);
            fixed (byte* pBuffer = buffer)
            {
                PluginAPI.GetPluginInfo(Id, null, null, null, pBuffer);
            }
            return new Utf8String(buffer);
        }

        /// <summary>
        /// Gets the value indicating whether the plugin is enabled.
        /// </summary>
        public bool IsEnabled => PluginAPI.IsPluginEnabled(Id) != 0;

        /// <summary>
        /// Enables the plugin if it not already enabled.
        /// </summary>
        /// <returns><see langword="true"/> if the plugin was successfully enabled; <see langword="false"/> otherwise.</returns>
        public bool Enable() => PluginAPI.EnablePlugin(Id) != 0;

        /// <summary>
        /// Disables the plugin.
        /// </summary>
        public void Disable() => PluginAPI.DisablePlugin(Id);

        /// <summary>
        /// Sends the message to the plugin. Only enabled plug-ins with
        /// a message receive function receive the message.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="param">Message parameter.</param>
        public unsafe void SendMessage(int message, IntPtr param = default) => PluginAPI.SendMessageToPlugin(Id, message, param.ToPointer());

        /// <summary>
        /// Broadcasts the message to all plugins. Only enabled plug-ins with
        /// a message receive function receive the message.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="param">Message parameter.</param>
        public static unsafe void BroadcastMessage(int message, IntPtr param = default) => PluginAPI.SendMessageToPlugin(PluginID.NoPlugin, message, param.ToPointer());

        /// <summary>
        /// Get the information about the plug-in whose file exists at the
        /// passed in absolute file system path.
        /// </summary>
        public static PluginInfo FindByPath(in Utf8String path) => new PluginInfo(PluginAPI.FindPluginByPath(path));

        /// <summary>
        /// Get the plug-in information of the plug-in whose file exists at the
        /// passed in absolute file system path.
        /// </summary>
        public static PluginInfo FindByPath(string path) => new PluginInfo(PluginAPI.FindPluginByPath(path));

        /// <summary>
        /// Gets the information about the plug-in whose signature matches
        /// what is passed in or <see langword="default"/> if no running plug-in has this
        /// signature.
        /// </summary>
        /// <remarks>
        /// Signatures are the best way to identify another plug-in as they
        /// are independent of the file system path of a plug-in or the human-readable
        /// plug-in name, and should be unique for all plug-ins.  Use this routine to
        /// locate another plugin that your plugin interoperates with.
        /// </remarks>
        public static PluginInfo FindBySignature(in Utf8String signature) => new PluginInfo(PluginAPI.FindPluginBySignature(signature));


        /// <summary>
        /// Gets the information about the plug-in whose signature matches
        /// what is passed in or <see langword="default"/> if no running plug-in has this
        /// signature.
        /// </summary>
        /// <remarks>
        /// Signatures are the best way to identify another plug-in as they
        /// are independent of the file system path of a plug-in or the human-readable
        /// plug-in name, and should be unique for all plug-ins.  Use this routine to
        /// locate another plugin that your plugin interoperates with.
        /// </remarks>
        public static PluginInfo FindBySignature(string signature) => new PluginInfo(PluginAPI.FindPluginBySignature(signature));

        /// <summary>
        /// Reloads all plug-ins.  
        /// </summary>
        /// <remarks>
        /// Once this routine is called and you return from the callback you were within
        /// (e.g. a menu select callback) you will receive your XPluginDisable and
        /// XPluginStop callbacks and your DLL will be unloaded,
        /// then the start process happens as if the sim was starting up.
        /// </remarks>
        public static void ReloadPlugins() => PluginAPI.ReloadPlugins();

        private class PluginInfoList : IReadOnlyList<PluginInfo>
        {
            public IEnumerator<PluginInfo> GetEnumerator()
            {
                int count = PluginAPI.CountPlugins();
                for (int index = 0; index < count; index++)
                {
                    yield return new PluginInfo(PluginAPI.GetNthPlugin(index));
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int Count => PluginAPI.CountPlugins();

            public PluginInfo this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    return new PluginInfo(PluginAPI.GetNthPlugin(index));
                }
            }
        }
    }
}
