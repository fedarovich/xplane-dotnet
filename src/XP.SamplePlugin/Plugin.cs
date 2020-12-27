using System;
using XP.SDK;
using XP.SDK.XPLM;

[assembly: Plugin(typeof(XP.SamplePlugin.Plugin))]

namespace XP.SamplePlugin
{

    public partial class Plugin : PluginBase
    {
        public override string Name => "Sample";
        public override string Signature => "com.fedarovich.xplane-dotnet.sample";
        public override string Description => "Sample plugin.";

        [Utf8StringLiteral("Start sample plugin.")]
        public static partial Utf8String StartMessage();

        protected override bool OnStart()
        {
            XPlane.Trace.WriteLine("Start sample plugin.");
            return true;
        }

        protected override bool OnEnable()
        {
            XPlane.Trace.WriteLine("Enable sample plugin.");
            return true;
        }

        protected override void OnDisable()
        {
            XPlane.Trace.WriteLine("Disable sample plugin.");
        }

        protected override void OnStop()
        {
            XPlane.Trace.WriteLine("Stop sample plugin.");
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
            XPlane.Trace.WriteLine($"Received message {message} from plugin {pluginId} with payload 0x{param.ToInt64():X8}.");
        }
    }
}
