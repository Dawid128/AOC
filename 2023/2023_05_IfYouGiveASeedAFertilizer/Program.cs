//[Range][SplitRangeByRanges][Long][BigNumbers][WorkWithRangeInsteadSingleNumber]
using System.Diagnostics;
using Range = AdventCodeExtension.Models.Range;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input); //40
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input); //XX
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var (seeds, convertNumberMaps) = Parse(input);

    foreach (var convertNumberMap in convertNumberMaps)
        seeds = ConvertNumbers(seeds, convertNumberMap);

    return seeds.Min();
}

object Part2(string input)
{
    var (seeds, convertNumberMaps) = Parse(input);

    var rangeSeeds = ParseToRange(seeds);
    foreach (var convertNumberMap in convertNumberMaps)
        rangeSeeds = ConvertNumberRange(rangeSeeds, convertNumberMap);

    return rangeSeeds.Select(x=>x.Start).Min();
}

List<long> ConvertNumbers(IList<long> seeds, ConvertNumberMap convertNumberMap)
{
    List<long> result = [];
    foreach (var seed in seeds)
    {
        var nextNumber = seed;
        foreach (var rcnm in convertNumberMap.RangeConvertNumberMaps)
        {
            if (seed >= rcnm.SourceRange.Start && seed <= rcnm.SourceRange.End)
            {
                nextNumber = rcnm.DestinationRange.Start + seed - rcnm.SourceRange.Start;
                continue;
            }
        }
        result.Add(nextNumber);
    }

    return result;
}

List<Range> ConvertNumberRange(IList<Range> rangeSeeds, ConvertNumberMap convertNumberMap)
{
    List<Range> result = [];
    foreach (var rangeSeed in rangeSeeds)
    {
        var matchRanges = convertNumberMap.RangeConvertNumberMaps.Where(x => x.SourceRange.IsOverlap(rangeSeed)).ToList();

        //If not found any match range, get range seed without change
        if (matchRanges.Count == 0)
        {
            result.Add(rangeSeed);
            continue;
        }

        //ITERATE ranges B overlapped with range A
        var currentStart = rangeSeed.Start;
        foreach (var (sourceRange, destinationRange) in matchRanges.Select(x => (x.SourceRange, x.DestinationRange)))
        {
            //IF range A exist before range B[N]: Add part of range A before range B[N] without change
            if (currentStart < sourceRange.Start)
            {
                result.Add(Range.CreateRangeBetween(currentStart, sourceRange.Start));
                currentStart = sourceRange.Start;
            }

            //IF range A exist after range B[N]: Add part of range A before end range B[N] with changes
            //IF range A not exist after range B[N]: add range A with changes
            var diff = destinationRange.Start - sourceRange.Start;
            result.Add(new Range(currentStart + diff, Math.Min(rangeSeed.End, sourceRange.End) - currentStart + 1));
            currentStart = rangeSeed.End + 1;
        }

        //IF range A exist after range C (last B[N]): Add part of range A after last range B[N] without change 
        if (currentStart < rangeSeed.End)
            result.Add(Range.CreateRangeBetween(currentStart, rangeSeed.End));
    }

    return result;
}

(List<long> Seeds, List<ConvertNumberMap> ConvertNumberMaps) Parse(string input)
{
    var split = input.Split(Environment.NewLine + Environment.NewLine);

    //Parse Seeds
    var seeds = split[0].Replace("seeds: ", string.Empty)
                        .Split(" ")
                        .Select(x => long.Parse(x))
                        .ToList();

    //Parse ConvertNumberMaps
    ConvertNumberMap ParseToConvertNumberMap(IList<string> lines)
    {
        List<RangeConvertNumberMap> rangeConvertNumberMaps = [];
        foreach (var line in lines)
        {
            var numbers = line.Split(" ")
                              .Select(x => long.Parse(x))
                              .ToArray();

            rangeConvertNumberMaps.Add(new RangeConvertNumberMap(new Range(numbers[0], numbers[2]), new Range(numbers[1], numbers[2])));
        }

        return new ConvertNumberMap(rangeConvertNumberMaps.OrderBy(x=>x.SourceRange.Start).ToList());
    }

    var convertNumberMaps = split.Skip(1)
                                 .Select(x => ParseToConvertNumberMap(x.Split(Environment.NewLine).Skip(1).ToList()))
                                 .ToList();

    //Result
    return (seeds, convertNumberMaps);
}

List<Range> ParseToRange(List<long> seeds)
{
    List<Range> result = [];
    for (int i = 0; i < seeds.Count - 1; i += 2)
        result.Add(new Range(seeds[i], seeds[i + 1]));

    return result.OrderBy(x=>x.Start).ToList();
}

readonly struct ConvertNumberMap(IList<RangeConvertNumberMap> rangeConvertNumberMaps)
{
    public List<RangeConvertNumberMap> RangeConvertNumberMaps { get; } = [.. rangeConvertNumberMaps];
}

[DebuggerDisplay("{SourceRange} -> {DestinationRange}")]
readonly struct RangeConvertNumberMap(Range destinationRange, Range sourceRange)
{

    public Range DestinationRange { get; } = destinationRange;
    public Range SourceRange { get; } = sourceRange;
}