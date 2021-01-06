using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using XP.SDK.Text;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Provides various utility functions of the X-Plane.
    /// </summary>
    public static class XPlane
    {
        private static readonly int MaxPath;
        private static readonly Lazy<(int xplaneVersion, int xplmVersion, HostApplicationID app)> Versions;
        private static readonly Lazy<string> SystemPathLazy;
        private static readonly Lazy<string> PreferencesPathLazy;

        #region Constructor

        static unsafe XPlane()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                MaxPath = 520;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                MaxPath = 4096;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                MaxPath = 1024;
            }
            else
            {
                MaxPath = short.MaxValue;
            }

            Versions = new Lazy<(int xplaneVersion, int xplmVersion, HostApplicationID app)>(GetVersion, LazyThreadSafetyMode.PublicationOnly);
            SystemPathLazy = new Lazy<string>(GetSystemPathString, LazyThreadSafetyMode.PublicationOnly);
            PreferencesPathLazy = new Lazy<string>(GetPreferencesPathString, LazyThreadSafetyMode.PublicationOnly);

            static (int, int, HostApplicationID) GetVersion()
            {
                (int xplaneVersion, int xplmVersion, HostApplicationID app) result = default;
                UtilitiesAPI.GetVersions(&result.xplaneVersion, &result.xplmVersion, &result.app);
                return result;
            }

            static string GetSystemPathString()
            {
                Span<byte> buffer = stackalloc byte[MaxPath];
                byte* ptr = (byte*) Unsafe.AsPointer(ref buffer.GetPinnableReference());
                UtilitiesAPI.GetSystemPath(ptr);
                int length = (int) Utils.CStringLength(ptr);
                return Utf8String.Encoding.GetString(buffer[..length]);
            }

            static string GetPreferencesPathString()
            {
                Span<byte> buffer = stackalloc byte[MaxPath];
                byte* ptr = (byte*) Unsafe.AsPointer(ref buffer.GetPinnableReference());
                UtilitiesAPI.GetPrefsPath(ptr);
                int length = (int) Utils.CStringLength(ptr);
                return Utf8String.Encoding.GetString(buffer[..length]);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the full path to the X-System folder.
        /// </summary>
        /// <seealso cref="GetSystemPath" />
        public static string SystemPath => SystemPathLazy.Value;

        /// <summary>
        /// Gets a full path to a file that is within X-Plane’s preferences directory.
        /// </summary>
        /// <remarks>
        /// You should remove the file name back to the last directory separator to get the preferences directory.
        /// </remarks>
        /// <seealso cref="GetPreferencesPath"/>
        public static string PreferencesPath => PreferencesPathLazy.Value;

        /// <summary>
        /// Returns the langauge the sim is running in.
        /// </summary>
        public static LanguageCode Language => UtilitiesAPI.GetLanguage();

        /// <summary>
        /// Gets the version of X-Plane.
        /// </summary>
        /// <remarks>
        /// The version is a three- or four-digit decimal number, e.g. 1142 for version 11.42 of X-Plane.
        /// </remarks>
        public static int Version => Versions.Value.xplaneVersion;

        /// <summary>
        /// Gets the version of the XPLM DLL.
        /// </summary>
        /// <remarks>
        /// The version is a three-digit decimal number, e.g. 300 for version 3.00 of XPLM.
        /// </remarks>
        public static int XplmVersion => Versions.Value.xplmVersion;

        /// <summary>
        /// Gets the host application ID.
        /// </summary>
        public static HostApplicationID HostApplication => Versions.Value.app;

        #endregion

        #region Methods

        /// <summary>
        /// Gets the full path to the X-System folder.
        /// </summary>
        /// <seealso cref="SystemPath" />
        public static unsafe Utf8String GetSystemPath()
        {
            var buffer = GC.AllocateUninitializedArray<byte>(MaxPath);
            fixed (byte* ptr = buffer)
            {
                UtilitiesAPI.GetSystemPath(ptr);
                return new Utf8String(ptr);
            }
        }

        /// <summary>
        /// Gets a full path to a file that is within X-Plane’s preferences directory.
        /// </summary>
        /// <remarks>
        /// You should remove the file name back to the last directory separator to get the preferences directory.
        /// </remarks>
        /// <seealso cref="PreferencesPath"/>
        public static unsafe Utf8String GetPreferencesPath()
        {
            var buffer = GC.AllocateUninitializedArray<byte>(MaxPath);
            fixed (byte* ptr = buffer)
            {
                UtilitiesAPI.GetPrefsPath(ptr);
                return new Utf8String(ptr);
            }
        }

        /// <summary>
        /// <para>
        /// Installs an error-reporting callback for your plugin.
        /// Normally the plugin system performs minimum diagnostics to maximize
        /// performance. When you install an error callback, you will receive calls due
        /// to certain plugin errors, such as passing bad parameters or incorrect data.
        /// </para>
        /// <para>
        /// The intention is for you to install the error callback during debug
        /// sections and put a break-point inside your callback. This will cause you to
        /// break into the debugger from within the SDK at the point in your plugin
        /// where you made an illegal call.
        /// </para>
        /// <para>
        /// Installing an error callback may activate error checking code that would
        /// not normally run, and this may adversely affect performance, so do not
        /// leave error callbacks installed in shipping plugins.
        /// </para>
        /// <para>
        /// For this reason, it is recommended to use <seealso cref="SetErrorCallbackForDebugBuild"/> instead.
        /// </para>
        /// </summary>
        [Obsolete("This method will cause performance issues and should not be used in Release builds. Use " 
                  + nameof(SetErrorCallbackForDebugBuild) + " method instead, if needed.")]
        public static unsafe void SetErrorCallback(delegate* unmanaged<byte*, void> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            UtilitiesAPI.SetErrorCallback(callback);
        }

        /// <summary>
        /// <para>
        /// Installs an error-reporting callback for your plugin.
        /// Normally the plugin system performs minimum diagnostics to maximize
        /// performance. When you install an error callback, you will receive calls due
        /// to certain plugin errors, such as passing bad parameters or incorrect data.
        /// </para>
        /// <para>
        /// The intention is for you to install the error callback during debug
        /// sections and put a break-point inside your callback. This will cause you to
        /// break into the debugger from within the SDK at the point in your plugin
        /// where you made an illegal call.
        /// </para>
        /// <para>
        /// Installing an error callback may activate error checking code that would
        /// not normally run, and this may adversely affect performance, so do not
        /// leave error callbacks installed in shipping plugins.
        /// </para>
        /// </summary>
        [Conditional("DEBUG")]
        public static unsafe void SetErrorCallbackForDebugBuild(delegate* unmanaged<byte*, void> callback)
        {
#pragma warning disable 618
            SetErrorCallback(callback);
#pragma warning restore 618
        }

        /// <summary>
        /// This function displays the string in a translucent overlay over the current display and also speaks the string if text-to-speech is enabled.
        /// The string is spoken asynchronously, this function returns immediately. This function may not speak or print depending on user preferences.
        /// </summary>
        public static void Speak(in Utf8String str)
        {
            UtilitiesAPI.SpeakString(str);
        }

        /// <summary>
        /// This function displays the string in a translucent overlay over the current display and also speaks the string if text-to-speech is enabled.
        /// The string is spoken asynchronously, this function returns immediately. This function may not speak or print depending on user preferences.
        /// </summary>
        public static void Speak(in ReadOnlySpan<char> str)
        {
            UtilitiesAPI.SpeakString(str);
        }

        /// <summary>
        /// This routine will attempt to find the symbol passed in the <paramref name="name"/>
        /// parameter. If the symbol is found a pointer the function is returned,
        /// otherwise the function will return <seealso cref="IntPtr.Zero"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You can use FindSymbol to utilize newer SDK API features without requiring newer versions of the SDK (and X-Plane)
        /// as your minimum X-Plane version as follows:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Use <seealso cref="Version"/>, <see cref="XplmVersion"/> and <see cref="FindSymbol(in Utf8String)"/> to detect that the host sim is new enough
        /// to use new functions and resolve function pointers.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Conditionally use the new functions if and only if <see cref="FindSymbol(in Utf8String)"/> returns a non-NULL pointer.
        /// </description>
        /// </item>
        /// </list>
        /// <para>
        /// Warning: you should always check the XPLM API version as well as the results of <see cref="FindSymbol(in Utf8String)"/> to determine if functionality is safe to use.
        /// </para>
        /// </remarks>
        public static unsafe IntPtr FindSymbol(in Utf8String name)
        {
            return new IntPtr(UtilitiesAPI.FindSymbol(name));
        }

        /// <summary>
        /// This routine will attempt to find the symbol passed in the <paramref name="name"/>
        /// parameter. If the symbol is found a pointer the function is returned,
        /// otherwise the function will return <seealso cref="IntPtr.Zero"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You can use FindSymbol to utilize newer SDK API features without requiring newer versions of the SDK (and X-Plane)
        /// as your minimum X-Plane version as follows:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Use <seealso cref="Version"/>, <see cref="XplmVersion"/> and <see cref="FindSymbol(in ReadOnlySpan{char})"/> to detect that the host sim is new enough
        /// to use new functions and resolve function pointers.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Conditionally use the new functions if and only if <see cref="FindSymbol(in ReadOnlySpan{char})"/> returns a non-NULL pointer.
        /// </description>
        /// </item>
        /// </list>
        /// <para>
        /// Warning: you should always check the XPLM API version as well as the results of <see cref="FindSymbol(in ReadOnlySpan{char})"/> to determine if functionality is safe to use.
        /// </para>
        /// </remarks>
        public static unsafe IntPtr FindSymbol(in ReadOnlySpan<char> name)
        {
            return new IntPtr(UtilitiesAPI.FindSymbol(name));
        }

        /// <summary>
        /// This routine will attempt to find the symbol passed in the <paramref name="name"/>
        /// parameter. If the symbol is found a delegate is returned,
        /// otherwise the function will return <seealso langword="null"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You can use FindSymbol to utilize newer SDK API features without requiring newer versions of the SDK (and X-Plane)
        /// as your minimum X-Plane version as follows:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Use <seealso cref="Version"/>, <see cref="XplmVersion"/> and <see cref="FindFunction{T}(in Utf8String)"/> to detect that the host sim is new enough
        /// to use new functions and resolve function pointers.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Conditionally use the new functions if and only if <see cref="FindFunction{T}(in Utf8String)"/> returns a non-NULL pointer.
        /// </description>
        /// </item>
        /// </list>
        /// <para>
        /// Warning: you should always check the XPLM API version as well as the results of <see cref="FindFunction{T}(in Utf8String)"/> to determine if functionality is safe to use.
        /// </para>
        /// </remarks>
        public static T FindFunction<T>(in Utf8String name) where T : Delegate
        {
            var ptr = FindSymbol(name);
            return ptr != default ? Marshal.GetDelegateForFunctionPointer<T>(ptr) : null;
        }

        /// <summary>
        /// This routine will attempt to find the symbol passed in the <paramref name="name"/>
        /// parameter. If the symbol is found a delegate is returned,
        /// otherwise the function will return <seealso langword="null"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You can use FindSymbol to utilize newer SDK API features without requiring newer versions of the SDK (and X-Plane)
        /// as your minimum X-Plane version as follows:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Use <seealso cref="Version"/>, <see cref="XplmVersion"/> and <see cref="FindFunction{T}(in ReadOnlySpan{char})"/> to detect that the host sim is new enough
        /// to use new functions and resolve function pointers.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Conditionally use the new functions if and only if <see cref="FindFunction{T}(in ReadOnlySpan{char})"/> returns a non-NULL pointer.
        /// </description>
        /// </item>
        /// </list>
        /// <para>
        /// Warning: you should always check the XPLM API version as well as the results of <see cref="FindFunction{T}(in ReadOnlySpan{char})"/> to determine if functionality is safe to use.
        /// </para>
        /// </remarks>
        public static T FindFunction<T>(in ReadOnlySpan<char> name) where T : Delegate
        {
            var ptr = FindSymbol(name);
            return ptr != default ? Marshal.GetDelegateForFunctionPointer<T>(ptr) : null;
        }

        /// <summary>
        /// Loads a data file of a given type. Paths must be relative to the X-System folder.
        /// To clear the replay, pass <see langword="null"/> or <see langword="default"/> as the <paramref name="path"/>
        /// (this is only valid with replay movies, not sit files).
        /// </summary>
        public static int LoadDataFile(DataFileType fileType, in Utf8String path = default)
        {
            return UtilitiesAPI.LoadDataFile(fileType, path);
        }

        /// <summary>
        /// Loads a data file of a given type. Paths must be relative to the X-System folder.
        /// To clear the replay, pass <see langword="null"/> or <see langword="default"/> as the <paramref name="path"/>
        /// (this is only valid with replay movies, not sit files).
        /// </summary>
        public static int LoadDataFile(DataFileType fileType, in ReadOnlySpan<char> path)
        {
            return UtilitiesAPI.LoadDataFile(fileType, path);
        }

        /// <summary>
        /// Saves the current situation or replay; paths are relative to the X-System folder.
        /// </summary>
        public static int SaveDataFile(DataFileType fileType, in Utf8String path)
        {
            return UtilitiesAPI.SaveDataFile(fileType, path);
        }
        
        /// <summary>
        /// Saves the current situation or replay; paths are relative to the X-System folder.
        /// </summary>
        public static int SaveDataFile(DataFileType fileType, in ReadOnlySpan<char> path)
        {
            return UtilitiesAPI.SaveDataFile(fileType, path);
        }

        /// <summary>
        /// Given a virtual key code this routine returns a human-readable string describing the character.
        /// This routine is provided for showing users what keyboard mappings they have set up.
        /// The string may read 'unknown' or be an empty string if the virtual key is unknown.
        /// </summary>
        /// <param name="virtualKey"></param>
        /// <returns></returns>
        public static unsafe string GetVirtualKeyDescription(byte virtualKey)
        {
            var ptr = UtilitiesAPI.GetVirtualKeyDescription(virtualKey);
            return ptr != null ? Marshal.PtrToStringUTF8(new IntPtr(ptr)) : string.Empty;
        }

        /// <summary>
        /// Reloads the current set of scenery.
        /// You can use this function in two typical ways: simply call it to reload the scenery,
        /// picking up any new installed scenery, .env files, etc. from disk.
        /// Or, change the lat/ref and lon/ref data refs and then call this function to shift the scenery environment.
        /// This routine is equivalent to picking "reload scenery" from the developer menu.
        /// </summary>
        public static void ReloadScenery()
        {
            UtilitiesAPI.ReloadScenery();
        }

        #endregion

        #region Nested Types

        /// <summary>
        /// Provides the methods to output string to the X-Plane's Log.txt file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Please do not leave routine diagnostic logging enabled in your shipping plugin.
        /// The X-Plane Log file is shared by X-Plane and every plugin in the system,
        /// and plugins that (when functioning normally) print verbose log output make it difficult for developers
        /// to find error conditions from other parts of the system.
        /// </para>
        /// <para>
        /// For most cases you should use <seealso cref="Debug"/> which is no-op in Release builds.
        /// </para>
        /// </remarks>
        /// <seealso cref="Debug"/>
        public static class Trace
        {
            /// <summary>
            /// This routine outputs a string to the Log.txt file.
            /// The file is immediately flushed so you will not lose data. (This does cause a performance penalty.)
            /// </summary>
            /// <remarks>
            /// <para>
            /// Please do not leave routine diagnostic logging enabled in your shipping plugin.
            /// The X-Plane Log file is shared by X-Plane and every plugin in the system,
            /// and plugins that (when functioning normally) print verbose log output make it difficult for developers
            /// to find error conditions from other parts of the system.
            /// </para>
            /// <para>
            /// For most cases you should use <seealso cref="Debug.Write"/> which is no-op in Release builds.
            /// </para>
            /// </remarks>
            /// <seealso cref="Debug"/>
            public static void Write(in ReadOnlySpan<char> str)
            {
                UtilitiesAPI.DebugString(str);
            }

            /// <summary>
            /// This routine outputs a string to the Log.txt file.
            /// The file is immediately flushed so you will not lose data. (This does cause a performance penalty.)
            /// </summary>
            /// <remarks>
            /// <para>
            /// Please do not leave routine diagnostic logging enabled in your shipping plugin.
            /// The X-Plane Log file is shared by X-Plane and every plugin in the system,
            /// and plugins that (when functioning normally) print verbose log output make it difficult for developers
            /// to find error conditions from other parts of the system.
            /// </para>
            /// <para>
            /// For most cases you should use <seealso cref="Debug.Write"/> which is no-op in Release builds.
            /// </para>
            /// </remarks>
            /// <seealso cref="Debug"/>
            public static void Write(in Utf8String str)
            {
                UtilitiesAPI.DebugString(str);
            }

            /// <summary>
            /// This routine outputs a string to the Log.txt file.
            /// The file is immediately flushed so you will not lose data. (This does cause a performance penalty.)
            /// </summary>
            /// <remarks>
            /// <para>
            /// Please do not leave routine diagnostic logging enabled in your shipping plugin.
            /// The X-Plane Log file is shared by X-Plane and every plugin in the system,
            /// and plugins that (when functioning normally) print verbose log output make it difficult for developers
            /// to find error conditions from other parts of the system.
            /// </para>
            /// <para>
            /// For most cases you should use <seealso cref="Debug.WriteLine"/> which is no-op in Release builds.
            /// </para>
            /// </remarks>
            /// <seealso cref="Debug"/>
            [SkipLocalsInit]
            public static unsafe void WriteLine(in ReadOnlySpan<char> str)
            {
                int strLength = str.Length * 3 + 4 + 3;
                Span<byte> strUtf8 = strLength <= 4096 ? stackalloc byte[strLength] : GC.AllocateUninitializedArray<byte>(strLength);
                Utf8.FromUtf16(str, strUtf8, out _, out var length);
                fixed (byte* pStr = strUtf8)
                {
                    // TODO: Check whether we have to use \r\n on Windows.
                    pStr[length] = (byte) '\n';
                    pStr[length + 1] = 0;
                    UtilitiesAPI.DebugString(pStr);
                }
            }

            /// <summary>
            /// This routine outputs a string to the Log.txt file.
            /// The file is immediately flushed so you will not lose data. (This does cause a performance penalty.)
            /// </summary>
            /// <remarks>
            /// <para>
            /// Please do not leave routine diagnostic logging enabled in your shipping plugin.
            /// The X-Plane Log file is shared by X-Plane and every plugin in the system,
            /// and plugins that (when functioning normally) print verbose log output make it difficult for developers
            /// to find error conditions from other parts of the system.
            /// </para>
            /// <para>
            /// For most cases you should use <seealso cref="Debug.WriteLine"/> which is no-op in Release builds.
            /// </para>
            /// </remarks>
            /// <seealso cref="Debug"/>
            public static void WriteLine(in Utf8String str)
            {
                using var builder = Utf8StringBuilderFactory.SharedPooled.CreateBuilder(str.Length + 3);
                builder.Append(str);
                builder.AppendLine();
                UtilitiesAPI.DebugString(builder.BuildUnsafe());
            }
        }

        /// <summary>
        /// Provides the methods to output string to the X-Plane's Log.txt file.
        /// </summary>
        /// <remarks>
        /// This method is no-op if DEBUG is not defined.
        /// </remarks>
        /// <seealso cref="Trace" />
        public static class Debug
        {
            /// <summary>
            /// This routine outputs a string to the Log.txt file.
            /// The file is immediately flushed so you will not lose data. (This does cause a performance penalty.)
            /// </summary>
            /// <remarks>
            /// This method is no-op if DEBUG is not defined.
            /// </remarks>
            /// <seealso cref="WriteLine"/>
            /// <seealso cref="Trace"/>
            [Conditional("DEBUG")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Write(in ReadOnlySpan<char> str) => Trace.Write(str);

            /// <summary>
            /// This routine outputs a string to the Log.txt file.
            /// The file is immediately flushed so you will not lose data. (This does cause a performance penalty.)
            /// </summary>
            /// <remarks>
            /// This method is no-op if DEBUG is not defined.
            /// </remarks>
            /// <seealso cref="Write"/>
            /// <seealso cref="Trace"/>
            [Conditional("DEBUG")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void WriteLine(in ReadOnlySpan<char> str) => Trace.WriteLine(str);
        }

        #endregion
    }
}
