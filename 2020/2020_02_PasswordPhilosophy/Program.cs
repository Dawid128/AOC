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
    return input.Split(Environment.NewLine)
                .Select(S1)
                .Count(IsValidateOld);
}

object Part2(string input)
{
    return input.Split(Environment.NewLine)
                .Select(S1)
                .Count(IsValidateNew);
}

(char Character, int Min, int Max, string Text) S1(string input)
{
    var data = input.Split(" ");
    var range = data[0].Split("-");
    return (data[1].First(), int.Parse(range[0]), int.Parse(range[1]), data[2]);
}

bool IsValidateOld((char Character, int Min, int Max, string Text) input)
{
    var count = input.Text.Count(x => x == input.Character);
    return (count >= input.Min && count <= input.Max);
}

bool IsValidateNew((char Character, int First, int Second, string Text) input)
{
    var hasFirst = (input.Text[input.First - 1] == input.Character) ? 1 : 0;
    var hasSecond = (input.Text[input.Second - 1] == input.Character) ? 1 : 0;

    return (hasFirst + hasSecond == 1);
}