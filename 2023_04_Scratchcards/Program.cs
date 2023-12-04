using AdventCodeExtension;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input); //07:10 - 07:24
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
    return input.Split(Environment.NewLine)
                .Select(l => l.Split(": ")[1]
                              .Split(" | ")
                              .Select(n => n.Split(" ")
                                            .Where(x => !string.IsNullOrEmpty(x))
                                            .Select(x => int.Parse(x))
                                            .ToArray())
                              .ToArray())
                .Select(n => n[0].Intersect(n[1]).Count() - 1)
                .Where(x => x >= 0)
                .Select(n => Math.Pow(2, n))
                .Aggregate((x, y) => x + y);
}

object Part2(string input)
{
    return -1;
}
