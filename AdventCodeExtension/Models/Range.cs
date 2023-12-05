using System.Diagnostics;

namespace AdventCodeExtension.Models
{
    [DebuggerDisplay("Start:{Start}, End:{End}, Length:{Length}")]
    public readonly struct Range(long start, long length)
    {
        public long Start { get; } = start;
        public long End { get; } = start + length - 1;
        public long Length { get; } = length;

        public bool IsInside(Range otherRange) => Start >= otherRange.Start && End <= otherRange.End;

        public bool IsOverlap(Range otherRange) => Math.Max(Start, otherRange.Start) < Math.Min(End, otherRange.End);
 
        /// <summary>
        /// Split the current range by the input ranges
        /// </summary>
        /// <param name="cutRanges">Input ranges used to split the current range</param>
        /// <returns></returns>
        public IEnumerable<Range> SplitByRanges(IList<Range> ranges)
        {
            var currentStart = Start;
            foreach (var splitRange in ranges)
            {
                if (currentStart < splitRange.Start)
                {
                    yield return CreateRangeBetween(currentStart, splitRange.Start);
                    currentStart = splitRange.Start;
                }

                yield return CreateRangeBetween(currentStart, Math.Min(End, splitRange.End) + 1);
                currentStart = End + 1;
            }

            if (currentStart < End)
                yield return CreateRangeBetween(currentStart, End);
        }

        public static Range CreateRangeBetween(long start, long end) => new(start, end - start + 1);
    }
}
