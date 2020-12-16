using System;

namespace XP.SDK
{
    public readonly struct Rect : IEquatable<Rect>
    {
        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Rect(in Rect other) => this = other;
        
        public int Left { get; init; }

        public int Top { get; init; }

        public int Right { get; init; }

        public int Bottom { get; init; }

        public int Width => Right - Left;

        public int Height => Top - Bottom;

        public bool Equals(Rect other) => Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;

        public override bool Equals(object obj) => obj is Rect other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

        public static bool operator ==(Rect left, Rect right) => left.Equals(right);

        public static bool operator !=(Rect left, Rect right) => !left.Equals(right);

        public void Deconstruct(out int left, out int top, out int right, out int bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }
    }
}
