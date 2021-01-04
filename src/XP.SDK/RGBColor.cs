using System;
using System.Buffers;
using System.Buffers.Text;
using XP.SDK.Text;

namespace XP.SDK
{
    public readonly struct RGBColor : IEquatable<RGBColor>, IUtf8Formattable
    {
        public RGBColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public float R { get; init; }
        public float G { get; init; }
        public float B { get; init; }

        public bool Equals(RGBColor other) => R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);

        public override bool Equals(object obj) => obj is RGBColor other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(R, G, B);

        public static bool operator ==(RGBColor left, RGBColor right) => left.Equals(right);

        public static bool operator !=(RGBColor left, RGBColor right) => !left.Equals(right);

        public static implicit operator RGBColor((float r, float g, float b) tuple) => new RGBColor(tuple.r, tuple.g, tuple.b);

        public void Deconstruct(out float r, out float g, out float b)
        {
            r = R;
            g = G;
            b = B;
        }

        public bool TryFormat(in Span<byte> destination, out int bytesWritten, StandardFormat format)
        {
            bytesWritten = 0;
            if (destination.Length < 7)
                return false;

            destination[0] = (byte) '#';
            Utf8Formatter.TryFormat(R, destination[1..], out _, new StandardFormat('X', 2));
            Utf8Formatter.TryFormat(G, destination[1..], out _, new StandardFormat('X', 2));
            Utf8Formatter.TryFormat(B, destination[1..], out _, new StandardFormat('X', 2));
            bytesWritten = 7;
            return false;
        }
    }
}
