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
    }
}
