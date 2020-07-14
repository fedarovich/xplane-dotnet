using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using AudioHelpers;
using OpenALSharedSilkNet;
using Silk.NET.OpenAL;
using XP.SDK;
using XP.SDK.XPLM;

[assembly: Plugin(typeof(Plugin))]

namespace OpenALSharedSilkNet
{
    public class Plugin : PluginBase
    {
        private FlightLoop _flightLoop;
        private AL _al;
        private ALContext _alc;
        private unsafe Device* _device;
        private unsafe Context* _context;
        private uint _soundSource;
        private uint _soundBuffer;

        protected override bool OnStart()
        {
            try
            {
                _al = AL.GetApi();
                _alc = ALContext.GetApi();
            }
            catch (Exception ex)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Failed to initialize Open AL: " + ex);
                return false;
            }

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

            var oldContext = _alc.GetCurrentContext();
            if (oldContext == null)
            {
                XPlane.Trace.WriteLine("[OpenAL Sample] I found no OpenAL, I will be the first to init.");
                _device = _alc.OpenDevice(null);
                if (_device == null)
                {
                    XPlane.Trace.WriteLine("[OpenAL Sample] Could not open the default OpenAL device.");
                    return 0;
                }

                _context = _alc.CreateContext(_device, null);
                if (_context == null)
                {
                    _alc.CloseDevice(_device);
                    _device = null;
                    XPlane.Trace.WriteLine("[OpenAL Sample] Could not open the default OpenAL device.");
                    return 0;
                }

                _alc.MakeContextCurrent(_context);
                XPlane.Trace.WriteLine("[OpenAL Sample] Created the Open AL context.");

                var hardware = _alc.GetContextProperty(_device, GetContextString.DeviceSpecifier);
                var extensions = _alc.GetContextProperty(_device, GetContextString.Extensions);
                int major, minor;
                _alc.GetContextProperty(_device, GetContextInteger.MajorVersion, 1, &major);
                _alc.GetContextProperty(_device, GetContextInteger.MinorVersion, 1, &minor);
                XPlane.Trace.WriteLine($"[OpenAL Sample] OpenAL version   : {major}.{minor}");
                XPlane.Trace.WriteLine($"[OpenAL Sample] OpenAL hardware  : {hardware}");
                XPlane.Trace.WriteLine($"[OpenAL Sample] OpenAL extensions: {extensions}");
                CheckError();
            }
            else
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] I found someone else's context: {(ulong)oldContext:X8}");
            }

            var path = Path.Combine(Path.GetDirectoryName(PluginInfo.ThisPlugin.FilePath), "sound.wav");

            // Generate 1 source and load a buffer of audio.
            _soundSource = _al.GenSource();

            CheckError();

            _soundBuffer = WavHelper.LoadWav(path, _al);
            XPlane.Trace.WriteLine($"[OpenAL Sample] Loaded {_soundBuffer} from {path}.");

            // Basic initialization code to play a sound: specify the buffer the source is playing, as well as some 
            // sound parameters. This doesn't play the sound - it's just one-time initialization.
            _al.SetSourceProperty(_soundSource, SourceInteger.Buffer, _soundBuffer);
            _al.SetSourceProperty(_soundSource, SourceFloat.Pitch, 1f);
            _al.SetSourceProperty(_soundSource, SourceFloat.Gain, 1f);
            _al.SetSourceProperty(_soundSource, SourceBoolean.Looping, false);
            _al.SetSourceProperty(_soundSource, SourceVector3.Position, Vector3.Zero);
            _al.SetSourceProperty(_soundSource, SourceVector3.Velocity, Vector3.Zero);
            CheckError();

            return 0;
        }

        private void PlaySound(MenuItem sender, EventArgs args)
        {
            if (_soundSource != 0)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Playing sound ({_soundSource})");
                _al.SetSourceProperty(_soundSource, SourceFloat.Pitch, 1f);
                _al.SourcePlay(_soundSource);
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

        protected override unsafe void OnStop()
        {
            // Cleanup: nuke our context if we have it.  This is hacky and bad - we should really destroy
            // our buffers and sources.  I have _no_ idea if OpenAL will leak memory.
            if (_alc.GetCurrentContext() != null)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Deleting snd {_soundBuffer}");
                if (_soundSource != 0)
                {
                    _al.DeleteSource(_soundSource);
                    _soundSource = 0;
                }

                if (_soundBuffer != 0)
                {
                    _al.DeleteBuffer(_soundBuffer);
                    _soundBuffer = 0;
                }
            }

            if (_context != null)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] Deleting my context 0x{((ulong)_context):X8}");
                _alc.MakeContextCurrent(null);
                _alc.DestroyContext(_context);
            }

            if (_device != null)
            {
                _alc.CloseDevice(_device);
            }

            _flightLoop.Dispose();
            _flightLoop = null;

            Menu.PluginsMenu.Dispose();
        }

        protected override void OnReceiveMessage(PluginID pluginId, int message, IntPtr param)
        {
        }

        public override string Name => "OpenAL Sound Demo (Shared Context)";
        public override string Signature => "com.fedarovich.xplane-dotnet.openal-shared-silk-net";
        public override string Description => "Demonstrates sound playback with OpenAL (shared context).";

        private void CheckError([CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = default)
        {
            var error = _al.GetError();
            if (error != AudioError.NoError)
            {
                XPlane.Trace.WriteLine($"[OpenAL Sample] ERROR: {error:D}: {error:G} ({filePath}:{lineNumber})");
            }
        }
    }
}
