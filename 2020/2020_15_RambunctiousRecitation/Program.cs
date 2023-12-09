using AdventCodeExtension;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part(input, 2020);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part(input, 30000000);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part(string input, int round)
{
    var data = input.Split(",")
                    .Select(int.Parse)
                    .Select((value, index) => new { value, index })
                    .ToDictionary(x => x.value, x => (CycleLength: 0, LastRound: x.index + 1));

    var lastRound = data.Values.Max().LastRound;
    var lastNumber = data.Keys.Last();
    while (lastRound < round)
    {
        lastRound++;

        //If Number was spoken before (more than 1 time)
        if (data.TryGetValue(lastNumber, out var whenRound) && whenRound.CycleLength > 0)
            lastNumber = whenRound.CycleLength;
        //If Number is spoken first time
        else
            lastNumber = 0;

        if (data.ContainsKey(lastNumber))
            data[lastNumber] = (lastRound - data[lastNumber].LastRound, lastRound);
        else
            data.Add(lastNumber, (0, lastRound));
    }

    return lastNumber;
}