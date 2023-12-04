using AdventCodeExtension;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input); //15
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input); //30
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    return input.Split(Environment.NewLine)
                .Select(l => l.Split(": ")[1]
                              .Split(" | ")
                              .Select(n => n.Split(" ")
                                            .Where(x => !string.IsNullOrEmpty(x))
                                            .Select(x => int.Parse(x))
                                            .ToArray())
                              .ToArray())
                .Select(n => n[0].Intersect(n[1]).Count() - 1)
                .Where(x => x >= 0)
                .Select(n => Math.Pow(2, n))
                .Aggregate((x, y) => x + y);
}

object Part2(string input)
{
    var cards = input.Split(Environment.NewLine)
                     .Select(l => l.Split(": ")[1]
                                   .Split(" | ")
                                   .Select(n => n.Split(" ")
                                                 .Where(x => !string.IsNullOrEmpty(x))
                                                 .Select(x => int.Parse(x))
                                                 .ToArray())
                                   .ToArray())
                     .ToList();

    int CountWin(int[][] numbers) => numbers[0].Intersect(numbers[1]).Count();

    //var cache = new Dictionary<int, int>();
    var result = 0;
    var queueCards = new Queue<(int Index, int CountWin)>(cards.Select((x, index) => (Index: index, CountWin: CountWin(cards[index]))));
    while (queueCards.Count > 0)
    {
        result++;
        var (index, countWin) = queueCards.Dequeue();
        if (countWin == 0)
            continue;

        for (int i = index + 1; i <= index + countWin; i++) 
            queueCards.Enqueue((i, CountWin(cards[i])));
    }

    return result;
}
