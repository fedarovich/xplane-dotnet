using System;
using HelloWorld;
using XP.SDK;
using XP.SDK.XPLM;

[assembly: Plugin(typeof(Plugin))]

namespace HelloWorld
{
    public class Plugin : PluginBase
    {
        private Window _window;

        public override string Name => "Hello World";
        public override string Signature => "com.fedarovich.xplane-dotnet.hello-world";
        public override string Description => "Hello World sample plugin.";
        
        protected override bool OnStart()
        {
            var (left, top, right, bottom) = Screen.BoundsGlobal;
            var rect = new Rect(left + 50, bottom + 350, left + 250, bottom + 150);
            _window = new Window(rect, decoration: WindowDecoration.RoundRectangle);
            _window.DrawWindow += OnDrawWindow;
            _window.SetPositioningMode(WindowPositioningMode.PositionFree);
            _window.SetResizingLimits(200, 200, 300, 300);
            _window.Title = "Sample window.";
            return true;
        }

        private static void OnDrawWindow(Window sender, EventArgs e)
        {
            Graphics.SetGraphicsState(0);
            var rect = sender.Geometry;
            var message = "Hello, world!";
            Graphics.DrawString(new RGBColor(1, 1, 1), rect.Left + 10, rect.Top - 20, message, FontID.Proportional);
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
            _window.Dispose();
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
        }
    }
}
