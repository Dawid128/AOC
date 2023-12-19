using System.Diagnostics;

namespace AdventCodeExtension.Models
{
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public struct PointStructLong
    {
        public long X { get; }
        public long Y { get; }

        public PointStructLong(Point point) : this(point.X, point.Y)
        {
        }
        public PointStructLong(long x, long y)
        {
            X = x;
            Y = y;
        }
        public readonly PointStructLong Rotate(PointStructLong center, PointRotate rotate)
        => rotate switch
        {
            PointRotate.Left => Move(center.X * -1, center.Y * -1).RotateLeft().Move(center.X, center.Y),
            PointRotate.Right => Move(center.X * -1, center.Y * -1).RotateRight().Move(center.X, center.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(rotate))
        };
        public readonly PointStructLong RotateLeft() => new(Y * -1, X);
        public readonly PointStructLong RotateRight() => new(Y, X * -1);

        public readonly PointStructLong Move(PointStructLong point) => Move(point.X, point.Y);
        public readonly PointStructLong Move((long X, long Y) coordinates) => Move(coordinates.X, coordinates.Y);
        public readonly PointStructLong Move(long x, long y) => new(X + x, Y + y);

        public static bool operator ==(PointStructLong pointL, PointStructLong pointR) => ((pointL.X, pointL.Y) == (pointR.X, pointR.Y));
        public static bool operator !=(PointStructLong pointL, PointStructLong pointR) => ((pointL.X, pointL.Y) != (pointR.X, pointR.Y));
    }
}
