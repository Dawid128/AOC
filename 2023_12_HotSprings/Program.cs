using AdventCodeExtension;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

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

    var count = 0;
    foreach (var (mask, numbers) in data)
        count += DifferentArrangementsCount(mask, numbers);

    return count;
}

object Part2(string input)
{
    return -1;
}

int DifferentArrangementsCount(string input, IList<int> numbers)
{
    var count = 0;
    bool IsValid(string part, char? before, char? after) => !part.Contains('.') && before != '#' && after != '#';

    var minLength = numbers.Sum() + numbers.Count - 1;
    var firstNumber = numbers.First();
    for (int i = 0; i <= input.Length - minLength; i++) 
    {
        var part = input.Substring(i, firstNumber);
        var before = input.ElementAtOrDefault(i - 1);
        var after = input.ElementAtOrDefault(i + firstNumber);
        if (!IsValid(part, before, after)) 
            continue;

        if (i > 0)
        {
            var partBefore = input.Substring(0, i);
            if (partBefore.Any(x => x == '#'))
                break;
        }

        if (numbers.Count == 1)
        {
            var partAfter = input.Substring(i + firstNumber);
            if (!partAfter.Any(x => x == '#'))
                count++;

            continue;
        }

        count += DifferentArrangementsCount(input.Substring(i + firstNumber + 1), numbers.Skip(1).ToList());
    }

    return count;
}