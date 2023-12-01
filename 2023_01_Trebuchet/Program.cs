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
output = Part2(input, new Dictionary<int, string>() { { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" }, { 5, "five" }, { 6, "six" }, { 7, "seven" }, { 8, "eight" }, { 9, "nine" } });
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    return input.Split(Environment.NewLine)
                .Select(x => x.Where(x => int.TryParse(x.ToString(), out _)).Select(x => x.ToString()).ToArray())
                .Select(x => int.Parse(x.First() + x.Last()))
                .Aggregate((x, y) => x + y);
}

object Part2(string input, Dictionary<int, string> numberTextList)
{
    bool TryGetNumber(string text, string character, out int number)
    {
        if (int.TryParse(character, out number))
            return true;

        var numberText = numberTextList.FirstOrDefault(x => text.Contains(x.Value));
        if (numberText.Key is not 0)
        {
            number = numberText.Key;
            return true;
        }

        return false;
    }

    int SelectNumber(string text)
    {
        var result = string.Empty;

        var temp = string.Empty;
        foreach (var character in text)
        {
            temp += character;
            if (TryGetNumber(temp, character.ToString(), out var number))
            {
                result += number;
                break;
            }
        }

        temp = string.Empty;
        foreach (var character in text.Reverse())
        {
            temp = temp.Insert(0, character.ToString());
            if (TryGetNumber(temp, character.ToString(), out var number))
            {
                result += number;
                break;
            }
        }

        return int.Parse(result);
    }

    return input.Split(Environment.NewLine)
                .Select(SelectNumber)
                .Aggregate((x, y) => x + y);
}