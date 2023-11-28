using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AdventCodeExtension.Models
{
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(Point point) => Move(point.X, point.Y);
        public void Move(int x, int y)
        {
            X += x;
            Y += y;
        }

        public void Rotate(Point center, PointRotate rotate)
        {
            Move(center.X * -1, center.Y * -1);
            switch (rotate)
            {
                case PointRotate.Left: RotateLeft(); break;
                case PointRotate.Right: RotateRight(); break;
                default: throw new ArgumentOutOfRangeException(nameof(rotate));
            };
            Move(center.X, center.Y);
        }
        private void RotateLeft()
        {
            var oldY = Y;
            Y = X;
            X = -1 * oldY;
        }
        private void RotateRight()
        {
            var oldX = X;
            X = Y;
            Y = -1 * oldX;
        }

        public void Flip(Point center, PointFlip flip)
        {
            Move(center.X * -1, center.Y * -1);
            switch (flip)
            {
                case PointFlip.Horizontal: FlipHorizontal(); break;
                case PointFlip.Vertical: FlipVertical(); break;
                default: throw new ArgumentOutOfRangeException(nameof(flip));
            };
            Move(center.X, center.Y);
        }
        private void FlipHorizontal()
        {
            X *= -1;
        }
        private void FlipVertical()
        {
            Y *= -1;
        }

        /// <summary>
        /// Calculate delta (distance) required to move to have relative point.  
        /// Example: RelativePoint(0,0) and Point(2,2) -> Delta is (-2,-2)
        /// </summary>
        /// <param name="relativePoint">relative point</param>
        /// <returns>delta to relative point</returns>
        public Delta Delta(Point relativePoint) => new(relativePoint.X - X, relativePoint.Y - Y);

        public Point Copy() => new(X, Y);
        public static Point operator *(Point point, int value) => new(point.X * value, point.Y * value);

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Point point2)
                return false;

            if (point2.X != X || point2.Y != Y)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            // Create a custom hash code based on the fields
            int hash = 17;  // A prime number
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }
    }
}
