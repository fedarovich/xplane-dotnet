#nullable enable
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;
using XP.SDK.Text.Buffers;
using XP.SDK.Text.Formatters;

namespace XP.SDK.Text
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
    ///     Utf8String str = builder.Build();
    ///     XPlane.Trace.Write(str);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public readonly ref struct Utf8StringBuilder
    {
        private static readonly Encoding Utf16 = new UnicodeEncoding(false, false);

        private readonly ICompletableBufferWriter<byte> _bufferWriter;

        private ICompletableBufferWriter<byte> BufferWriter => _bufferWriter ?? throw new InvalidOperationException("The Utf8StringBuilder must be created by Utf8StringBuilderFactory.");

        private static ReadOnlySpan<byte> WindowsNewLine => new byte[] {(byte) '\r', (byte) '\n'};

        private static ReadOnlySpan<byte> UnixNewLine => new byte[] { (byte)'\n' };

        /// <summary>
        /// Gets the value indicating whether the underlying buffer is scoped, and so the constructed string must be created using <see cref="BuildScoped"/> method.
        /// </summary>
        public bool RequiresScope => _bufferWriter is IDisposable;
        
        internal Utf8StringBuilder(ICompletableBufferWriter<byte> bufferWriter)
        {
            _bufferWriter = bufferWriter;
        }

        static Utf8StringBuilder()
        {
        }

        /// <summary>
        /// Appends a platform-dependent new line to the string.
        /// </summary>
        /// <remarks>
        /// This method will append <c>\r\n</c> on Windows and <c>\n</c> on all other platforms.
        /// </remarks>
        public void AppendLine()
        {
            if (OperatingSystem.IsWindows())
            {
                BufferWriter.Write(WindowsNewLine);
            }
            else
            {
                BufferWriter.Write(UnixNewLine);
            }
        }

        /// <summary>
        /// Appends a new line to the string.
        /// </summary>
        public void AppendLine(NewLineSequence newLine)
        {
            switch (newLine)
            {
                case NewLineSequence.Auto:
                    if (OperatingSystem.IsWindows())
                    {
                        BufferWriter.Write(WindowsNewLine);
                    }
                    else
                    {
                        BufferWriter.Write(UnixNewLine);
                    }
                    break;
                case NewLineSequence.Windows:
                    BufferWriter.Write(WindowsNewLine);
                    break;
                case NewLineSequence.Unix:
                    BufferWriter.Write(UnixNewLine);
                    break;
            }
        }

        #region Append

        /// <summary>
        /// Appends the string.
        /// </summary>
        public void Append(string? str) => Append(str.AsSpan());

        /// <summary>
        /// Appends the substring of the string.
        /// </summary>
        public void Append(string? str, int start) => Append(str.AsSpan(start));

        /// <summary>
        /// Appends the substring of the string.
        /// </summary>
        public void Append(string? str, int start, int length) => Append(str.AsSpan(start, length));

        /// <summary>
        /// Appends the substring of the string.
        /// </summary>
        public void Append(string? str, Range range) => Append(str.AsSpan()[range]);

        /// <summary>
        /// Appends the UTF-16 string.
        /// </summary>
        /// <param name="str"></param>
        public void Append(in ReadOnlySpan<char> str)
        {
            int offset = 0;
            OperationStatus result;

            do
            {
                var source = str.Slice(offset);
                var dest = BufferWriter.GetSpan(source.Length);
                result = Utf8.FromUtf16(source, dest, out var read, out var written);
                BufferWriter.Advance(written);
                offset += read;
            } while (result != OperationStatus.Done);
        }

        /// <summary>
        /// Appends the UTF-8 string.
        /// </summary>
        public void Append(in Utf8String utf8String) => Append(utf8String.Span[..utf8String.Length]);

        /// <summary>
        /// Appends the UTF-8 substring.
        /// </summary>
        public void Append(in Utf8String utf8String, int start) => Append(utf8String.Span[start..utf8String.Length]);

        /// <summary>
        /// Appends the UTF-8 substring.
        /// </summary>
        public void Append(in Utf8String utf8String, int start, int length) => Append(utf8String.Span[..utf8String.Length].Slice(start, length));

        /// <summary>
        /// Appends the UTF-8 substring.
        /// </summary>
        public void Append(in Utf8String utf8String, Range range) => Append(utf8String.Span[..utf8String.Length][range]);

        /// <summary>
        /// Appends the UTF-8 substring.
        /// </summary>
        public void Append(in ReadOnlySpan<byte> utf8)
        {
            BufferWriter.Write(utf8);
        }

        /// <summary>
        /// Appends the <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(bool,System.Span{byte},out int,System.Buffers.StandardFormat)"/> for the available formats.</param>
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
        /// Appends the <see cref="char"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(byte, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        [SkipLocalsInit]
        public void Append(char value, StandardFormat format = default)
        {
            ReadOnlySpan<char> chars = stackalloc char[] {value};
            Span<byte> bytes = stackalloc byte[4];
            Utf8.FromUtf16(chars, bytes, out _, out var len);
            BufferWriter.Write(bytes.Slice(0, len));
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
        /// Appends the <see cref="IntPtr"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(long, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(nint value, StandardFormat format = default)
        {
            if (IntPtr.Size == 8)
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
            else
            {
                int sizeHint = 17;
                while (true)
                {
                    if (Utf8Formatter.TryFormat((int) value, BufferWriter.GetSpan(sizeHint), out var written, format))
                    {
                        BufferWriter.Advance(written);
                        break;
                    }

                    sizeHint *= 2;
                }
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

        /// <summary>
        /// Appends the <see cref="UIntPtr"/> value.
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <param name="format">The format. See <seealso cref="Utf8Formatter.TryFormat(ulong, Span{byte}, out int, StandardFormat)"/> for the available formats.</param>
        public void Append(nuint value, StandardFormat format = default)
        {
            if (IntPtr.Size == 8)
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
            else
            {
                int sizeHint = 16;
                while (true)
                {
                    if (Utf8Formatter.TryFormat((uint) value, BufferWriter.GetSpan(sizeHint), out var written, format))
                    {
                        BufferWriter.Advance(written);
                        break;
                    }

                    sizeHint *= 2;
                }
            }
        }

        /// <summary>
        /// Appends the value of type implementing <see cref="IUtf8Formattable"/> interface.
        /// </summary>
        /// <typeparam name="T">The formattable type.</typeparam>
        /// <param name="value">The formattable value.</param>
        /// <param name="format">The format.</param>
        public void Append<T>(T value, StandardFormat format = default)
            where T : IUtf8Formattable
        {
            int sizeHint = value.GetSizeHint(format);
            
            while (true)
            {
                if (value.TryFormat(BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        /// <summary>
        /// Appends the value of type implementing <see cref="IUtf8Formattable"/> interface.
        /// </summary>
        /// <typeparam name="T">The formattable type.</typeparam>
        /// <param name="value">The formattable value.</param>
        /// <param name="format">The format.</param>
        public void Append<T>(in T value, StandardFormat format = default)
            where T : IUtf8Formattable
        {
            int sizeHint = value.GetSizeHint(format);

            while (true)
            {
                if (value.TryFormat(BufferWriter.GetSpan(sizeHint), out var written, format))
                {
                    BufferWriter.Advance(written);
                    break;
                }

                sizeHint *= 2;
            }
        }

        internal void AppendRef<T>(ref T value, StandardFormat format = default)
            where T : IUtf8Formattable
        {
            int sizeHint = value.GetSizeHint(format);

            while (true)
            {
                if (value.TryFormat(BufferWriter.GetSpan(sizeHint), out var written, format))
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
        /// <exception cref="InvalidOperationException">The underlying buffer is disposable. <see cref="BuildScoped"/> method must be used instead.</exception>
        /// <seealso cref="BuildScoped"/>
        /// <seealso cref="RequiresScope"/>
        public Utf8String Build()
        {
            if (RequiresScope)
                throw new InvalidOperationException("The underlying buffer is disposable, so BuildScoped method must be used.");

            return BuildUnsafe();
        }

        /// <summary>
        /// Completes building of the UTF-8 string and returns the disposable scope containing the built string.
        /// </summary>
        /// <returns>The the disposable scope containing the built UTF-8 string.</returns>
        /// <remarks>
        /// It is not possible to modify the string and the builder after this method is called.
        /// </remarks>
        /// <seealso cref="Build"/>
        /// <seealso cref="RequiresScope"/>
        public Utf8StringScope BuildScoped()
        {
            var utf8String = BuildUnsafe();
            return new Utf8StringScope(utf8String, BufferWriter as IDisposable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Utf8String BuildUnsafe()
        {
            BufferWriter.Write(new[] { (byte)0 });
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
