//[Regex]
using AdventCodeExtension;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
    var array2D = input.Split(Environment.NewLine)
                       .Select(x => x.ToCharArray())
                       .ToArray()
                       .To2DArray();

    int count = 0;

    //Horizontal
    foreach (var horizontal in array2D.TakeRows())
    {
        count += Regex.Matches(new string(horizontal), Regex.Escape("XMAS")).Count;
        count += Regex.Matches(new string(horizontal.Reverse().ToArray()), Regex.Escape("XMAS")).Count;
    }

    //Vertical
    foreach (var vertical in array2D.TakeColumns())
    {
        count += Regex.Matches(new string(vertical), Regex.Escape("XMAS")).Count;
        count += Regex.Matches(new string(vertical.Reverse().ToArray()), Regex.Escape("XMAS")).Count;
    }

    //Diagonals
    foreach (var diagonal in array2D.TakeDiagonals()) 
    {
        count += Regex.Matches(new string(diagonal), Regex.Escape("XMAS")).Count;
        count += Regex.Matches(new string(diagonal.Reverse().ToArray()), Regex.Escape("XMAS")).Count;
    }

    //Diagonals II
    foreach (var diagonal in array2D.FlipHorizontal().TakeDiagonals())
    {
        count += Regex.Matches(new string(diagonal), Regex.Escape("XMAS")).Count;
        count += Regex.Matches(new string(diagonal.Reverse().ToArray()), Regex.Escape("XMAS")).Count;
    }

    return count;
}

object Part2(string input, int maxLength)
{
    var array2D = input.Split(Environment.NewLine)
                       .Select(x => x.ToCharArray())
                       .ToArray()
                       .To2DArray();

    int count = 0;

    for (int i = 0; i < array2D.GetLength(0) - 2; i++)
    {
        for (int j = 0; j < array2D.GetLength(1) - 2; j++)
        {
            var array2DTemp = array2D.TakeRange((i, j), (i + 2, j + 2));
            if (array2DTemp[1, 1] != 'A')
                continue;

            if (GetCount(array2DTemp) != 2) 
                continue;

            count++;
        }
    }

    return count;
}

int GetCount(char[,] array2D)
{
    var count = 0;

    //Diagonals
    foreach (var diagonal in array2D.TakeDiagonals())
    {
        count += Regex.Matches(new string(diagonal), Regex.Escape("MAS")).Count;
        count += Regex.Matches(new string(diagonal.Reverse().ToArray()), Regex.Escape("MAS")).Count;
    }

    //Diagonals II
    foreach (var diagonal in array2D.FlipHorizontal().TakeDiagonals())
    {
        count += Regex.Matches(new string(diagonal), Regex.Escape("MAS")).Count;
        count += Regex.Matches(new string(diagonal.Reverse().ToArray()), Regex.Escape("MAS")).Count;
    }

    return count;
}

partial class Program
{
    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MyRegex();
}