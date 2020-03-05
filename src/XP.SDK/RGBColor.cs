using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK
{
    public readonly struct RGBColor : IEquatable<RGBColor>
    {
        public RGBColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public readonly float R;
        public readonly float G;
        public readonly float B;

        public bool Equals(RGBColor other) => R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);

        public override bool Equals(object obj) => obj is RGBColor other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(R, G, B);

        public static bool operator ==(RGBColor left, RGBColor right) => left.Equals(right);

        public static bool operator !=(RGBColor left, RGBColor right) => !left.Equals(right);
    }
}
