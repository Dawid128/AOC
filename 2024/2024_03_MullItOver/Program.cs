//[Regex]
using AdventCodeExtension;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
var output2 = Part2(input, 8);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    return MyRegex().Matches(input)
                    .Select(x => x.Value.Replace("mul(", string.Empty)
                                        .Replace(")", string.Empty)
                                        .Split(',')
                                        .Select(int.Parse)
                                        .Product())
                    .Sum();
}

object Part2(string input, int maxLength)
{
    var data = input.Insert(0, "do()")
                    .Split("don't()")
                    .SelectMany(x => x.Split("do()").Skip(1));

    long score = 0;
    foreach (var item in data)
    {
        score += MyRegex().Matches(item)
                          .Select(x => x.Value.Replace("mul(", string.Empty)
                                              .Replace(")", string.Empty)
                                              .Split(',')
                                              .Select(int.Parse)
                                              .Product())
                          .Sum();
    }
    return score;
}

partial class Program
{
    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MyRegex();
}