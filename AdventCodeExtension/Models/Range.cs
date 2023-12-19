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

        public Range CutRangeAfter(int number)
        {
            if (number > End)
                return CreateRangeBetween(Start, End);

            if (Start > number)
                return CreateRangeBetween(0, 0);

            return CreateRangeBetween(Start, number);
        }

        public Range CutRangeBefore(int number)
        {
            if (number < Start)
                return CreateRangeBetween(Start, End);

            if (End < number)
                return CreateRangeBetween(0, 0);

            return CreateRangeBetween(number, End);
        }

        public Range RemoveLastNumber() => Length > 0 ? CreateRangeBetween(Start, End - 1) : CreateRangeBetween(0, 0);
        public Range RemoveFirstNumber() => Length > 0 ? CreateRangeBetween(Start + 1, End) : CreateRangeBetween(0, 0);

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
