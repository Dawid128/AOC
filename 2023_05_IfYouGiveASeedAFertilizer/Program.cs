using AdventCodeExtension;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

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
output = Part2(input);
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
    return -1;
}

List<long> ConvertNumbers(IList<long> seeds, ConvertNumberMap convertNumberMap)
{
    List<long> result = [];
    foreach (var seed in seeds)
    {
        var nextNumber = seed;
        foreach (var rcnm in convertNumberMap.RangeConvertNumberMaps)
        {
            if (seed >= rcnm.SourceRangeStart && seed <= rcnm.SourceRangeEnd)
            {
                nextNumber = rcnm.DestinationRangeStart + seed - rcnm.SourceRangeStart;
                continue;
            }
        }
        result.Add(nextNumber);
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

            rangeConvertNumberMaps.Add(new RangeConvertNumberMap(numbers[0], numbers[1], numbers[2]));
        }

        return new ConvertNumberMap(rangeConvertNumberMaps);
    }

    var convertNumberMaps = split.Skip(1)
                                 .Select(x => ParseToConvertNumberMap(x.Split(Environment.NewLine).Skip(1).ToList()))
                                 .ToList();

    //Result
    return (seeds, convertNumberMaps);
}



readonly struct ConvertNumberMap(IList<RangeConvertNumberMap> rangeConvertNumberMaps)
{
    public List<RangeConvertNumberMap> RangeConvertNumberMaps{ get; } = [.. rangeConvertNumberMaps];
}

readonly struct RangeConvertNumberMap(long destinationRangeStart, long sourceRangeStart, long rangeLength)
{
    public long DestinationRangeStart { get; } = destinationRangeStart;
    public long SourceRangeStart { get; } = sourceRangeStart;
    public long SourceRangeEnd { get; } = sourceRangeStart + rangeLength;
    public long RangeLength { get; } = rangeLength;
}
