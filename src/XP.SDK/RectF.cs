using System;
using System.Runtime.InteropServices;

namespace XP.SDK
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct RectF : IEquatable<Rect>
    {
        public RectF(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RectF(in RectF other) => this = other;
        
        public float Left { get; init; }

        public float Top { get; init; }

        public float Right { get; init; }

        public float Bottom { get; init; }

        public float Width => Right - Left;

        public float Height => Top - Bottom;

        public bool Equals(Rect other) => Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;

        public override bool Equals(object obj) => obj is Rect other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

        public static bool operator ==(RectF left, RectF right) => left.Equals(right);

        public static bool operator !=(RectF left, RectF right) => !left.Equals(right);

        public void Deconstruct(out float left, out float top, out float right, out float bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }
    }
}
