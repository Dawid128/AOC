using AdventCodeExtension;
using AdventCodeExtension.Models;
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
output = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray())
                   .ToArray()
                   .To2DArray();

    Execute(map, out var selectVisited);
    return selectVisited.SelectMany(x => x).Count(x => x == true);
}

object Part2(string input)
{
    return -1;
}

void Execute(char[,] map, out bool[][] selectVisited)
{
    HashSet<(PointStruct Point, PointStruct Move)> _cache = [];

    var rowsNumber = map.GetLength(0);
    var columnsNumber = map.GetLength (1);

    selectVisited = Enumerable.Range(1, rowsNumber).Select(x => new bool[columnsNumber]).ToArray();

    Queue<(PointStruct Point, PointStruct Move)> points = new([(new(-1, 0), new(1, 0))]);
    while (points.Count > 0)
    {
        var (point, move) = points.Dequeue();
        if (_cache.Contains((point, move)))
            continue;
        _cache.Add((point, move));

        var nextPoint = new PointStruct(point.X + move.X, point.Y + move.Y);
        if ((nextPoint.X < 0 || nextPoint.X >= columnsNumber) || (nextPoint.Y < 0 || nextPoint.Y >= rowsNumber))
            continue;

        selectVisited[nextPoint.Y][nextPoint.X] = true;
        var nextValue = map[nextPoint.Y, nextPoint.X];

        if (nextValue == '/') 
        {
            points.Enqueue((nextPoint, new(move.Y * -1, move.X * -1)));
            continue;
        }

        if (nextValue == '\\')
        {
            points.Enqueue((nextPoint, new(move.Y, move.X)));
            continue;
        }

        if ((move.X != 0 && nextValue == '|') || (move.Y != 0 && nextValue == '-'))
        {
            points.Enqueue((nextPoint, new(move.Y, move.X)));
            points.Enqueue((nextPoint, new(move.Y * -1, move.X * -1)));
            continue;
        }

        points.Enqueue((nextPoint, new(move.X, move.Y)));
    }
}
