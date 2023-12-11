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
    var map = Parse(input);
    map = ExpansionMap(map);

    var galaxies = map.SelectMany((x, rowId) => x.Select((Value, columnId) => (Value, Point: new PointStruct(columnId, rowId))))
                      .Where(x => x.Value is true)
                      .Select(x => x.Point)
                      .ToList();

    return galaxies.Select((x, index) => (Point: x, Points: galaxies.Skip(index + 1)))
                   .SelectMany(x => x.Points.Select(p => Math.Abs(x.Point.X - p.X) + Math.Abs(x.Point.Y - p.Y)))
                   .Sum();
}

object Part2(string input)
{
    return -1;
}

bool[][] Parse(string input)
=> input.Split(Environment.NewLine)
        .Select(x => x.Select(s => s == '#')
                      .ToArray())
        .ToArray();

bool[][] ExpansionMap(bool[][] map)
{
    var array2D = map.To2DArray();

    var emptyRows = array2D.TakeRows()
                           .Select((Value, Index) => (Value, Index))
                           .Where(x => !x.Value.Any(a => a == true))
                           .Select(x => x.Index)
                           .ToList();

    var emptyColumns = array2D.TakeColumns()
                              .Select((Value, Index) => (Value, Index))
                              .Where(x => !x.Value.Any(a => a == true))
                              .Select(x => x.Index)
                              .ToList();

    foreach (var rowId in emptyRows.Reverse<int>())
        array2D = array2D.InsertRow(rowId, Enumerable.Range(0, array2D.GetLength(1)).Select(x => false).ToArray());

    foreach (var columnId in emptyColumns.Reverse<int>())
        array2D = array2D.InsertColumn(columnId, Enumerable.Range(0, array2D.GetLength(0)).Select(x => false).ToArray());

    return array2D.ToJaggedArray();
}