using System;
using System.Runtime.InteropServices;
using OpenToolkit;
using OpenToolkit.Graphics.OpenGL;
using WindowSample;
using XP.SDK;
using XP.SDK.XPLM;
using Window = XP.SDK.XPLM.Window;

#pragma warning disable 618

[assembly: Plugin(typeof(Plugin))]

namespace WindowSample
{
    public class Plugin : PluginBase
    {
        private Window _window;
        private RectF _popButtonRect;
        private RectF _positionButtonRect;

        protected override bool OnStart()
        {
            try
            {
                GL.LoadBindings(new XPlaneBindingsContext());
            }
            catch (Exception ex)
            {
                XPlane.Trace.WriteLine(ex.ToString());
                return false;
            }

            var desktopBounds = WindowBase.ScreenBoundsGlobal;
            _window = new Window(
                new Rect(desktopBounds.Left + 50, desktopBounds.Bottom + 450, desktopBounds.Left + 350, desktopBounds.Bottom + 150),
                decoration: WindowDecoration.RoundRectangle);
            _window.SetPositioningMode(WindowPositioningMode.PositionFree);
            _window.SetGravity(0, 1, 0, 1);
            _window.SetResizingLimits(200, 200, 500, 500);
            _window.Title = "Sample Window";
            _window.DrawWindow += OnDrawWindow;
            _window.MouseLeftButtonEvent += OnMouseLeftButtonEvent;

            return true;
        }

        private void OnDrawWindow(Window window, EventArgs args)
        {
            var white = new RGBColor(1, 1, 1);

            Graphics.SetGraphicsState();

            // We draw our rudimentary button boxes based on the height of the button text
            var (_, charHeight, _) = Graphics.GetFontDimensions(FontID.Proportional);


            bool isPoppedOut = window.IsPoppedOut;
            var popLabel = isPoppedOut ? "Pop In" : "Pop Out";

            // We'll change the text of the pop-in/pop-out button based on our current state
            var (l, t, r, b) = window.Geometry;

            // Position the pop-in/pop-out button in the upper left of the window (sized to fit the text)
            _popButtonRect = new RectF(
                l + 10,
                t - 15,
                l + 10 + Graphics.MeasureString(FontID.Proportional, popLabel), // *just* wide enough to fit the button text
                t - 15 - (float)(1.25 * charHeight)); // a bit taller than the button text

            // Position the "move to lower left" button just to the right of the pop-in/pop-out button
            const string positionButtonText = "Move to Lower Left";
            _positionButtonRect = new RectF(
                _popButtonRect.Right + 30,
                _popButtonRect.Top,
                _popButtonRect.Right + 30 + Graphics.MeasureString(FontID.Proportional, positionButtonText),
                _popButtonRect.Bottom);

            // Draw our buttons
            {

                GL.Color4(0f, 1f, 0f, 1f);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex2(_popButtonRect.Left, _popButtonRect.Top);
                    GL.Vertex2(_popButtonRect.Right, _popButtonRect.Top);
                    GL.Vertex2(_popButtonRect.Right, _popButtonRect.Bottom);
                    GL.Vertex2(_popButtonRect.Left, _popButtonRect.Bottom);
                }
                GL.End();
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex2(_positionButtonRect.Left, _positionButtonRect.Top);
                    GL.Vertex2(_positionButtonRect.Right, _positionButtonRect.Top);
                    GL.Vertex2(_positionButtonRect.Right, _positionButtonRect.Bottom);
                    GL.Vertex2(_positionButtonRect.Left, _positionButtonRect.Bottom);
                }
                GL.End();

                // Draw the button text (pop in/pop out)
                Graphics.DrawString(white, (int)_popButtonRect.Left, (int)_popButtonRect.Bottom + 4, popLabel, FontID.Proportional);

                // Draw the button text (reposition)
                Graphics.DrawString(white, (int)_positionButtonRect.Left, (int)_positionButtonRect.Bottom + 4, positionButtonText, FontID.Proportional);
            }

            // Draw a bunch of informative text
            {
                // Set the y position for the first bunch of text we'll draw to a little below the buttons
                int y = (int)_popButtonRect.Bottom - 2 * charHeight;

                // Display the total global desktop bounds
                var desktopBounds = WindowBase.ScreenBoundsGlobal;
                Graphics.DrawString(white, l, y,
                    $"Global desktop bounds: ({desktopBounds.Left}, {desktopBounds.Bottom}) to ({desktopBounds.Right}, {desktopBounds.Top})",
                    FontID.Proportional);

                y -= (int)(1.5 * charHeight);

                // Display our bounds
                var windowBounds = _window.Geometry;
                if (_window.IsPoppedOut)
                {
                    Graphics.DrawString(white, l, y,
                        $"OS Bounds: ({windowBounds.Left}, {windowBounds.Bottom}) to ({windowBounds.Right}, {windowBounds.Top})",
                        FontID.Proportional);
                    y -= (int)(1.5 * charHeight);
                }
                else
                {
                    Graphics.DrawString(white, l, y,
                        $"Window Bounds: ({windowBounds.Left}, {windowBounds.Bottom}) to ({windowBounds.Right}, {windowBounds.Top})",
                        FontID.Proportional);
                    y -= (int)(1.5 * charHeight);
                }

                // Display whether we're in front of our our layer
                {
                    Graphics.DrawString(white, l, y,
                        $"In front? {_window.IsInFront}",
                        FontID.Proportional);
                    y -= (int)(1.5 * charHeight);
                }

                // Display the mouse's position info text
                {
                    var (mouseX, mouseY) = WindowBase.MouseLocationGlobal;
                    Graphics.DrawString(white, l, y,
                        $"Draw mouse (global): {mouseX} {mouseY}\n",
                        FontID.Proportional);
                    y -= (int)(1.5 * charHeight);
                }
            }
        }

        private void OnMouseLeftButtonEvent(Window window, ref MouseButtonEventArgs args)
        {
            if (args.MouseStatus == MouseStatus.Down)
            {
                bool isPoppedOut = window.IsPoppedOut;
                if (!_window.IsInFront)
                {
                    _window.BringToFront();
                }
                else if (CoordInRect(args.X, args.Y, _popButtonRect)) // user clicked the pop-in/pop-out button
                {
                    _window.SetPositioningMode(isPoppedOut ? WindowPositioningMode.PositionFree : WindowPositioningMode.PopOut, 0);
                }
                else if (CoordInRect(args.X, args.Y, _positionButtonRect)) // user clicked the "move to lower left" button
                {
                    // If we're popped out, and the user hits the "move to lower left" button,
                    // we need to move them to the lower left of their OS's desktop space (units are pixels).
                    // On the other hand, if we're a floating window inside of X-Plane, we need
                    // to move to the lower left of the X-Plane global desktop (units are boxels).
                    var geometry = _window.Geometry;
                    var bounds = isPoppedOut ? WindowBase.ScreenBoundsGlobal : WindowBase.AllMonitorBoundsOS[0];
                    _window.Geometry = new Rect(
                        bounds.Left,
                        bounds.Bottom + geometry.Height,
                        bounds.Left + geometry.Width,
                        bounds.Bottom);
                }
            }
        }

        protected override bool OnEnable()
        {
            return true;
        }

        protected override void OnDisable()
        {
        }

        protected override void OnStop()
        {
            _window?.Dispose();
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
        }

        private static bool CoordInRect(float x, float y, in RectF rect) => x >= rect.Left && x < rect.Right && y < rect.Top && y >= rect.Bottom;

        public override string Name => "WindowSamplePlugin";
        public override string Signature => "com.fedarovich.xplane-dotnet.window-sample";
        public override string Description => "A test plug-in that demonstrates the X-Plane 11 GUI plugin API.";

        // TODO: Move this class to a separate package.
        private class XPlaneBindingsContext : IBindingsContext
        {
            private readonly IntPtr _opengl;

            public XPlaneBindingsContext()
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    _opengl = NativeLibrary.Load("opengl32.dll");
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    _opengl = NativeLibrary.Load("libGL.so");
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    _opengl = NativeLibrary.Load("/System/Library/Frameworks/OpenGL.framework/Libraries/libGL.dylib");
                else
                    throw new PlatformNotSupportedException();
            }

            public IntPtr GetProcAddress(string procName)
            {
                NativeLibrary.TryGetExport(_opengl, procName, out var address);
                return address;
            }
        }
    }
}
