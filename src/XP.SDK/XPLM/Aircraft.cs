#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using InlineIL;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// Provides aircrafts management functions.
    /// </summary>
    public static class Aircraft
    {
        /// <summary>
        /// The index of the aircraft controlled by the user.
        /// </summary>
        public const int UserAircraft = 0;

        /// <summary>
        /// This function returns the number of aircraft X-Plane is capable of having,
        /// as well as the number of aircraft that are currently active.  These numbers
        /// count the user's aircraft.  It can also return the plugin that is currently
        /// controlling aircraft.
        /// </summary>
        public static unsafe (int total, int active, PluginID controller) GetCountAndController()
        {
            (int total, int active, PluginID controller) result = default;
            PlanesAPI.CountAircraft(&result.total, &result.active, &result.controller);
            return result;
        }

        /// <summary>
        /// This function returns the aircraft model for the aircraft with the specified <paramref name="index"/>.
        /// Indices are  zero based, with zero being the user's aircraft. 
        /// </summary>
        public static unsafe (string? fileName, string? path) GetModel(int index)
        {
            IL.DeclareLocals(false);

            var fileNameBuffer = stackalloc byte[512];
            var pathBuffer = stackalloc byte[4096];
            
            PlanesAPI.GetNthAircraftModel(index, fileNameBuffer, pathBuffer);
            var fileName = Marshal.PtrToStringUTF8((IntPtr) fileNameBuffer);
            var path = Marshal.PtrToStringUTF8((IntPtr) pathBuffer);
            return (fileName, path);
        }


        /// <summary>
        /// This routine sets the number of active planes.  If you pass in a number
        /// higher than the total number of planes available, only the total number of
        /// planes available is actually used.
        /// </summary>
        public static void SetActiveAircraftCount(int count)
        {
            PlanesAPI.SetActiveAircraftCount(count);
        }

        /// <summary>
        /// This routine loads an aircraft model. It may only be called if you have
        /// exclusive access to the airplane APIs. Pass in the path of the model with
        /// the .acf extension. The index is zero based, but you may not pass in 0
        /// (use XPLMSetUsersAircraft to load the user's aircracft).
        /// </summary>
        public static void SetModel(int index, in Utf8String aircraftPath)
        {
            PlanesAPI.SetAircraftModel(index, aircraftPath);
        }

        /// <summary>
        /// This routine loads an aircraft model. It may only be called if you have
        /// exclusive access to the airplane APIs. Pass in the path of the model with
        /// the .acf extension. The index is zero based, but you may not pass in 0
        /// (use XPLMSetUsersAircraft to load the user's aircracft).
        /// </summary>
        public static void SetModel(int index, in ReadOnlySpan<char> aircraftPath)
        {
            PlanesAPI.SetAircraftModel(index, aircraftPath);
        }

        /// <summary>
        /// This routine turns off X-Plane's AI for a given plane.  The plane will
        /// continue to draw and be a real plane in X-Plane, but will not move itself.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static void DisableAIForPlane(int index)
        {
            PlanesAPI.DisableAIForPlane(index);
        }

        /// <summary>
        /// XPLMAcquirePlanes grants your plugin exclusive access to the aircraft.  It
        /// returns <see langword="true"/> if you gain access, <see langword="false"/> if you do not.
        /// </summary>
        /// <param name="aircrafts">
        /// <para>Pass in a list of strings specifying the planes you want loaded.</para>
        /// <para>For any plane index you do not want loaded, pass an empty string.</para>
        /// <para>Other strings should be full paths with the <c>.acf</c> extension.</para>
        /// <para>Pass <see langword="null"/> if there are no planes you want loaded.</para>
        /// </param>
        /// <param name="callback">
        /// If you pass in a callback and do not receive access to the planes your
        /// callback will be called when the airplanes are available. If you do receive
        /// airplane access, your callback will not be called.
        /// </param>
        public static unsafe bool AcquireExclusiveControl(IReadOnlyList<string>? aircrafts, Action? callback = null)
        {
            bool hasAircrafts = aircrafts != null;
            Span<IntPtr> array = hasAircrafts
                ? stackalloc IntPtr[aircrafts!.Count + 1]
                : default;

            array.Clear();
            if (hasAircrafts)
            {
                for (int i = 0; i < aircrafts!.Count; i++)
                {
                    array[i] = Marshal.StringToCoTaskMemUTF8(aircrafts[i]);
                }
            }

            var pArray = (byte**) Unsafe.AsPointer(ref array.GetPinnableReference());
            bool result;
            if (callback != null)
            {
                var wrapper = new ActionWrapper(callback);
                result = PlanesAPI.AcquirePlanes(pArray, 
                    &OnPlanesAvailable,
                    GCHandle.ToIntPtr(wrapper.Handle).ToPointer()) != 0;
                if (result)
                {
                    wrapper.Dispose();
                }
                else
                {
                    PluginBase.RegisterObject(wrapper);
                }
            }
            else
            {
                result = PlanesAPI.AcquirePlanes(pArray, null, null) != 0;
            }

            if (hasAircrafts)
            {
                for (int i = aircrafts!.Count - 1; i >= 0; i--)
                {
                    Marshal.FreeCoTaskMem(array[i]);
                }
            }

            return result;

            [UnmanagedCallersOnly]
            static void OnPlanesAvailable(void* inrefcon)
            {
                Utils.TryGetObject<ActionWrapper>(inrefcon)?.Invoke();
            }
        }

        /// <summary>
        /// XPLMAcquirePlanes grants your plugin exclusive access to the aircraft.  It
        /// returns <see langword="true"/> if you gain access, <see langword="false"/> if you do not.
        /// </summary>
        /// <param name="aircrafts">
        /// <para>Pass in a list of strings specifying the planes you want loaded.</para>
        /// <para>For any plane index you do not want loaded, pass an empty string.</para>
        /// <para>Other strings should be full paths with the <c>.acf</c> extension.</para>
        /// <para>Pass <see langword="null"/> if there are no planes you want loaded.</para>
        /// </param>
        /// <param name="callback">
        /// If you pass in a callback and do not receive access to the planes your
        /// callback will be called when the airplanes are available. If you do receive
        /// airplane access, your callback will not be called.
        /// </param>
        /// <param name="refCon">
        /// Pointer to any arbitrary data to be passed into <paramref name="callback"/>.
        /// </param>
        public static unsafe bool AcquireExclusiveControl(IReadOnlyList<string>? aircrafts, delegate* unmanaged<void*, void> callback, void* refCon)
        {
            bool hasAircrafts = aircrafts != null;
            Span<IntPtr> array = hasAircrafts
                ? stackalloc IntPtr[aircrafts!.Count + 1]
                : default;

            array.Clear();
            if (hasAircrafts)
            {
                for (int i = 0; i < aircrafts!.Count; i++)
                {
                    array[i] = Marshal.StringToCoTaskMemUTF8(aircrafts[i]);
                }
            }

            var pArray = (byte**) Unsafe.AsPointer(ref array.GetPinnableReference());
            bool result = PlanesAPI.AcquirePlanes(pArray, callback, refCon) != 0;

            if (hasAircrafts)
            {
                for (int i = aircrafts!.Count - 1; i >= 0; i--)
                {
                    Marshal.FreeCoTaskMem(array[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Call this function to release access to the planes.  Note that if you are
        /// disabled, access to planes is released for you and you must reacquire it.
        /// </summary>
        public static void ReleaseExclusiveControl()
        {
            PlanesAPI.ReleasePlanes();
        }

        private sealed class ActionWrapper : DelegateWrapper<Action>
        {
            public ActionWrapper(Action @delegate) : base(@delegate)
            {
            }

            public void Invoke() => Delegate.Invoke();
        }
    }
}
