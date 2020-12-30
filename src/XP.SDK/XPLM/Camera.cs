using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using XP.SDK.XPLM.Interop;

#nullable enable

namespace XP.SDK.XPLM
{
    /// <summary>
    /// <para>
    /// The Camera APIs allow plug-ins to control the camera angle in X-Plane. This has a number of applications, including but not limited to:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>Creating new views (including dynamic/user-controllable views) for the user.</description>
    /// </item>
    /// <item>
    /// <description>Creating applications that use X-Plane as a renderer of scenery, aircrafts, or both.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The camera is controlled via six parameters: a location in OpenGL coordinates and pitch, roll and yaw, similar to an airplane’s position.
    /// OpenGL coordinate info is described in detail in the XPLMGraphics documentation; generally you should use the XPLMGraphics routines to convert from world to local coordinates.
    /// The camera’s orientation starts facing level with the ground directly up the negative-Z axis (approximately north) with the horizon horizontal.
    /// It is then rotated clockwise for yaw, pitched up for positive pitch, and rolled clockwise around the vector it is looking along for roll.
    /// </para>
    /// <para>
    /// You control the camera either until the user selects a new view or permanently (the later being similar to how UDP camera control works).
    /// You control the camera by registering a callback per frame from which you calculate the new camera positions.
    /// This guarantees smooth camera motion.
    /// </para>
    /// <para>
    /// Use the XPLMDataAccess APIs to get information like the position of the aircraft, etc. for complex camera positioning.
    /// </para>
    /// <para>
    /// Note: if your goal is to move the virtual pilot in the cockpit, this API is not needed; simply update the datarefs for the pilot’s head position.
    /// </para>
    /// <para>
    /// For custom exterior cameras, set the camera’s mode to an external view first to get correct sound and 2-d panel behavior.
    /// </para>
    /// </remarks>
    public static class Camera
    {
        /// <summary>
        /// This function repositions the camera on the next drawing cycle.
        /// You must pass a non-null <paramref name="controlAction"/>.
        /// Specify in <paramref name="duration"/> how long you’d like control (indefinitely or until a new view mode is set by the user).
        /// </summary>
        /// <param name="duration">How long you’d like control (indefinitely or until a new view mode is set by the user).</param>
        /// <param name="controlAction">Camera control action.</param>
        /// <param name="onLoosingControl">Callback called before loosing control.</param>
        /// <returns>Camera controller.</returns>
        public static unsafe Controller ControlCamera(CameraControlDuration duration,
            CameraControlAction controlAction,
            Action? onLoosingControl = null)
        {
            var controller = new Controller(
                controlAction ?? throw new ArgumentNullException(nameof(controlAction)),
                onLoosingControl);
            CameraAPI.ControlCamera(duration, &OnCameraCallback, controller.RefCon);
            return controller;

            [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
            static int OnCameraCallback(CameraPosition* outcameraposition, int inislosingcontrol, void* inrefcon)
            {
                return Utils.TryGetObject<Controller>(inrefcon)?.OnCameraControl(outcameraposition, inislosingcontrol) ?? 0;
            }
        }

        /// <summary>
        /// Gets the value indicating whether the camera is being controlled.
        /// </summary>
        public static unsafe bool IsBeingControlled => CameraAPI.IsCameraBeingControlled(null) != 0;

        /// <summary>
        /// Gets the current <see cref="CameraControlDuration"/> if the camera is being controlled; <see langword="null"/> otherwise.
        /// </summary>
        public static unsafe CameraControlDuration? CurrentControlDuration
        {
            get
            {
                CameraControlDuration duration;
                return CameraAPI.IsCameraBeingControlled(&duration) != 0 ? duration : (CameraControlDuration?) null;
            }
        }

        /// <summary>
        /// Reads the current camera position.
        /// </summary>
        /// <param name="position">The current camera position</param>
        /// <seealso cref="GetCurrentPosition"/>
        public static unsafe void ReadCurrentPosition(out CameraPosition position)
        {
            fixed (CameraPosition* pos = &position)
            {
                CameraAPI.ReadCameraPosition(pos);
            }
        }

        /// <summary>
        /// Get the current camera position.
        /// </summary>
        /// <remarks>
        /// For better performance consider using <see cref="ReadCurrentPosition" /> function.
        /// </remarks>
        public static unsafe CameraPosition GetCurrentPosition()
        {
            CameraPosition position;
            CameraAPI.ReadCameraPosition(&position);
            return position;
        }

        public sealed class Controller : IDisposable
        {
            private readonly CameraControlAction _controlAction;
            private readonly Action? _onLoosingControl;

            private volatile bool _isLoosingControl;
            private GCHandle _handle;
            private int _disposed;

            internal Controller(CameraControlAction controlAction, Action? onLoosingControl = null)
            {
                _controlAction = controlAction;
                _onLoosingControl = onLoosingControl;
                _handle = GCHandle.Alloc(this);
            }

            internal unsafe void* RefCon => GCHandle.ToIntPtr(_handle).ToPointer();

            /// <inheritdoc cref="IDisposable.Dispose"/>
            public void Dispose()
            {
                if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
                {
                    if (!_isLoosingControl)
                    {
                        CameraAPI.DontControlCamera();
                    }
                    _handle.Free();
                }
            }

            internal unsafe int OnCameraControl(CameraPosition* outcameraposition, int inislosingcontrol)
            {
                if (inislosingcontrol == 0)
                {
                    ref CameraPosition position = ref Unsafe.AsRef<CameraPosition>(outcameraposition);
                    return _controlAction(this, ref position).ToInt();
                }

                try
                {
                    _isLoosingControl = true;
                    _onLoosingControl?.Invoke();
                }
                catch (Exception ex)
                {
                    XPlane.Trace.WriteLine(ex.Message);
                }
                finally
                {
                    Dispose();
                }

                return 0;
            }
        }

        /// <summary>
        /// You use an CameraControlAction delegate to provide continuous control over the camera.
        /// You are passed in a structure in which to put the new camera position;
        /// modify it and return <see langword="true"/> to reposition the camera.
        /// Return <see langword="false"/> to surrender control of the camera; camera control will be handled by X-Plane on this draw loop.
        /// The contents of the structure as you are called are undefined.
        /// </summary>
        public delegate bool CameraControlAction(Controller controller, ref CameraPosition position);
    }
}
