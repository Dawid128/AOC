using AdventCodeExtension;
using AdventCodeExtension.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

    var cache = new HashSet<Cell<DirectionEnum>>();
    bool ValidAdjacent(Cell<DirectionEnum> currentCell, Cell<DirectionEnum> adjacentCell)
    {
        if (cache.Contains(adjacentCell))
            return false;

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

    var startCell = map.WhereCell(x => x.Value == DirectionEnum.All).Single();
    cache.Add(startCell);

    var nextCells = map.SelectAdjacent4(startCell.RowId, startCell.ColumnId, x => ValidAdjacent(startCell, x)).ToList();
    foreach (var nextCell in nextCells)
        cache.Add(nextCell);

    var count = 1;
    while (true) 
    {
        if (nextCells.Count != nextCells.Distinct().Count())
            return count;

        var newCells = new List<Cell<DirectionEnum>>();
        foreach (var nextCell in nextCells)
            newCells.AddRange(map.SelectAdjacent4(nextCell.RowId, nextCell.ColumnId, x => ValidAdjacent(nextCell, x)));

        foreach (var newCell in newCells)
            cache.Add(newCell);

        nextCells = newCells;
        count++;
    }
}

object Part2(string input)
{
    return -1;
}

DirectionEnum[][] Parse(string input)
{
    DirectionEnum GetDirection(char c)
    => c switch
    {
        '|' => DirectionEnum.North | DirectionEnum.South,
        '-' => DirectionEnum.East | DirectionEnum.West,
        'L' => DirectionEnum.North | DirectionEnum.East,
        'J' => DirectionEnum.North | DirectionEnum.West,
        '7' => DirectionEnum.South | DirectionEnum.West,
        'F' => DirectionEnum.South | DirectionEnum.East,
        'S' => DirectionEnum.All,
        _ => DirectionEnum.None
    };

    return input.Split(Environment.NewLine)
                .Select(x => x.Select(GetDirection)
                              .ToArray())
                .ToArray();
}

[Flags]
public enum DirectionEnum
{
    None = 0,
    North = 1,
    Top = North,
    South = 2,
    Bottom = South,
    West = 4,
    Left = West,
    East = 8,
    Right = East,
    All = North | South | East | West,
}

public static class EnumHelper
{
    public static DirectionEnum GetOppositeDirection(DirectionEnum direction)
    {
        var result = DirectionEnum.None;

        if (direction.HasFlag(DirectionEnum.North))
            result |= DirectionEnum.South;

        if (direction.HasFlag(DirectionEnum.South))
            result |= DirectionEnum.North;

        if (direction.HasFlag(DirectionEnum.East))
            result |= DirectionEnum.West;

        if (direction.HasFlag(DirectionEnum.West))
            result |= DirectionEnum.East;

        return result;
    }

    public static bool HasAnyFlag(DirectionEnum enumFirst, DirectionEnum enumSecond) => (enumFirst & enumSecond) != 0;
}