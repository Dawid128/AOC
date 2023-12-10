//[Array2D][Map][ShapeInsideAndOutside][Directions][Adjacents][Pipes][CheckIfInsideOrOutsideShape][EvenOrOdd]
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
    var pipes = ConnectPipes(map);

    return pipes.Count / 2;
}

object Part2(string input)
{
    var map = Parse(input);
    var pipes = ConnectPipes(map);

    int count = 0;
    for (int y = 0; y < map.Length; y++)
    {
        var pipeTopCount = 0;
        var pipeBottomCount = 0;
        for (int x = 0; x < map[y].Length; x++)
        {
            var cell = new Cell<DirectionEnum>(y, x, map[y][x]);
            if (pipes.Contains(cell))
            {
                var value = cell.Value;
                if (value.HasFlag(DirectionEnum.Bottom | DirectionEnum.Right))
                    pipeBottomCount++;
                if (value.HasFlag(DirectionEnum.Bottom | DirectionEnum.Left))
                    pipeBottomCount++;

                if (value.HasFlag(DirectionEnum.Top | DirectionEnum.Right))
                    pipeTopCount++;
                if (value.HasFlag(DirectionEnum.Top | DirectionEnum.Left))
                    pipeTopCount++;

                if (value.HasFlag(DirectionEnum.Top | DirectionEnum.Bottom))
                {
                    pipeBottomCount++;
                    pipeTopCount++;
                }

                continue;
            }

            if (pipeTopCount % 2 != 0 && pipeBottomCount % 2 != 0)
                count++;
        }
    }

    return count;
}

DirectionEnum[][] Parse(string input)
{
    DirectionEnum GetDirection(char c)
    => c switch
    {
        '|' => DirectionEnum.Top | DirectionEnum.Bottom,
        '-' => DirectionEnum.Right | DirectionEnum.Left,
        'L' => DirectionEnum.Top | DirectionEnum.Right, 
        'J' => DirectionEnum.Top | DirectionEnum.Left,
        '7' => DirectionEnum.Bottom | DirectionEnum.Left,
        'F' => DirectionEnum.Bottom | DirectionEnum.Right,
        'S' => DirectionEnum.All,
        _ => DirectionEnum.None
    };

    return input.Split(Environment.NewLine)
                .Select(x => x.Select(GetDirection)
                              .ToArray())
                .ToArray();
}

HashSet<Cell<DirectionEnum>> ConnectPipes(DirectionEnum[][] map)
{
    //Start from single item "S"
    var pipes = new HashSet<Cell<DirectionEnum>>();
    var startCell = map.WhereCell(x => x.Value == DirectionEnum.All).Single();
    var nextCells = new List<Cell<DirectionEnum>>() { startCell };
    while (true)
    {
        var newCells = new List<Cell<DirectionEnum>>();
        foreach (var nextCell in nextCells)
            newCells.AddRange(map.SelectAdjacent4(nextCell.RowId, nextCell.ColumnId, x => ValidAdjacent(nextCell, x)));

        nextCells = [];
        foreach (var newCell in newCells)
            if (!pipes.Contains(newCell))
            {
                nextCells.Add(newCell);
                pipes.Add(newCell);
            }

        if (nextCells.Count == 0)
            return pipes;
    }
}

bool ValidAdjacent(Cell<DirectionEnum> currentCell, Cell<DirectionEnum> adjacentCell)
{
    //Adjacent from top
    if (adjacentCell.RowId < currentCell.RowId)
        return currentCell.Value.HasFlag(DirectionEnum.Top) && adjacentCell.Value.HasFlag(DirectionEnum.Bottom);

    //Adjacent from right
    if (adjacentCell.ColumnId > currentCell.ColumnId)
        return currentCell.Value.HasFlag(DirectionEnum.Right) && adjacentCell.Value.HasFlag(DirectionEnum.Left);

    //Adjacent from bottom
    if (adjacentCell.RowId > currentCell.RowId)
        return currentCell.Value.HasFlag(DirectionEnum.Bottom) && adjacentCell.Value.HasFlag(DirectionEnum.Top);

    //Adjacent from left
    if (adjacentCell.ColumnId < currentCell.ColumnId)
        return currentCell.Value.HasFlag(DirectionEnum.Left) && adjacentCell.Value.HasFlag(DirectionEnum.Right);

    return false;
}

[Flags]
public enum DirectionEnum
{
    None = 0,
    Top = 1,
    Right = 2,
    Bottom = 4,
    Left = 8,
    All = Top | Right | Bottom | Left,
}