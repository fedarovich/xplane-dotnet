using System;
using System.Collections.Generic;
using System.Text;

namespace XP.SDK
{
    public readonly struct Size : IEquatable<Size>
    {
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        public bool Equals(Size other) => Width == other.Width && Height == other.Height;

        public override bool Equals(object obj) => obj is Size other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Width, Height);

        public static bool operator ==(Size left, Size right) => left.Equals(right);

        public static bool operator !=(Size left, Size right) => !left.Equals(right);
    }
}
