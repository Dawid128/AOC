//[ThroughInstructionsinfinity][LCM-NWW][GCD-NWD][MOD]
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
    var (instructions, map) = Parse(input);
    return CountSteps(instructions, map, "AAA", new[] { "ZZZ" });
}

object Part2(string input)
{
    var (instructions, map) = Parse(input);

    var endWithA = map.Where(x => x.Key[2] == 'A').Select(x => x.Key).ToList();
    var endWithZ = map.Where(x => x.Key[2] == 'Z').Select(x => x.Key).ToList();

    var numbers = new List<long>();
    foreach (var nextItem in endWithA)
        numbers.Add(CountSteps(instructions, map, nextItem, endWithZ));

    return MathAdvanced.CalculateLCM(numbers);
}

(int[] Instructions, Dictionary<string, string[]> Map) Parse(string input)
{
    var split = input.Split(Environment.NewLine + Environment.NewLine);

    var instructions = split[0].Select(x => x == 'R' ? 1 : 0)
                               .ToArray();

    var map = split[1].Replace("=", string.Empty)
                      .Replace("(", string.Empty)
                      .Replace(",", string.Empty)
                      .Replace(")", string.Empty)
                      .Split(Environment.NewLine)
                      .Select(x => x.Split(" ")
                                    .Where(y => y != string.Empty)
                                    .ToArray())
                      .ToDictionary(x => x[0], x => x.Skip(1).ToArray());

    return (instructions, map);
}

int CountSteps(int[] instructions, Dictionary<string, string[]> map, string startItem, IList<string> endItems)
{
    var count = 1;
    var index = 0;

    var currentItem = map[startItem];
    while (true)
    {
        var instruction = instructions.ElementAt(index);
        var nextItem = currentItem[instruction];
        if (endItems.Contains(nextItem)) 
            return count;

        count++;
        index++;
        index %= instructions.Length;
        currentItem = map[nextItem];
    }
}