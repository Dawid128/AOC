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

        public readonly List<PointStruct> GetAdjacentPoints4(int radius)
        {
            var result = new List<PointStruct>();
            for (int i = 1; i <= radius; i++)
                foreach (var (x, y) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
                    result.Add(Move(x * i, y * i));

            return result;
        }

        public readonly IEnumerable<PointStruct> GetAdjacentPoints4(int radius, int width, int height)
        {
            for (int i = 1; i <= radius; i++)
                foreach (var (x, y) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
                {
                    var newPoint = Move(x * i, y * i);

                    //Ignore the points, if is outside of map
                    if ((newPoint.X < 0 || newPoint.X >= width) || (newPoint.Y < 0 || newPoint.Y >= height))
                        continue;

                    yield return newPoint;
                }
        }

        public readonly IEnumerable<(PointStruct Point, bool Outside)> GetInfiniteAdjacentPoints4(int width, int height)
        {
            foreach (var (x, y) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
            {
                var newPoint = Move(x, y);

                if ((newPoint.X < 0 || newPoint.X >= width) || (newPoint.Y < 0 || newPoint.Y >= height))
                {
                    var newX = newPoint.X;
                    if (newX < 0)
                        newX += width;
                    else if (newX >= width)
                        newX -= width;

                    var newY = newPoint.Y;
                    if (newY < 0)
                        newY += height;
                    else if (newY >= height)
                        newY -= height;

                    yield return (new(newX, newY), true);
                    continue;
                }

                yield return (newPoint, false);
            }
        }

        public readonly PointStruct Copy() => new(X, Y);

        public readonly PointStruct Rotate(PointStruct center, PointRotate rotate)
        => rotate switch
        {
            PointRotate.Left => Move(center.X * -1, center.Y * -1).RotateLeft().Move(center.X, center.Y),
            PointRotate.Right => Move(center.X * -1, center.Y * -1).RotateRight().Move(center.X, center.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(rotate))
        };
        public readonly PointStruct RotateLeft() => new(Y * -1, X);
        public readonly PointStruct RotateRight() => new(Y, X * -1);

        public readonly PointStruct Move(PointStruct point) => Move(point.X, point.Y);
        public readonly PointStruct Move((int X, int Y) coordinates) => Move(coordinates.X, coordinates.Y);
        public readonly PointStruct Move(int x, int y) => new(X + x, Y + y);

        public static bool operator ==(PointStruct pointL, PointStruct pointR) => ((pointL.X, pointL.Y) == (pointR.X, pointR.Y));
        public static bool operator !=(PointStruct pointL, PointStruct pointR) => ((pointL.X, pointL.Y) != (pointR.X, pointR.Y));
        public static PointStruct operator -(PointStruct pointL, PointStruct pointR) => new(pointL.X - pointR.X, pointL.Y - pointR.Y);
    }
}
