using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XP.SDK.XPLM;
#if SILK_NET
using Silk.NET.OpenAL;
#elif OPEN_TOOLKIT
using OpenToolkit.Audio.OpenAL;
#endif

namespace AudioHelpers
{
    public static class WavHelper
    {
        private const uint RiffId = 0x46464952;
        private const uint FmtId = 0x20746D66;
        private const uint DataId = 0x61746164;

#if SILK_NET
        public static uint LoadWav(string path, AL al)
#elif OPEN_TOOLKIT
        public static int LoadWav(string path)
#endif
        {
            try
            {
                bool swapped = false;

                // First: we open the file and copy it into a single large memory buffer for processing.
                byte[] buffer;

                using (var stream = File.OpenRead(path))
                {
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                }

                Span<byte> bufferSpan = buffer;

                // Second: find the RIFF chunk.  Note that by searching for RIFF both normal
                // and reversed, we can automatically determine the endian swap situation for
                // this file regardless of what machine we are on.

                var riff = FindChunk(bufferSpan, RiffId, false);
                if (riff.IsEmpty)
                {
                    riff = FindChunk(bufferSpan, RiffId, true);
                    if (riff.IsEmpty)
                    {
                        XPlane.Trace.WriteLine("[OpenAL Sample] Could not find RIFF chunk in wave file.");
                        return 0;
                    }

                    swapped = true;
                }

                // The wave chunk isn't really a chunk at all. :-(  It's just a "WAVE" tag 
                // followed by more chunks.  This strikes me as totally inconsistent, but
                // anyway, confirm the WAVE ID and move on.
                if (riff[0] != (byte)'W' ||
                    riff[1] != (byte)'A' ||
                    riff[2] != (byte)'V' ||
                    riff[3] != (byte)'E')
                {
                    XPlane.Trace.WriteLine("[OpenAL Sample] Could not find WAVE signature in wave file.");
                    return 0;
                }

                bufferSpan = bufferSpan.Slice(4 + (int) Unsafe.ByteOffset(ref bufferSpan[0], ref riff[0]));
                var formatChunk = FindChunk(bufferSpan, FmtId, swapped);
                if (formatChunk.IsEmpty)
                {
                    XPlane.Trace.WriteLine("[OpenAL Sample] Could not find FMT chunk in wave file.");
                }

                var format = MemoryMarshal.Read<FormatInfo>(formatChunk);
                if (swapped)
                {
                    format.Format = BinaryPrimitives.ReverseEndianness(format.Format);
                    format.ChannelCount = BinaryPrimitives.ReverseEndianness(format.ChannelCount);
                    format.SampleRate = BinaryPrimitives.ReverseEndianness(format.SampleRate);
                    format.ByteRate = BinaryPrimitives.ReverseEndianness(format.ByteRate);
                    format.BlockAlign = BinaryPrimitives.ReverseEndianness(format.BlockAlign);
                    format.BitsPerSample = BinaryPrimitives.ReverseEndianness(format.BitsPerSample);
                }

                // Reject things we don't understand...expand this code to support weirder audio formats.
                if (format.Format != 1)
                {
                    XPlane.Trace.WriteLine("[OpenAL Sample] Wave file is not PCM format data.");
                    return 0;
                }
                if (format.ChannelCount != 1 && format.ChannelCount != 2)
                {
                    XPlane.Trace.WriteLine("[OpenAL Sample] Must have mono or stereo sound.");
                    return 0;
                }
                if (format.BitsPerSample != 8 && format.BitsPerSample != 16)
                {
                    XPlane.Trace.WriteLine("[OpenAL Sample] Must have 8 or 16 bit sounds.");
                    return 0;
                }

                var data = FindChunk(bufferSpan, DataId, swapped);
                if (data.IsEmpty)
                {
                    XPlane.Trace.WriteLine("[OpenAL Sample] I could not find the DATA chunk.");
                    return 0;
                }

                var sampleSize = format.ChannelCount * format.BitsPerSample / 8;
                var dataSamples = data.Length / sampleSize;

                // If the file is swapped and we have 16-bit audio, we need to endian-swap the audio too or we'll 
                // get something that sounds just astoundingly bad!
                if (format.BitsPerSample == 16 && swapped)
                {
                    var wordSpan = MemoryMarshal.Cast<byte, ushort>(data);
                    foreach (ref uint word in wordSpan)
                    {
                        word = BinaryPrimitives.ReverseEndianness(word);
                    }
                }

                return BufferData(data);

#if SILK_NET
                unsafe uint BufferData(in ReadOnlySpan<byte> audioData)
                {
                    var bufferId = al.GenBuffer();
                    if (bufferId == 0)
                    {
                        XPlane.Trace.WriteLine("[OpenAL Sample] Could not generate buffer id.");
                        return 0;
                    }

                    fixed (void* pData = audioData)
                    {
                        al.BufferData(
                            bufferId,
                            format.BitsPerSample == 16
                                ? (format.ChannelCount == 2 ? BufferFormat.Stereo16 : BufferFormat.Mono16)
                                : (format.ChannelCount == 2 ? BufferFormat.Stereo8 : BufferFormat.Mono8),
                            pData,
                            audioData.Length,
                            format.SampleRate);
                    }

                    return bufferId;
                }
#elif OPEN_TOOLKIT
                int BufferData(in Span<byte> audioData)
                {
                    AL.GenBuffer(out var bufferId);
                    if (bufferId == 0)
                    {
                        XPlane.Trace.WriteLine("[OpenAL Sample] Could not generate buffer id.");
                        return 0;
                    }

                    AL.BufferData(
                        bufferId,
                        format.BitsPerSample == 16
                            ? (format.ChannelCount == 2 ? ALFormat.Stereo16 : ALFormat.Mono16)
                            : (format.ChannelCount == 2 ? ALFormat.Stereo8 : ALFormat.Mono8),
                        audioData,
                        format.SampleRate);

                    return bufferId;
                }
#endif
            }
            catch (Exception ex)
            {
                XPlane.Trace.WriteLine(ex.ToString());
                return 0;
            }
        }

        // Wave files are RIFF files, which are "chunky" - each section has an ID and a length.  This lets us skip
        // things we can't understand to find the parts we want.  This header is common to all RIFF chunks.
        private struct ChunkHeader
        {
            public uint Id;
            public int Size;
        }

        // WAVE file format info.  We pass this through to OpenAL so we can support mono/stereo, 8/16/bit, etc.
        private struct FormatInfo
        {
            public short Format; // PCM = 1, not sure what other values are legal.
            public short ChannelCount;
            public int SampleRate;
            public int ByteRate;
            public short BlockAlign;
            public short BitsPerSample;
        }

        private static Span<byte> FindChunk(Span<byte> data, uint desiredId, bool swapped)
        {
            while (!data.IsEmpty)
            {
                ref readonly ChunkHeader header = ref MemoryMarshal.AsRef<ChunkHeader>(data);
                if (!swapped && header.Id == desiredId)
                    return data.Slice(Unsafe.SizeOf<ChunkHeader>(), header.Size);
                if (swapped && header.Id == BinaryPrimitives.ReverseEndianness(desiredId))
                    return data.Slice(Unsafe.SizeOf<ChunkHeader>(), BinaryPrimitives.ReverseEndianness(header.Size));

                int chunkSize = swapped ? BinaryPrimitives.ReverseEndianness(header.Size) : header.Size;
                var next = chunkSize + Unsafe.SizeOf<ChunkHeader>();
                if (next >= data.Length)
                    return Span<byte>.Empty;

                data = data.Slice(next);
            }

            return Span<byte>.Empty;
        }
    }
}
