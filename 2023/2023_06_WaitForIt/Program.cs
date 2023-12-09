//[FormatRow][QuadraticInequality]
using AdventCodeExtension;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input); //30-20
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input); //15
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var data = input.Replace("Time:  ", string.Empty)
                    .Replace("Distance:", string.Empty)
                    .Split(Environment.NewLine)
                    .Select(x => x.Split(" ")
                                  .Where(x => int.TryParse(x, out _))
                                  .Select(x => int.Parse(x))
                                  .ToArray())
                    .ToArray();

    var timeAndDistanceList = data[0].Zip(data[1], (Time, Distance) => (Time, Distance)).ToArray();

    var score = 1;
    foreach (var (time, distance) in timeAndDistanceList)
    {
        var count = 0;
        for (int i = 0; i <= time; i++)
            if ((time - i) * i > distance)
                count++;

        score *= count;
    }

    return score;
}

object Part2(string input)
{
    var data = input.Replace("Time:  ", string.Empty)
                    .Replace("Distance:", string.Empty)
                    .Replace(" ", string.Empty)
                    .Split(Environment.NewLine)
                    .Select(double.Parse)
                    .ToArray();

    var time = data[0];
    var distance = data[1];


    var (min, max) = SolveQuadraticInequality(-1, data[0], -data[1]);
    return Math.Floor(max) - Math.Ceiling(min) + 1;
}

static (double Min, double Max) SolveQuadraticInequality(double a, double b, double c)
{
    double discriminant = b * b - 4 * a * c;

    if (discriminant < 0)
        throw new Exception("Discriminant can not be null");

    if (discriminant == 0)
    {
        double root = -b / (2 * a);
        return (root, root);
    }

    double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
    double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

    return (Math.Min(root1, root2), Math.Max(root1, root2));
}