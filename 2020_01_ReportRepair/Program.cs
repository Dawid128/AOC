using System.Diagnostics;
using AdventCodeExtension;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
//var output = Part1(input, 2020); //181044
var output = Part(input, 2, 2020);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
//output = Part2(input, 2020); //82660352
output = Part(input, 3, 2020);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, int requiredSum)
{
    var data = input.Split(Environment.NewLine)
                .Select(x => int.Parse(x))
                .ToList();
    
    for (int i = 0; i < data.Count; i++)
        for (int j = i + 1; j < data.Count; j++)
            if (data[i] + data[j] == requiredSum)
                return data[i] * data[j];

    return -1;
}

object Part2(string input, int requiredSum)
{
    var data = input.Split(Environment.NewLine)
                .Select(x => int.Parse(x))
                .ToList();

    for (int i = 0; i < data.Count; i++)
        for (int j = i + 1; j < data.Count; j++)
            for (int k = j + 1; k < data.Count; k++)
                if (data[i] + data[j] + data[k] == requiredSum)
                    return data[i] * data[j] * data[k];

    return -1;
}

object Part(string input, int count, int requiredSum)
{
    var data = input.Split(Environment.NewLine)
                .Select(x => int.Parse(x))
                .ToList();

    var nextCombinationArray = Enumerable.Range(0, count)
                                         .ToArray();

    while (true)
    {
        var sum = data.ElementsAt(nextCombinationArray)
                      .Sum();

        if (sum == requiredSum)
            return data.ElementsAt(nextCombinationArray)
                       .Product();

        if (!Next(nextCombinationArray, nextCombinationArray.Length - 1, data.Count - 1))
            return -1;
    }
}

bool Next(int[] input, int index, int maxValue)
{
    if (index < 0)
        return false;

    if (input[index] < maxValue)
    {
        input[index]++;
        for (int i = index + 1; i < input.Length; i++) 
            input[i] = input[i - 1] + 1;

        return true;
    }

    return Next(input, index - 1, maxValue - 1);
}