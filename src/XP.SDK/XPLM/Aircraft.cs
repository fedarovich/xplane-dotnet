using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using InlineIL;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    public static class Aircraft
    {
        public const int UserAircraft = 0;

        public static unsafe (int total, int active, PluginID controller) GetCountAndController()
        {
            (int total, int active, PluginID controller) result = default;
            PlanesAPI.CountAircraft(&result.total, &result.active, &result.controller);
            return result;
        }

        public static unsafe (string fileName, string path) GetModel(int index)
        {
            IL.DeclareLocals(false);

            Span<byte> pathBuffer = stackalloc byte[4096];
            Span<byte> fileNameBuffer = stackalloc byte[512];

            PlanesAPI.GetNthAircraftModel(
                index,
                (byte*)Unsafe.AsPointer(ref fileNameBuffer.GetPinnableReference()),
                (byte*)Unsafe.AsPointer(ref pathBuffer.GetPinnableReference()));
            var fileName = Encoding.UTF8.GetString(pathBuffer[..pathBuffer.IndexOf((byte) 0)]);
            var path = Encoding.UTF8.GetString(pathBuffer[..pathBuffer.IndexOf((byte)0)]);
            return (fileName, path);
        }

        public static void SetActiveAircraftCount(int count)
        {
            PlanesAPI.SetActiveAircraftCount(count);
        }

        public static void SetModel(int index, in ReadOnlySpan<char> aircraftPath)
        {
            PlanesAPI.SetAircraftModel(index, aircraftPath);
        }

        public static void DisableAIForPlane(int index)
        {
            PlanesAPI.DisableAIForPlane(index);
        }

        public static unsafe bool AcquireExclusiveControl(IReadOnlyList<string> aircrafts, Action callback = null)
        {
            bool hasAircrafts = aircrafts != null;
            Span<IntPtr> array = hasAircrafts
                ? stackalloc IntPtr[aircrafts.Count + 1]
                : default;

            array.Clear();
            if (hasAircrafts)
            {
                for (int i = 0; i < aircrafts.Count; i++)
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
                for (int i = aircrafts.Count - 1; i >= 0; i--)
                {
                    Marshal.FreeCoTaskMem(array[i]);
                }
            }

            return result;

            [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
            static void OnPlanesAvailable(void* inrefcon)
            {
                Utils.TryGetObject<ActionWrapper>(inrefcon)?.Invoke();
            }
        }

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
