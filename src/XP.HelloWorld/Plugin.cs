using System;
using System.Runtime.CompilerServices;
using System.Text.Unicode;
using XP.SDK;
using XP.SDK.XPLM;
using XP.SDK.XPLM.Internal;

[assembly: Plugin(typeof(XP.HelloWorld.Plugin))]

namespace XP.HelloWorld
{
    public class Plugin : PluginBase
    {
        private Window _window;

        public override string Name => "Hello World";
        public override string Signature => "com.fedarovich.xplane-dotnet.hello-world";
        public override string Description => "Hello World sample plugin.";
        
        protected override unsafe bool OnStart()
        {
            int left, bottom, right, top;
            Display.GetScreenBoundsGlobal(&left, &top, &right, &bottom);
            var rect = new Rect(left + 50, bottom + 350, left + 250, bottom + 150);
            _window = new Window(rect, decoration: WindowDecoration.RoundRectangle);
            _window.DrawWindow += OnDrawWindow;
            _window.SetPositioningMode(WindowPositioningMode.PositionFree);
            _window.SetResizingLimits(200, 200, 300, 300);
            _window.Title = "Sample window.";
            return true;
        }

        private static unsafe void OnDrawWindow(object sender, EventArgs e)
        {
            Graphics.SetGraphicsState(
                0 /* no fog */,
                0 /* 0 texture units */,
                0 /* no lighting */,
                0 /* no alpha testing */,
                1 /* do alpha blend */,
                1 /* do depth testing */,
                0 /* no depth writing */);
            var rect = ((Window) sender).Geometry;
            var message = "Hello, world!";
            Graphics.DrawString(new RGBColor(1, 1, 1), rect.Left + 10, rect.Top - 20, message, null, FontID.Proportional);
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
