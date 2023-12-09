using AdventCodeExtension;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input, 25);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input, (long)output);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, int preambleSize)
{
    var numbers = input.Split(Environment.NewLine)
                       .Select(x => long.Parse(x))
                       .ToList();

    var preambleNumbers = numbers.Take(preambleSize).ToList();
    foreach (var nextNumber in numbers.Skip(preambleSize)) 
    {
        if (!preambleNumbers.Any(n1 => preambleNumbers.Where(n2 => n1 != n2).Contains(nextNumber - n1)))
            return nextNumber;

        preambleNumbers.RemoveAt(0);
        preambleNumbers.Add(nextNumber);
    }

    return -1;
}

object Part2(string input, long numberRequired)
{
    var numbers = input.Split(Environment.NewLine)
                       .Select(x => long.Parse(x))
                       .ToList();

    var contiguousNumbers = new List<long>();
    foreach (var number in numbers)
    {
        contiguousNumbers.Add(number);

        var sum = contiguousNumbers.Sum();
        if (sum < numberRequired)
            continue;

        while (sum > numberRequired)
        {
            sum -= contiguousNumbers[0];
            contiguousNumbers.RemoveAt(0);
        }

        if (sum == numberRequired)
            return contiguousNumbers.Min() + contiguousNumbers.Max();
    }

    return -1;
}
