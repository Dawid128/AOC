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
var output2 = Part2(input, 8);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.Select(s1 => s1.ToString())
                                 .Select(int.Parse)
                                 .ToArray())
                   .ToArray();

    var score = 0;
    foreach (var startPoint in map.WhereCell(x => x.Value == 0))
    {
        var nextPoints = new HashSet<Cell<int>>(){ startPoint};
        for (var i = 1; i <= 9; i++)
        {
            var nextPointsTemp = new HashSet<Cell<int>>();
            foreach (var nextPoint in nextPoints)
                nextPointsTemp.AddRange(map.SelectAdjacent4(nextPoint.RowId, nextPoint.ColumnId, x => x.Value == i).ToList());

            nextPoints = nextPointsTemp;
            if (nextPoints.Count == 0)
                break;
        }

        score += nextPoints.Count;
    }

    return score;
}

object Part2(string input, int maxLength)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.Select(s1 => s1.ToString())
                                 .Select(int.Parse)
                                 .ToArray())
                   .ToArray();

    var score = 0;
    foreach (var startPoint in map.WhereCell(x => x.Value == 0))
    {
        var nextPoints = new List<Cell<int>>() { startPoint };
        for (var i = 1; i <= 9; i++)
        {
            var nextPointsTemp = new List<Cell<int>>();
            foreach (var nextPoint in nextPoints)
                nextPointsTemp.AddRange(map.SelectAdjacent4(nextPoint.RowId, nextPoint.ColumnId, x => x.Value == i));

            nextPoints = nextPointsTemp;
            if (nextPoints.Count == 0)
                break;
        }

        score += nextPoints.Count;
    }

    return score;
}