using AdventCodeExtension;
using AdventCodeExtension.Models;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt"); 
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input); //30
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input); //40
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.Select(s => int.TryParse(s.ToString(), out var number) ? number : s == '.' ? -2 : -1)
                                 .ToArray())
                   .ToArray();

    return GetScore(map);
}

object Part2(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.Select(s => int.TryParse(s.ToString(), out var number) ? number : s == '*' ? -1 : -2)
                                 .ToArray())
                   .ToArray();

    var gears = map.SelectMany((x, rowId) => x.Select((value, columnId) => (Value: value, Point: new PointStruct(columnId, rowId))))
                   .Where(x => x.Value == -1)
                   .Select(x => x.Point)
                   .ToList();

    var numbers = GetNumbers(map);

    var result = 0;
    foreach (var gear in gears)
    {
        var neighboursGear = GetNeighboursPoints(gear).ToList();

        var neighbourNumbers = numbers.Where(x => x.Points.Intersect(neighboursGear).Any())
                                      .ToList();
        if (neighbourNumbers.Count != 2)
            continue;

        result += (int)neighbourNumbers.Select(x => x.Number).Product();
        foreach (var neighbourNumber in neighbourNumbers)
            numbers.Remove(neighbourNumber);
    }

    return result;
}

int GetScore(int[][] map)
{
    var rowsNumber = map.Length;
    var columnsNumber = map[0].Length;

    var result = 0;
    void BuildResult(int number) { result += number; }

    for (int rowId = 0; rowId < rowsNumber; rowId++)
    {
        var number = 0;
        var hasNeighbourSymbol = false;
        for (int columnId = 0; columnId < columnsNumber; columnId++)
        {
            var cell = map[rowId][columnId];
            if (cell < 0)
            {
                if (hasNeighbourSymbol)
                    BuildResult(number);

                number = 0;
                hasNeighbourSymbol = false;
                continue;
            }

            number *= 10;
            number += cell;
            if (!hasNeighbourSymbol)
                hasNeighbourSymbol = HasNeighbourSymbol(map, rowId, columnId);
        }

        if (hasNeighbourSymbol)
            BuildResult(number);
    }

    return result;
}

bool HasNeighbourSymbol(int[][] map, int rowId, int columnId)
{
    var rowsNumber = map.Length;
    var columnsNumber = map[0].Length;

    var starRange = (rowId > 0 ? rowId - 1 : 0, columnId > 0 ? columnId - 1 : 0);
    var endRange = (rowId + 1 < rowsNumber ? rowId + 1 : rowsNumber - 1, columnId + 1 < columnsNumber ? columnId + 1 : columnsNumber - 1);

    return map.TakeRange(starRange, endRange).SelectMany(x => x)
                                             .Any(x => x == -1);
}

List<(int Number, List<PointStruct> Points)> GetNumbers(int[][] map)
{
    var rowsNumber = map.Length;
    var columnsNumber = map[0].Length;

    var result = new List<(int Number, List<PointStruct>)>();
    for (int rowId = 0; rowId < rowsNumber; rowId++)
    {
        var number = 0;
        var points = new List<PointStruct>();
        for (int columnId = 0; columnId < columnsNumber; columnId++)
        {
            var cell = map[rowId][columnId];
            if (cell < 0)
            {
                if (points.Count > 0)
                    result.Add((number, points));

                number = 0;
                points = new List<PointStruct>();
                continue;
            }

            number *= 10;
            number += cell;
            points.Add(new PointStruct(columnId, rowId));
        }

        if (points.Count > 0)
            result.Add((number, points));
    }

    return result;
}

IEnumerable<PointStruct> GetNeighboursPoints(PointStruct point)
{
    foreach (var x in new[] { -1, 0, 1 })
        foreach (var y in new[] { -1, 0, 1 })
            if (x != 0 || y != 0)
                yield return new PointStruct(point.X + x, point.Y + y);
}