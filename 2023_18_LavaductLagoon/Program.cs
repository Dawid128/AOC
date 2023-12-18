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
    var instructions = input.Split(Environment.NewLine)
                            .Select(x => x.Split(" "))
                            .Select(x => (Direction: x[0], Value: int.Parse(x[1])))
                            .ToList();

    //Create Points
    var nextPoint = new PointStruct(0, 0);
    List<PointStruct> points = [nextPoint];
    foreach (var (direction, value) in instructions)
    {
        nextPoint = nextPoint.Move(GetMove(direction, value));
        points.Add(nextPoint);
    }

    //Create Map
    var minX = points.Min(p => p.X);
    var minY = points.Min(p => p.Y);
    var width = points.Max(p => p.X) - minX + 1;
    var height = points.Max(p => p.Y) - minY + 1;
    var map = new bool[height, width];

    //Corigate Points
    points = points.Select(x => x.Move(minX * -1, minY * -1)).ToList();

    //Complete Map
    for (int i = 1; i < points.Count; i++)
    {
        var previousPoint = points[i - 1];
        var currentPoint = points[i];

        var diffX = currentPoint.X - previousPoint.X;
        var diffY = currentPoint.Y - previousPoint.Y;
        var movePoint = new PointStruct(diffX != 0 ? diffX / Math.Abs(diffX) : 0, diffY != 0 ? diffY / Math.Abs(diffY) : 0);
        while (true)
        {
            map[previousPoint.Y, previousPoint.X] = true;
            if (previousPoint == currentPoint)
                break;

            previousPoint = previousPoint.Move(movePoint);
        }
    }

    //Fill in (required is know start point) at the moment hard coded. 
    var startPoint = new PointStruct(43, 3);
    var queue = new Queue<PointStruct>([startPoint]);
    var cache = new HashSet<PointStruct>([startPoint]);
    while (queue.Count > 0)
    {
        var point = queue.Dequeue();
        map[point.Y, point.X] = true;

        foreach (var adjacentPoint in point.GetAdjacentPoints4(1))
        {
            if (cache.Contains(adjacentPoint))
                continue;
            cache.Add(adjacentPoint);

            if (map[adjacentPoint.Y, adjacentPoint.X])
                continue;

            queue.Enqueue(adjacentPoint);
        }
    }

    //Thread.Sleep(1000);
    //var a = 0;
    //Console.WriteLine();
    //var currentPosition = Console.GetCursorPosition();
    //for (int rowId = 0; rowId < height; rowId++)
    //    for (int columnId = 0; columnId < width; columnId++)
    //    {
    //        Console.SetCursorPosition(currentPosition.Left + columnId, currentPosition.Top + rowId);
    //        if (map[rowId, columnId] == true)
    //        {
    //            Console.WriteLine("X");
    //            a++;
    //            continue;
    //        }
    //        Console.WriteLine(".");
    //    }

    //Console.SetCursorPosition(currentPosition.Left, currentPosition.Top - 1);
    //Console.ReadKey();

    //return a;

    //Result
    return map.ToJaggedArray()
              .SelectMany(x => x)
              .Count(x => x == true);
}

object Part2(string input)
{
    return -1;
}

(int X, int Y) GetMove(string direction, int value)
=> direction switch
{
    "R" => (value, 0),
    "L" => (value * -1, 0),
    "D" => (0, value),
    "U" => (0, value * -1),
    _ => throw new NotImplementedException(),
};