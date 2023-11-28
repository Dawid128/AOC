using System.Diagnostics;
using AdventCodeExtension;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part(input, new[] { (3, 1) });
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part(input, new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) });
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part(string input, IList<(int X, int Y)> moves)
{
    var map = input.Split(Environment.NewLine)
                   .Select(SelectRow)
                   .ToArray();

    return moves.Select(x => CountTrees(map, x))
                .Product();
}

bool[] SelectRow(string row)
{
    var result = new bool[row.Length];

    for (int i = 0; i < result.Length; i++)
        result[i] = row[i] is '#';

    return result;
}

int CountTrees(bool[][] map, (int X, int Y) move)
{
    var height = map.Length;
    var width = map[0].Length;

    var count = 0;
    (int X, int Y) position = new(0, 0);

    while (true)
    {
        position.X += move.X;
        if (position.X >= width)
            position.X -= width;

        position.Y += move.Y;
        if (position.Y >= height)
            return count;

        if (map[position.Y][position.X])
            count++;
    }
}