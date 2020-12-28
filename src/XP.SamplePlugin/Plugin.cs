using System;
using XP.SDK;
using XP.SDK.XPLM;

[assembly: Plugin(typeof(XP.SamplePlugin.Plugin))]

namespace XP.SamplePlugin
{

    public partial class Plugin : PluginBase
    {
        [Utf8StringLiteral("Sample")]
        public sealed override partial Utf8String GetName();

        public override Utf8String GetSignature() => Utf8String.FromString("com.fedarovich.xplane-dotnet.sample");

        public override Utf8String GetDescription()
        {
            return Utf8String.FromString("Sample plugin.");
        }

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
