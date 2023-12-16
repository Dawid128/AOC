using System.Diagnostics;

namespace AdventCodeExtension.Models
{
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public struct PointStruct
    {
        public int X { get; }
        public int Y { get; }

        public PointStruct(Point point) : this(point.X, point.Y)
        {
        }
        public PointStruct(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point ToPoint() => new(X, Y);

        public List<PointStruct> GetAdjacentPoints()
        {
            var result = new List<PointStruct>();
            foreach (var x in new[] { -1, 0, 1 })
                foreach (var y in new[] { -1, 0, 1 })
                    if (x != 0 || y != 0)
                        result.Add(new PointStruct(X + x, Y + y));

            return result;
        }

        public readonly PointStruct Rotate(PointStruct center, PointRotate rotate)
        => rotate switch
        {
            PointRotate.Left => Move(center.X * -1, center.Y * -1).RotateLeft().Move(center.X, center.Y),
            PointRotate.Right => Move(center.X * -1, center.Y * -1).RotateRight().Move(center.X, center.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(rotate))
        };
        public readonly PointStruct RotateLeft() => new(Y * -1, X);
        public readonly PointStruct RotateRight() => new(Y, X * -1);
        public readonly PointStruct Move(int x, int y) => new(X + x, Y + y);
    }
}
