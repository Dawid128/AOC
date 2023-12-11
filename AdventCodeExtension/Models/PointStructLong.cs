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
    }
}
