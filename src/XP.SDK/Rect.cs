using System;
using System.Collections.Generic;
using System.Text;

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

        public static Rect FromSize(int left, int top, int width, int height) => new Rect(left, top, left + width, top + height);

        public static Rect FromSize(int left, int top, in Size size) => new Rect(left, top, left + size.Width, top + size.Height);

        public int Left { get; }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public int Width => Right - Left;

        public int Height => Bottom - Top;

        public bool Equals(Rect other) => Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;

        public override bool Equals(object obj) => obj is Rect other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

        public static bool operator ==(Rect left, Rect right) => left.Equals(right);

        public static bool operator !=(Rect left, Rect right) => !left.Equals(right);
    }
}
