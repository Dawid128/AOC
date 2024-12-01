//[RangesVsMathFormula][ShoelaceFormula][InverseOfPicksTheorem]
using AdventCodeExtension.Models;
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
    var instructions = input.Split(Environment.NewLine)
                            .Select(x => x.Split(" "))
                            .Select(x => (Direction: x[0], Value: long.Parse(x[1])))
                            .ToList();

    return CalculateArea(instructions);
}

object Part2(string input)
{
    (string Direction, long Value) Parse(string part)
    {
        part = part.Substring(2, 6);
        var value = int.Parse(part.AsSpan(0, 5), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var direction = part.Last() switch
        {
            '0' => "R",
            '1' => "D",
            '2' => "L",
            '3' => "U",
            _ => throw new NotImplementedException(),
        };

        return (direction, value);
    }

    var instructions = input.Split(Environment.NewLine)
                            .Select(x => x.Split(" "))
                            .Select(x => Parse(x[2]))
                            .ToList();

    return CalculateArea(instructions);
}

double CalculateArea(List<(string Direction, long Value)> instructions)
{
    var currentPoint = new PointStructLong(0, 0);
    long circum = 1;
    long area = 0;
    foreach (var (direction, value) in instructions)
    {
        var move = GetMove(direction, value);
        var nextPoint = currentPoint.Move(move);

        circum += value;
        area += currentPoint.X * nextPoint.Y - currentPoint.Y * nextPoint.X;
        currentPoint = nextPoint;
    }

    return (area / 2) + (circum / 2) + 1;
}

(long X, long Y) GetMove(string direction, long value)
=> direction switch
{
    "R" => (value, 0),
    "L" => (value * -1, 0),
    "D" => (0, value),
    "U" => (0, value * -1),
    _ => throw new NotImplementedException(),
};