using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using AudioHelpers;
using OpenALSharedOpenToolkit;
using OpenToolkit.Audio.OpenAL;
using XP.SDK;
using XP.SDK.XPLM;

[assembly: Plugin(typeof(Plugin))]

namespace OpenALSharedOpenToolkit
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
                .AddItem("OpenAL Sample (Shared Context)", item =>
                    item.CreateSubMenu()
                        .AddItem("Play Sound", out _, PlaySound)
                );

            return true;
        }

        private unsafe float InitSound(float elapsedSinceLastCall, float elapsedTimeSinceLastFlightLoop, int counter)
        {
            CheckError();

            var oldContext = ALC.GetCurrentContext();
            if (oldContext == default)
            {
                XPlane.Trace.WriteLine("[OpenAL Sample] I found no OpenAL, I will be the first to init.");
                _device = ALC.OpenDevice(null);
                if (_device == null)
                {
                    XPlane.Trace.WriteLine("[OpenAL Sample] Could not open the default OpenAL device.");
                    return 0;
                }

                _context = ALC.CreateContext(_device, new ALContextAttributes());
                if (_context == null)
                {
                    ALC.CloseDevice(_device);
                    _device = default;
                    XPlane.Trace.WriteLine("[OpenAL Sample] Could not open the default OpenAL device.");
                    return 0;
                }

                ALC.MakeContextCurrent(_context);
                XPlane.Trace.WriteLine("[OpenAL Sample] Created the Open AL context.");

                var hardware = ALC.GetString(_device, AlcGetString.DeviceSpecifier);
                var extensions = ALC.GetString(_device, AlcGetString.Extensions);
                var major = ALC.GetInteger(_device, AlcGetInteger.MajorVersion);
                var minor = ALC.GetInteger(_device, AlcGetInteger.MinorVersion);
                XPlane.Trace.WriteLine($"[OpenAL Sample] OpenAL version   : {major}.{minor}");
                XPlane.Trace.WriteLine($"[OpenAL Sample] OpenAL hardware  : {hardware}");
                XPlane.Trace.WriteLine($"[OpenAL Sample] OpenAL extensions: {extensions}");
                CheckError();
            }
            else
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] I found someone else's context: {(ulong)oldContext.Handle:X8}");
            }

            var path = Path.Combine(Path.GetDirectoryName(PluginInfo.ThisPlugin.FilePath), "sound.wav");

            // Generate 1 source and load a buffer of audio.
            _soundSource = AL.GenSource();

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

            return 0;
        }

        private void PlaySound(MenuItem sender, EventArgs args)
        {
            if (_soundSource != 0)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Playing sound ({_soundSource})");
                AL.Source(_soundSource, ALSourcef.Pitch, 1f);
                AL.SourcePlay(_soundSource);
                CheckError();
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
            if (ALC.GetCurrentContext() != default)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Deleting snd {_soundBuffer}");
                if (_soundSource != 0)
                {
                    AL.DeleteSource(_soundSource);
                    _soundSource = 0;
                }

                if (_soundBuffer != 0)
                {
                    AL.DeleteBuffer(_soundBuffer);
                    _soundBuffer = 0;
                }
            }

            if (_context != default)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Deleting my context 0x{(ulong)_context.Handle:X8}");
                ALC.MakeContextCurrent(default);
                ALC.DestroyContext(_context);
            }

            if (_device != default)
            {
                ALC.CloseDevice(_device);
            }

            _flightLoop.Dispose();
            _flightLoop = null;

            Menu.PluginsMenu.Dispose();
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
        }

        public override string Name => "OpenAL Sound Demo (Shared Context)";
        public override string Signature => "com.fedarovich.xplane-dotnet.openal-shared-open-toolkit";
        public override string Description => "Demonstrates sound playback with OpenAL (shared context).";

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
