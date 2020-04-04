using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace XP.SDK
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct RectF : IEquatable<Rect>
    {
        private readonly float _left;
        private readonly float _top;
        private readonly float _right;
        private readonly float _bottom;

        public RectF(float left, float top, float right, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public float Left => _left;

        public float Top => _top;

        public float Right => _right;

        public float Bottom => _bottom;

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
