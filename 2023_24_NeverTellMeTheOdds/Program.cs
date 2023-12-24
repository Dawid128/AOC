using AdventCodeExtension.Models;
using System.Diagnostics;
using Range = AdventCodeExtension.Models.Range;


var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input, Range.CreateRangeBetween(200000000000000, 400000000000000));
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, Range range)
{
    var data = input.Replace(" ", string.Empty)
                    .Split(Environment.NewLine)
                    .Select(Parse)
                    .ToArray();

    bool IsValid(double x, double y, PointStructLong mp, PointStructLong sp)
    {
        //If line go right, but cross point is left relative to start point
        if (mp.X > 0 && x < sp.X)
            return false;

        //If line go left, but cross point is right relative to start point
        if (mp.X < 0 && x > sp.X)
            return false;

        //If line go top, but cross point is bottom relative to start point
        if (mp.Y > 0 && y < sp.Y)
            return false;

        //If line go bottom, but cross point is top relative to start point
        if (mp.Y < 0 && y > sp.Y)
            return false;

        if (mp.X == 0 || mp.Y == 0)
            throw new Exception("Method not support move with value 0");

        return true;
    }

    var count = 0;
    for (int i = 0; i < data.Length; i++)
    {
        var (a1, b1, sp1, mp1) = data[i];
        for (int j = i + 1; j < data.Length; j++) 
        {
            var (a2, b2, sp2, mp2) = data[j];
            if (a1 == a2)
                continue;

            var (x, y) = FindLinearIntersection(a1, b1, a2, b2);
            if (x > range.End || x < range.Start || y > range.End || y < range.Start) 
                continue;

            if (!IsValid(x, y, mp1, sp1) || !IsValid(x, y, mp2, sp2))
                continue;

            count++;
        }
    }

    return count;
}

object Part2(string input)
{
    return -1;
}

(double a, double b, PointStructLong StartPoint, PointStructLong MovePoint) Parse(string input)
{
    var split = input.Split("@");
    var point1 = split[0].Split(",").Select(long.Parse).ToArray();
    var move = split[1].Split(",").Select(long.Parse).ToArray();

    var (a, b) = GetLinearFormula(point1[0], point1[1], point1[0] + move[0], point1[1] + move[1]);
    return (a, b, new(point1[0], point1[1]), new(move[0], move[1]));
}

(double a, double b) GetLinearFormula(double x1, double y1, double x2, double y2)
{
    var a = (y1 - y2) / (x1 - x2);
    var b = y1 - (a * x1);
    return (a, b);
}

(double x, double y) FindLinearIntersection(double a1, double b1, double a2, double b2)
{
    if (a1 == a2)
        throw new ArgumentException("The lines are parallel and do not intersect.");

    var x = (b2 - b1) / (a1 - a2);
    var y = a1 * x + b1;

    return (x, y);
}