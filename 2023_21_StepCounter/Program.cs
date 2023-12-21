using AdventCodeExtension;
using AdventCodeExtension.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;

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
                   .ToArray();

    var startPoint = map.SelectMany((row, Y) => row.Select((Value, X) => (Point: new PointStruct(X, Y), Value))).Single(x => x.Value == 'S').Point;
    return GetScore(map , startPoint, 64);
}

object Part2(string input)
{
    return -1;
}

int GetScore(char[][] map, PointStruct startPoint, int repetitionsNumber)
{
    var height = map.Length;
    var width = map[0].Length;

    HashSet<PointStruct> visitedPoints = [startPoint];
    List<int> visitedCountInStep = [1];

    var queue = new Queue<PointStruct>([startPoint]);
    for (var i = 1; i <= repetitionsNumber; i++)
    {
        Queue<PointStruct> newQueue = [];
        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();

            foreach (var nextPoint in currentPoint.GetAdjacentPoints4(1, width, height))
            {
                if (visitedPoints.Contains(nextPoint))
                    continue;

                if (map[nextPoint.Y][nextPoint.X] == '#')
                    continue;

                newQueue.Enqueue(nextPoint);
                visitedPoints.Add(nextPoint);
            }
        }
        visitedCountInStep.Add(newQueue.Count);
        queue = newQueue;
    }

    var result = visitedCountInStep.Where((_, index) => index % 2 == 0).Sum();
    return result;
}
