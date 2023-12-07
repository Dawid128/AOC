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
output = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, IList<char> cards)
{
    return input.Split(Environment.NewLine)
                .Select(x => x.Split(" "))
                .Select(x => (Cards: x[0].Select(s => cards.IndexOf(s)).ToArray(), Score: int.Parse(x[1].Replace(" ", string.Empty))))
                .Select(x => (x.Cards, x.Score, HandType: GetHandType(x.Cards)))
                .OrderBy(x => x.Cards[4])
                .OrderBy(x => x.Cards[3])
                .OrderBy(x => x.Cards[2])
                .OrderBy(x => x.Cards[1])
                .OrderBy(x => x.Cards[0])
                .OrderBy(x => x.HandType)
                .Select((x, index) => x.Score * (index + 1))
                .Sum();
}

object Part2(string input)
{
    return -1;
}

int GetHandType(IList<int> cards)
{
    var groups = cards.GroupBy(x => x).ToList();
    if (groups.Count == 1)
        return 7;

    if (groups.Count == 2 && groups.Any(x => x.Count() == 4))
        return 6;

    if (groups.Count == 2 && groups.Any(x => x.Count() == 3))
        return 5;

    if (groups.Count == 3 && groups.Any(x => x.Count() == 3))
        return 4;

    if (groups.Count == 3 && groups.Count(x => x.Count() == 2) == 2)
        return 3;

    if (groups.Count == 4 && groups.Any(x => x.Count() == 2)) 
        return 2;

    return 1;
}