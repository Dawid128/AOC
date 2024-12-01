//[Array2D][Map][Iterators][PointStruct][LocalCache][MoveOnMap][Queue][GoIn2Sides][SwitchExpressionSyntax]
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

    return GetScore(map, (new(-1, 0), new(1, 0)));
}

object Part2(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray())
                   .ToArray()
                   .To2DArray();

    //Prepare iterators around map
    List<(int StartX, int EndX, int StartY, int EndY, int DirX, int DirY)> GetIterators()
    {
        var endX = map.GetLength(1) - 1;
        var endY = map.GetLength(0) - 1;
        return
        [
            (0, endX, -1, -1, 0, 1),               //Top edge (from left to right)
            (0, endX, endY + 1, endY + 1, 0, -1),  //Bottom edge (from left to right)
            (-1, -1, 0, endY, 1, 0),               //Left edge (from top to bottom)
            (endX + 1, endX + 1, 0, endY, -1, 0),  //Right edge (from top to bottom)
        ];
    }
    var iterators = GetIterators();

    //Calculate max value
    var max = 0;
    foreach (var (startX, endX, startY, endY, dirX, dirY) in iterators)
        for (int x = startX; x <= endX; x++)
            for (int y = startY; y <= endY; y++)
                max = Math.Max(GetScore(map, (new(x, y), new(dirX, dirY))), max);

    return max;
}

int GetScore(char[,] map, (PointStruct Point, PointStruct Move) input)
{
    HashSet<(PointStruct Point, PointStruct Move)> localCache = [];

    var rowsNumber = map.GetLength(0);
    var columnsNumber = map.GetLength (1);

    var selectVisited = Enumerable.Range(1, rowsNumber).Select(x => new bool[columnsNumber]).ToArray();

    Queue<(PointStruct Point, PointStruct Move)> points = new([input]);
    while (points.Count > 0)
    {
        var (point, move) = points.Dequeue();

        if (localCache.Contains((point, move)))
            continue;
        localCache.Add((point, move));

        var nextPoint = new PointStruct(point.X + move.X, point.Y + move.Y);
        if ((nextPoint.X < 0 || nextPoint.X >= columnsNumber) || (nextPoint.Y < 0 || nextPoint.Y >= rowsNumber))
            continue;

        selectVisited[nextPoint.Y][nextPoint.X] = true;

        PointStruct[] nextMoves = map[nextPoint.Y, nextPoint.X] switch
        {
            '\\' => [new(move.Y, move.X)],
            '/' => [new(move.Y * -1, move.X * -1)],
            '|' when move.X != 0 => [new(move.Y, move.X), new(move.Y * -1, move.X * -1)],
            '-' when move.Y != 0 => [new(move.Y, move.X), new(move.Y * -1, move.X * -1)],
            _ => [new(move.X, move.Y)]
        };

        foreach (var nextMove in nextMoves)
            points.Enqueue((nextPoint, nextMove));
    }

    return selectVisited.SelectMany(x => x).Count(x => x == true);
}
