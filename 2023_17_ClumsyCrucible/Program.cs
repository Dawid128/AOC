using AdventCodeExtension;
using AdventCodeExtension.Models;
using System.Diagnostics;
using System.Drawing;
using System.Security.AccessControl;

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
                   .Select(x => x.Select(c => int.Parse(c.ToString()))
                                 .ToArray())
                   .ToArray()
                   .To2DArray();

    return GetScore(map);
}

object Part2(string input)
{
    return -1;
}

int GetScore(int[,] map)
{
    Dictionary<(PointStruct Point, int Straights), int> cache = [];

    var rowsNumber = map.GetLength(0);
    var columnsNumber = map.GetLength(1);
    var root = new PointStruct(0, 0);

    Queue<(PointStruct Point, PointStruct previousMove, int Straights, int Sum)> queue = [];
    queue.Enqueue((root, new PointStruct(0, 0), 0, map[root.X, root.Y]));
    cache.Add((root, 0), int.MaxValue);
    while (queue.Count > 0)
    {
        var (point, previousMove, previousStraights, sum) = queue.Dequeue();
        var nextPoints = GetPossiblePoints(rowsNumber, columnsNumber, point, previousMove, previousStraights);
        foreach (var (nextPoint, move, straights) in nextPoints)
        {
            var newSum = sum + map[root.X, root.Y];

            if (cache.TryGetValue((nextPoint, straights), out var cacheSum) && newSum >= cacheSum) 
                continue;

            Console.WriteLine($"New Point: ({nextPoint.X},{nextPoint.Y}) [{straights}] = {newSum}");
            if (!cache.ContainsKey((nextPoint, straights)))
                cache.Add((nextPoint, straights), newSum);
            else
                cache[(nextPoint, straights)] = newSum;

            queue.Enqueue((nextPoint, move, straights, newSum));
        }
    }

    return -1;
}

List<(PointStruct Point, PointStruct Move, int MoveOneDirectionCount)> GetPossiblePoints(int rowsNumber, int columnsNumber, PointStruct point, PointStruct previousMove, int previousMoveOneDirectionCount)
{
    var result = new List<(PointStruct Point, PointStruct Move, int MoveOneDirectionCount)>();
    foreach (var (x, y) in new[] { (0, -1), (1, 0), (0, 1), (-1, 0) })
    {
        //Ignore the points from which it comes
        if ((previousMove.X, previousMove.Y) == (x * -1, y * -1))
            continue;

        var continueMoveOneDirection = false;
        if ((previousMove.X, previousMove.Y) == (x, y))
            continueMoveOneDirection = true;

        //Ignore the points if continue straight is not possible
        if (previousMoveOneDirectionCount == 3 && continueMoveOneDirection)
            continue;

        var newPoint = new PointStruct(point.X + (x * -1), point.Y + (y * -1));

        //Ignore the points, if is outside of map
        if ((newPoint.X < 0 || newPoint.X >= columnsNumber) || (newPoint.Y < 0 || newPoint.Y >= rowsNumber))
            continue;

        result.Add((newPoint, new PointStruct(x, y), continueMoveOneDirection ? ++previousMoveOneDirectionCount : 1));
    }

    return result;
}