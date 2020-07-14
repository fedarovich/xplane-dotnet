using System;
using System.IO;
using System.Runtime.CompilerServices;
using AudioHelpers;
using OpenALPrivateOpenToolkit;
using OpenToolkit.Audio.OpenAL;
using XP.SDK;
using XP.SDK.XPLM;

[assembly: Plugin(typeof(Plugin))]

namespace OpenALPrivateOpenToolkit
{
    public class Plugin : PluginBase
    {
        private FlightLoop _flightLoop;
        private ALDevice _device;
        private ALContext _context;
        private int _soundSource;
        private int _soundBuffer;

        protected override bool OnStart()
        {
            _flightLoop = FlightLoop.Create(FlightLoopPhaseType.BeforeFlightModel, InitSound);
            _flightLoop.Schedule(-1, false);

            Menu.PluginsMenu
                .AddItem("OpenAL Sample (Private Context)", item =>
                    item.CreateSubMenu()
                        .AddItem("Play Sound", out _, PlaySound)
                );

            return true;
        }

        private float InitSound(float elapsedSinceLastCall, float elapsedTimeSinceLastFlightLoop, int counter)
        {
            CheckError();

            // We have to save the old context and restore it later, so that we don't interfere with X-Plane
            // and other plugins.

            var oldContext = ALC.GetCurrentContext();

            // Try to create our own default device and context.  If we fail, we're dead, we won't play any sound.
            _device = ALC.OpenDevice(null);
            if (_device == null)
            {
                XPlane.Trace.WriteLine("[OpenAL Sample] Could not open the default OpenAL device.");
                return 0;
            }

            _context = ALC.CreateContext(_device, new ALContextAttributes());
            if (_context == null)
            {
                if (oldContext != default)
                {
                    ALC.MakeContextCurrent(oldContext);
                }

                ALC.CloseDevice(_device);
                _device = default;
                XPlane.Trace.WriteLine("[OpenAL Sample] Could not create a context.");
                return 0;
            }

            // Make our context current, so that OpenAL commands affect our, um, stuff.
            ALC.MakeContextCurrent(_context);

            var path = Path.Combine(Path.GetDirectoryName(PluginInfo.ThisPlugin.FilePath), "sound.wav");

            // Generate 1 source and load a buffer of audio.
            AL.GenSource(out _soundSource);

            CheckError();

            _soundBuffer = WavHelper.LoadWav(path);
            XPlane.Trace.WriteLine($"[OpenAL Sample] Loaded {_soundBuffer} from {path}.");

            // Basic initialization code to play a sound: specify the buffer the source is playing, as well as some 
            // sound parameters. This doesn't play the sound - it's just one-time initialization.
            AL.Source(_soundSource, ALSourcei.Buffer, _soundBuffer);
            AL.Source(_soundSource, ALSourcef.Pitch, 1f);
            AL.Source(_soundSource, ALSourcef.Gain, 1f);
            AL.Source(_soundSource, ALSourceb.Looping, false);
            AL.Source(_soundSource, ALSource3f.Position, 0f, 0f, 0f);
            AL.Source(_soundSource, ALSource3f.Velocity, 0f, 0f, 0f);
            CheckError();

            // Finally: put back the old context _if_ we had one.  If old_ctx was null, X-Plane isn't using OpenAL.
            if (oldContext != default)
                ALC.MakeContextCurrent(oldContext);

            return 0;
        }

        private void PlaySound(MenuItem sender, EventArgs args)
        {
            if (_context != default && _soundSource != 0)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Playing sound ({_soundSource})");

                var oldContext = ALC.GetCurrentContext();
                ALC.MakeContextCurrent(_context);

                AL.Source(_soundSource, ALSourcef.Pitch, 1f);
                AL.SourcePlay(_soundSource);
                CheckError();

                if (oldContext != default)
                    ALC.MakeContextCurrent(oldContext);
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
            // Cleanup: nuke our context if we have it.  This is hacky and bad - we should really destroy
            // our buffers and sources.  I have _no_ idea if OpenAL will leak memory.
            if (_context != null)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Deleting my context 0x{(ulong)_context.Handle:X8}");
                ALC.DestroyContext(_context);
                _context = default;
            }

            if (_device != null)
            {
                ALC.CloseDevice(_device);
                _device = default;
            }

            _flightLoop.Dispose();
            _flightLoop = null;

            Menu.PluginsMenu.Dispose();
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
        }

        public override string Name => "OpenAL Sound Demo (Private Context)";
        public override string Signature => "com.fedarovich.xplane-dotnet.openal-private-open-toolkit";
        public override string Description => "Demonstrates sound playback with OpenAL (private context).";

        private void CheckError([CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = default)
        {
            var error = AL.GetError();
            if (error != ALError.NoError)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] ERROR: {error:D}: {error:G} ({filePath}:{lineNumber})");
            }
        }
    }
}
