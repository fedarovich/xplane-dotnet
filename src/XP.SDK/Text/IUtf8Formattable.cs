using System;
using System.Buffers;

namespace XP.SDK.Text
{
    public interface IUtf8Formattable
    {
        bool TryFormat(in Span<byte> destination, out int bytesWritten, StandardFormat format);

        int GetSizeHint(StandardFormat format) => 32;
    }
}
