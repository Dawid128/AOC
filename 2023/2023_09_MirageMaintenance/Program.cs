//[TryPrepend][[Recursion][Zip][CreateLessList-1][FuncOrDelegate]
using AdventCodeExtension;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part(input, ExtrapolateLastValue); //20
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part(input, ExtrapolateFirstValue); //20
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part(string input, Func<IList<int>, int> extrapolateValue)
{
    return input.Split(Environment.NewLine)
                .Select(x => x.Split(" ")
                              .Where(w => w != string.Empty)
                              .Select(s => int.Parse(s))
                              .ToList())
                .Select(extrapolateValue)
                .Sum();
}

int ExtrapolateLastValue(IList<int> numbers)
{
    if (!numbers.Any(x => x != 0))
        return 0;

    var nextNumbers = numbers.Zip(numbers.Skip(1), (current, next) => next - current).ToList();
    return ExtrapolateLastValue(nextNumbers) + numbers.Last();
}

int ExtrapolateFirstValue(IList<int> numbers)
{
    if (!numbers.Any(x => x != 0))
        return 0;

    var nextNumbers = numbers.Zip(numbers.Skip(1), (current, next) => next - current).ToList();
    return numbers.First() - ExtrapolateFirstValue(nextNumbers);
}