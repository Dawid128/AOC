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
var output2 = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    List<long> ChangeStone(long value)
    {
        if (value == 0)
            return [1];

        var valueStr = $"{value}";
        if (valueStr.Length % 2 == 0)
        {
            var mid = valueStr.Length / 2;
            var firstHalf = valueStr[..mid];
            var secondHalf = valueStr[mid..];

            return [long.Parse(firstHalf), long.Parse(secondHalf)];
        }

        return [value * 2024];
    }

    var stones =  input.Split(" ")
                       .Select(long.Parse)
                       .ToList();

    var result = stones;  
    for(int i = 0;  i < 25; i++)
        result = result.SelectMany(ChangeStone).ToList();

    return result.Count;
}

object Part2(string input)
{
    List<long> ChangeStone(long value)
    {
        if (value == 0)
            return [1];

        var valueStr = $"{value}";
        if (valueStr.Length % 2 == 0)
        {
            var mid = valueStr.Length / 2;
            var firstHalf = valueStr[..mid];
            var secondHalf = valueStr[mid..];

            return [long.Parse(firstHalf), long.Parse(secondHalf)];
        }

        return [value * 2024];
    }

    var stones = input.Split(" ")
                      .Select(long.Parse)
                      .ToDictionary(x => x, x => (long)1);

    for (int i = 0; i < 75; i++)
    {
        var stonesTemp = new Dictionary<long, long>();
        foreach (var key in stones.Where(x=>x.Value > 0).Select(x=>x.Key).ToList())
        {
            foreach (var nextKey in ChangeStone(key))
            {
                if (!stonesTemp.ContainsKey(nextKey))
                    stonesTemp.Add(nextKey, 0);

                stonesTemp[nextKey] += stones[key];
            }
        }
        stones = stonesTemp;
    }

    return stones.Select(x=>x.Value).Sum();
}