using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using InlineIL;
using Microsoft.VisualBasic.CompilerServices;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public static class Aircraft
    {
        private static readonly PlanesAvailableCallback _planesAvailable;
        private static readonly ConditionalWeakTable<PluginBase, HashSet<ActionWrapper>> _actionList = new ConditionalWeakTable<PluginBase, HashSet<ActionWrapper>>();

        public const int UserAircraft = 0;

        static unsafe Aircraft()
        {
            _planesAvailable = OnPlanesAvailable;

            static void OnPlanesAvailable(void* inrefcon)
            {
                Utils.TryGetObject<ActionWrapper>(inrefcon)?.Invoke();
            }
        }

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
            if (callback != null && GlobalContext.CurrentPlugin.TryGetTarget(out var plugin))
            {
                var wrapper = new ActionWrapper(callback);
                var wrappers = _actionList.GetOrCreateValue(plugin);
                wrappers.Add(wrapper);

                result = PlanesAPI.AcquirePlanes(pArray, 
                    _planesAvailable,
                    GCHandle.ToIntPtr(wrapper.Handle).ToPointer()) != 0;
                if (result)
                {
                    wrapper.Dispose();
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
        }

        public static void ReleaseExclusiveControl()
        {
            PlanesAPI.ReleasePlanes();
        }

        private sealed class ActionWrapper : IDisposable, IEquatable<ActionWrapper>
        {
            private Action _action;
            private int _disposed;
            public GCHandle Handle;

            public ActionWrapper(Action action)
            {
                _action = action;
                Handle = GCHandle.Alloc(this, GCHandleType.Weak);
            }

            ~ActionWrapper()
            {
                Dispose(false);
            }

            public void Invoke() => _action.Invoke();

            private void Dispose(bool disposing)
            {
                if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
                {
                    if (GlobalContext.CurrentPlugin.TryGetTarget(out var plugin))
                    {
                        var wrappers = _actionList.GetOrCreateValue(plugin);
                        wrappers.Remove(this);
                    }

                    Handle.Free();
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
                Dispose(true);
            }

            public bool Equals(ActionWrapper other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Handle.Equals(other.Handle);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is ActionWrapper other && Equals(other);
            }

            public override int GetHashCode()
            {
                return Handle.GetHashCode();
            }
        }
    }
}
