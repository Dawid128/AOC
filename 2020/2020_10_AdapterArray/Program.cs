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
    var adapters = input.Split(Environment.NewLine)
                        .Select(x => int.Parse(x))
                        .OrderByDescending(x => x)
                        .ToList();

    adapters.Insert(0, adapters.Max() + 3);
    adapters.Insert(adapters.Count, 0);

    var map = new Dictionary<int, int>() { { 1, 0 }, { 3, 0 } };
    for (int i = 0; i < adapters.Count - 1; i++)  
    {
        var current = adapters[i];
        var next = adapters[i + 1];

        var joltNumber = current - next;
        if (!map.ContainsKey(joltNumber))
            map.Add(joltNumber, 0);
        map[joltNumber]++;
    }


    return map[1] * map[3];
}

object Part2(string input)
{
    var adapters = input.Split(Environment.NewLine)
                        .Select(x => int.Parse(x))
                        .OrderByDescending(x => x)
                        .ToList();

    adapters.Insert(0, adapters.Max() + 3);
    adapters.Insert(adapters.Count, 0);

    var map = new Dictionary<int, HashSet<int>>(); //Adapter & ConnectionAdapters
    foreach (var currentAdapter in adapters.ToList())
    {
        adapters.Remove(currentAdapter);
        var previousAdapters = adapters.Take(adapters.Count >= 3 ? 3 : adapters.Count)
                                       .Where(x => x >= currentAdapter - 3)
                                       .ToHashSet();

        map.Add(currentAdapter, previousAdapters);
    }

    var mapScores = new Dictionary<int, long>();
    long TakeScore(HashSet<int> connectionAdapters)
    {
        long score = 0;
        foreach (var connectionAdapter in connectionAdapters)
            score += mapScores[connectionAdapter];

        return score;
    }
    foreach (var item in map.Reverse())
    {
        if (item.Key == 0)
        {
            mapScores.Add(0, 1);
            continue;
        }

        mapScores.Add(item.Key, TakeScore(item.Value));

    }
    return mapScores.Last().Value;
}
