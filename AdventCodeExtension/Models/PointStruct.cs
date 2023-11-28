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
    }
}
