using AdventCodeExtension;
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
                   .To2DArray()
                   .RotateLeft()
                   .ToJaggedArray();

    var result = 0;
    foreach (var row in map)
    {
        var ratio = row.Length;
        for (var i = 0; i < row.Length; i++)
        {
            if (row[i] == '.')
                continue;

            if (row[i] == '#')
            {
                ratio = row.Length - (i + 1);
                continue;
            }

            result += ratio;
            ratio--;
        }
    }

    return result;
}

object Part2(string input)
{
    return -1;
}
