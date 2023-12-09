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
                   .Select(S1)
                   .ToDictionary(x => x.Bag, x => x.AggregationBags);

    var usedBags = new HashSet<string>();
    var parentBags = new Queue<KeyValuePair<string, Dictionary<string, int>>>(map.Where(x => x.Value.ContainsKey("shinygold")).ToList());
    while (parentBags.Any())
    {
        var parentBag = parentBags.Dequeue();
        usedBags.Add(parentBag.Key);
        foreach (var nextParentBag in map.Where(x => x.Value.ContainsKey(parentBag.Key)))
            if (!usedBags.Contains(nextParentBag.Key))
                parentBags.Enqueue(map.Single(x => x.Key == nextParentBag.Key));
    }
    return usedBags.Count;
}

object Part2(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(S1)
                   .ToDictionary(x => x.Bag, x => x.AggregationBags);

    //Recursion
    var cache = new Dictionary<string, int>();
    int TakeCountBags(string bag, Dictionary<string, int> aggregationBags)
    {
        var count = 1;
        foreach (var aggregationBag in aggregationBags)
        {
            if (cache.TryGetValue(aggregationBag.Key, out int value))
            {
                count += value * aggregationBag.Value;
                continue;
            }

            var childAggregationBags = map[aggregationBag.Key];
            var countAggregationBag = TakeCountBags(aggregationBag.Key, childAggregationBags);
            count += countAggregationBag * aggregationBag.Value;
            cache.Add(aggregationBag.Key, countAggregationBag);
        }
        return count;
    }

    return TakeCountBags("shinygold", map["shinygold"]) - 1;
}

(string Bag, Dictionary<string, int> AggregationBags) S1(string input)
{
    var split = input.Split(" bags contain ");
    var bag = split[0].Replace(" ", string.Empty);
    var aggregationBags = new Dictionary<string, int>();

    if(split[1] != "no other bags.")
    {
        var aggregationBagsStr = split[1].Replace(".", string.Empty)
                                         .Replace(" bags", string.Empty)
                                         .Replace(" bag", string.Empty)
                                         .Split(", ");


        foreach (var aggregationBagStr in aggregationBagsStr)
        {
            var part1 = new string(aggregationBagStr.TakeWhile(char.IsDigit).ToArray());
            var part2 = new string(aggregationBagStr.SkipWhile(char.IsDigit).ToArray());

            var count = int.Parse(part1);
            var aggregationBag = part2.Replace(" ", string.Empty);
            aggregationBags.Add(aggregationBag, count);
        }
    }

    return new(bag, aggregationBags);
}