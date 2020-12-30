#nullable enable
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text;
using System.Text.Unicode;
using XP.SDK.Buffers;

namespace XP.SDK
{
    /// <summary>
    /// Provides a way to build a <see cref="Utf8String"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// You must always call <see cref="Dispose"/> on the builder if it uses the memory from <see cref="ArrayPool{T}"/> or <see cref="MemoryPool{T}"/>.
    /// The easiest way to do is to use <c>using</c> block in C# (see the example below).
    /// </para>
    /// <para>
    /// You must never use the UTF-8 string created by the builder after the builder has been disposed.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example shows how you can use <see cref="Utf8StringBuilderFactory"/>
    /// to create a <see cref="Utf8StringBuilder"/> and build a <see cref="Utf8String"/> with it.
    /// <code>
    /// <![CDATA[
    /// void SayHello(Utf8StringBuilderFactory factory)
    /// {
    ///     using Utf8StringBuilder builder = factory.CreateBuilder();
    ///     builder.Append("Hello, world!");
    ///     builder.AppendLine();
    ///     Utf8String str = builder.Complete();
    ///     XPlane.Trace.Write(str);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public readonly ref struct Utf8StringBuilder
    {
        private static readonly Encoding Utf16 = new UnicodeEncoding(false, false);

        private readonly ICompletableBufferWriter<byte> _bufferWriter;
        private readonly bool _useCrLf;

        private ICompletableBufferWriter<byte> BufferWriter => _bufferWriter ?? throw new InvalidOperationException("The Utf8StringBuilder must be created by Utf8StringBuilderFactory.");

        internal Utf8StringBuilder(ICompletableBufferWriter<byte> bufferWriter)
        {
            _bufferWriter = bufferWriter;
            _useCrLf = Environment.NewLine == "\r\n";
        }

        /// <summary>
        /// Appends a platform-dependent new line to the string.
        /// </summary>
        /// <remarks>
        /// This method will append <c>\r\n</c> on Windows and <c>\n</c> on all other platforms.
        /// </remarks>
        public void AppendLine()
        {
            if (_useCrLf)
            {
                BufferWriter.Write(new [] { (byte)'\r', (byte)'\n' });
            }
            else
            {
                BufferWriter.Write(new [] { (byte)'\n' } );
            }
        }

        #region Append

        /// <summary>
        /// Appends the string.
        /// </summary>
        public void Append(string? str)
        {
            int offset = 0;
            OperationStatus result;

            do
            {
                var source = str.AsSpan(offset);
                var dest = BufferWriter.GetSpan(source.Length);
                result = Utf8.FromUtf16(source, dest, out var read, out var written);
                BufferWriter.Advance(written);
                offset += read;
            } while (result != OperationStatus.Done);
        }

        /// <summary>
        /// Appends the UTF-8 string.
        /// </summary>
        public void Append(in Utf8String utf8String)
        {
            BufferWriter.Write(utf8String.Data.Slice(0, utf8String.Length));
        }

        /// <summary>
        /// Appends the <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(bool, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(bool value, StandardFormat format = default)
        {
            int sizeHint = 5;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="byte"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(byte, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(byte value, StandardFormat format = default)
        {
            int sizeHint = 3;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(DateTime, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(DateTime value, StandardFormat format = default)
        {
            int sizeHint = 33;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="DateTimeOffset"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(DateTimeOffset, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(DateTimeOffset value, StandardFormat format = default)
        {
            int sizeHint = 36;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="decimal"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(decimal, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(decimal value, StandardFormat format = default)
        {
            int sizeHint = 11;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="double"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(double, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(double value, StandardFormat format = default)
        {
            int sizeHint = 11;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="Guid"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(Guid, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(Guid value, StandardFormat format = default)
        {
            int sizeHint = 38;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="short"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(short, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(short value, StandardFormat format = default)
        {
            int sizeHint = 7;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="int"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(int, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(int value, StandardFormat format = default)
        {
            int sizeHint = 17;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="long"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(long, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(long value, StandardFormat format = default)
        {
            int sizeHint = 29;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="sbyte"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(sbyte, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(sbyte value, StandardFormat format = default)
        {
            int sizeHint = 4;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="float"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(float, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(float value, StandardFormat format = default)
        {
            int sizeHint = 11;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(TimeSpan, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(TimeSpan value, StandardFormat format = default)
        {
            int sizeHint = 20;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="ushort"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(ushort, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(ushort value, StandardFormat format = default)
        {
            int sizeHint = 6;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(uint, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(uint value, StandardFormat format = default)
        {
            int sizeHint = 16;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the <see cref="ulong"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(ulong, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(ulong value, StandardFormat format = default)
        {
            int sizeHint = 28;
            while (true)
            {
                if (Utf8Formatter.TryFormat(value, BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        #endregion

        /// <summary>
        /// Completes building of the UTF-8 string and returns the built string.
        /// </summary>
        /// <returns>The built UTF-8 string.</returns>
        /// <remarks>
        /// It is not possible to modify the string and the builder after this method is called.
        /// </remarks>
        public Utf8String Build()
        {
            BufferWriter.Write(new [] { (byte) 0 });
            BufferWriter.Complete();
            var utf8String = new Utf8String(BufferWriter.WrittenSpan, BufferWriter.WrittenCount - 1);
            return utf8String;
        }

        /// <summary>
        /// Gets the current representation of the string stored in the builder.
        /// </summary>
        public override string ToString()
        {
            return Utf16.GetString(BufferWriter.WrittenSpan[..^1]);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            (BufferWriter as IDisposable)?.Dispose();
        }
    }
}
