using AdventCodeExtension.Models;
using System.Diagnostics;
using Point = AdventCodeExtension.Models.Point;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input, out var path);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
var output2 = Part2(input, path);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, out List<(Cell<char> StartItem, Point Move)> Path)
{
    var map = input.Split(Environment.NewLine)
                   .SelectMany((s1, row) => s1.Select((s2, column) => new Cell<char>(row, column, s2)))
                   .ToList();

    var startItem = map.Single(x => x.Value == '^');
    var move = new Point(0, -1);

    //Console.Clear();
    //ConsoleLog.WriteMap2D(map);
    //Thread.Sleep(500);

    Path = [(startItem, move.Copy())];
    var visited = new HashSet<Cell<char>>() { startItem };
    var nextItem = startItem;
    while (true)
    {
        var nextItemTemp = map.FirstOrDefault(x => x.RowId == nextItem.RowId + move.Y && x.ColumnId == nextItem.ColumnId + move.X);
        if (nextItemTemp.Value is default(char))
            break;

        if (nextItemTemp.Value == '#')
        {
            move.Rotate(new Point(0, 0), PointRotate.Left);
            Path.RemoveAt(Path.Count - 1);
            Path.Add((nextItem, move.Copy()));
            continue;
        }

        //ConsoleLog.WriteCellOnMap2D(new(nextItemTemp.RowId, nextItemTemp.ColumnId, 'X'), ConsoleColor.Green);
        //Thread.Sleep(500);

        nextItem = nextItemTemp;
        visited.Add(nextItem);
        Path.Add((nextItem, move.Copy()));
        continue;
    }

    return visited.Count;
}

object Part2(string input, List<(Cell<char> StartItem, Point Move)> Path)
{
    var map = input.Split(Environment.NewLine)
                   .SelectMany((s1, row) => s1.Select((s2, column) => new Cell<char>(row, column, s2)))
                   .ToList();

    var score = 0;
    var i = 0;
    var positionUsed = new HashSet<string>(0);
    foreach (var (startItem, move) in Path)
    {
        //Console.WriteLine(i++ + "/" + Path.Count);
        var x = startItem.ColumnId + move.X;
        var y = startItem.RowId + move.Y;

        //Ignore off the map 
        var tempBlock = map.FirstOrDefault(f => f.RowId == y && f.ColumnId == x);
        if (tempBlock.Value is default(char))
            continue;

        //Ignore the same 'blocks'
        var key = $"[{tempBlock.ColumnId};{tempBlock.RowId}]";
        if (positionUsed.Contains(key))
            continue;
        positionUsed.Add(key);

        var indexTempBlock = map.IndexOf(tempBlock);

        map.RemoveAt(indexTempBlock);
        map.Insert(indexTempBlock, new Cell<char>(tempBlock.RowId, tempBlock.ColumnId, '#'));

        if (Test(map, startItem, move.Copy(), false))
        {
            score++;
            //Test(map, startItem, move.Copy(), true);
            //Console.Clear();
        }

        map.RemoveAt(indexTempBlock);
        map.Insert(indexTempBlock, tempBlock);
    }

    return score;
}

bool Test(List<Cell<char>> map, Cell<char> startItem, Point move, bool draw)
{
    var unique = new HashSet<string>();

    //if(draw)
    //{
    //    Console.Clear();
    //    ConsoleLog.WriteMap2D(map);
    //    Console.ReadKey();
    //}

    var i = 0;
    var nextItem = startItem;
    while (true)
    {
        i++;
        var nextItemTemp = map.FirstOrDefault(x => x.RowId == nextItem.RowId + move.Y && x.ColumnId == nextItem.ColumnId + move.X);
        if (nextItemTemp.Value is default(char))
            return false;

        var key = $"{nextItemTemp.ColumnId}_{nextItemTemp.RowId}_{move.X}_{move.Y}";

        if (unique.Contains(key))
        {
            //if (draw)
            //{
            //    Console.ReadKey();
            //}
            return true;
        }

        unique.Add(key);

        if (nextItemTemp.Value == '#')
        {
            move.Rotate(new Point(0, 0), PointRotate.Left);
            continue;
        }

        //if (draw)
        //{
        //    ConsoleLog.WriteCellOnMap2D(new(nextItemTemp.RowId, nextItemTemp.ColumnId, 'X'), ConsoleColor.Green);
        //    Thread.Sleep(20);
        //}

        nextItem = nextItemTemp;
        continue;
    }

}