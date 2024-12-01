//[DynamicProgramming][TopDownApproch][Recursion][Cache][EnumerableRepeat][ReadOnlySpan][Slice][EquivalentRangeSyntax]
using AdventCodeExtension;
using System.Buffers;
using System.Diagnostics;

Dictionary<string, long> _cache = [];
var searchHash = SearchValues.Create("#".AsSpan());
var searchDot = SearchValues.Create(".".AsSpan());

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input);
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
    var data = input.Split(Environment.NewLine)
                    .Select(x => x.Split(" "))
                    .Select(x => (Mask: x[0],
                                  Numbers: x[1].Split(",")
                                               .Select(s => int.Parse(s))
                                               .ToList()))
                    .ToList();

    long count = 0;
    foreach (var (mask, numbers) in data)
        count += DifferentArrangementsCount(mask.AsSpan(), numbers);

    return count;
}

object Part2(string input)
{
    var data = input.Split(Environment.NewLine)
                    .Select(x => x.Split(" "))
                    .Select(x => (Mask: x[0],
                                  Numbers: x[1].Split(",")
                                               .Select(s => int.Parse(s))
                                               .ToList()))
                    .Select(x => (Mask: Enumerable.Repeat(x.Mask, 5).Aggregate((x, y) => $"{x}?{y}"),
                                  Numbers: Enumerable.Repeat(x.Numbers, 5).SelectMany(x => x).ToList()))
                    .ToList();

    long count = 0;
    foreach (var (mask, numbers) in data)
        count += DifferentArrangementsCount(mask.AsSpan(), numbers);

    return count;
}

long DifferentArrangementsCount(ReadOnlySpan<char> input, IList<int> numbers)
{
    long count = 0;
    bool IsValid(ReadOnlySpan<char> part, char? before, char? after) => !part.ContainsAny(searchDot) && before != '#' && after != '#';

    var minLength = numbers.Sum() + numbers.Count - 1;
    var firstNumber = numbers.First();
    for (int i = 0; i <= input.Length - minLength; i++) 
    {
        var part = input.Slice(i, firstNumber);
        char? before = i > 0 ? input[i - 1] : null;
        char? after = i + firstNumber < input.Length ? input[i + firstNumber] : null;
        if (!IsValid(part, before, after)) 
            continue;

        if (i > 0)
        {
            var partBefore = input[..i];
            if (partBefore.ContainsAny(searchHash))
                break;
        }

        if (numbers.Count == 1)
        {
            var partAfter = input[(i + firstNumber)..];
            if (!partAfter.ContainsAny(searchHash))
                count++;

            continue;
        }

        var nextInput = input[(i + firstNumber + 1)..];
        var nextNumbers = numbers.Skip(1).ToList();
        var key = nextInput.ToString() + " ; " + nextNumbers.Select(x => x.ToString()).Aggregate((x, y) => $"{x},{y}");
        if (_cache.TryGetValue(key, out var countTemp))
        {
            count += countTemp;
            continue;
        }

        countTemp = DifferentArrangementsCount(nextInput, nextNumbers);
        count += countTemp;
        _cache.Add(key, countTemp);
    }

    return count;
}