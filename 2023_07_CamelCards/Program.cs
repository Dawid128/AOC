//[Order][OrderbyVsThenBy]
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input, new[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' }); //30
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input, new[] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' }); //50
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, IList<char> cards)
{
    return input.Split(Environment.NewLine)
                .Select(x => x.Split(" "))
                .Select(x => (Cards: x[0].Select(s => cards.IndexOf(s)).ToArray(), Score: int.Parse(x[1].Replace(" ", string.Empty))))
                .Select(x => (x.Cards, x.Score, HandType: CalculateHandType(x.Cards)))
                .SortCards()
                .Select((x, index) => x.Score * (index + 1))
                .Sum();
}

object Part2(string input, IList<char> cards)
{
    return input.Split(Environment.NewLine)
                .Select(x => x.Split(" "))
                .Select(x => (Cards: x[0].Select(s => cards.IndexOf(s)).ToArray(), Score: int.Parse(x[1].Replace(" ", string.Empty))))
                .Select(x => (x.Cards, x.Score, HandType: CalculateHandTypeWithJoker(x.Cards)))
                .SortCards()
                .Select((x, index) => x.Score * (index + 1))
                .Sum();
}

int CalculateHandType(IList<int> cards)
{
    var groups = cards.OrderByDescending(x => x)
                      .GroupBy(x => x)
                      .ToDictionary(x => x.Key, x => x.Count());

    return GetHandType(groups);
}

int CalculateHandTypeWithJoker(IList<int> cards)
{
    var groups = cards.OrderByDescending(x => x)
                      .GroupBy(x => x)
                      .ToDictionary(x => x.Key, x => x.Count());

    //SKIP "JJJJJ" 
    //BE CAREFUL "3xJ" -> skip J (0) when search max
    if (groups.TryGetValue(0, out var count) && count < 5)
    {
        var max = groups.Where(x => x.Key != 0).MaxBy(x => x.Value);
        groups.Remove(max.Key);
        groups.Remove(0);
        groups.Add(max.Key, max.Value + count);
    }

    return GetHandType(groups);
}

int GetHandType(Dictionary<int, int> groups)
{
    if (groups.Count == 1)
        return 7;

    if (groups.Count == 2 && groups.Any(x => x.Value == 4))
        return 6;

    if (groups.Count == 2 && groups.Any(x => x.Value == 3))
        return 5;

    if (groups.Count == 3 && groups.Any(x => x.Value == 3))
        return 4;

    if (groups.Count == 3 && groups.Count(x => x.Value == 2) == 2)
        return 3;

    if (groups.Count == 4 && groups.Any(x => x.Value == 2))
        return 2;

    return 1;
}

static class Extensions
{
    public static IOrderedEnumerable<(int[] Cards, int Score, int HandType)> SortCards(this IEnumerable<(int[] Cards, int Score, int HandType)> cards) => cards.OrderBy(x => x.HandType)
                                                                                                                                                               .ThenBy(x => x.Cards[0])
                                                                                                                                                               .ThenBy(x => x.Cards[1])
                                                                                                                                                               .ThenBy(x => x.Cards[2])
                                                                                                                                                               .ThenBy(x => x.Cards[3])
                                                                                                                                                               .ThenBy(x => x.Cards[4]);
}