using AdventCodeExtension;
using System.Diagnostics;

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
    var data = input.Split(Environment.NewLine)
                    .Select(s1 => s1.Split(": "))
                    .Select(s2 => (ResultTest: long.Parse(s2[0]),
                                   Numbers: s2[1].Split(" ")
                                                 .Select(long.Parse)
                                                 .ToList()))
                    .ToList();

    long score = 0;
    foreach (var (resultTest, numbers) in data)
    {
        if (!Test(numbers, resultTest))
            continue;

        score += resultTest;
    }

    return score;
}

object Part2(string input, int maxLength)
{
    var data = input.Split(Environment.NewLine)
                    .Select(s1 => s1.Split(": "))
                    .Select(s2 => (ResultTest: long.Parse(s2[0]),
                                   Numbers: s2[1].Split(" ")
                                                 .Select(long.Parse)
                                                 .ToList()))
                    .ToList();

    long score = 0;
    foreach (var (resultTest, numbers) in data)
    {
        if (!Test2(numbers, resultTest))
            continue;

        score += resultTest;
    }

    return score;
}

bool Test(List<long> numbers, long resultTest)
{
    var results = new HashSet<long>() { numbers.First() };
    foreach (var number in numbers.Skip(1))
    {
        var newResults = new HashSet<long>();
        newResults.AddRange(results.Select(x => x + number).Where(x => x <= resultTest).ToList());
        newResults.AddRange(results.Select(x => x * number).Where(x => x <= resultTest).ToList());

        results = newResults;
    }

    return results.Contains(resultTest);
}

bool Test2(List<long> numbers, long resultTest)
{
    long Concat(long number1, long number2)
    => long.Parse($"{number1}{number2}");

    var results = new HashSet<long>() { numbers.First() };
    foreach (var number in numbers.Skip(1))
    {
        var newResults = new HashSet<long>();
        newResults.AddRange(results.Select(x => x + number).Where(x => x <= resultTest).ToList());
        newResults.AddRange(results.Select(x => x * number).Where(x => x <= resultTest).ToList());
        newResults.AddRange(results.Select(x => Concat(x, number)).Where(x => x <= resultTest).ToList());

        results = newResults;
    }

    return results.Contains(resultTest);
}