//[MultiSplit][Intersect][RecursionVsIteration][RecursionAndCache][IterationAndQueue]
using AdventCodeExtension;
using System.Diagnostics;

var cache = new Dictionary<int, int>(); //Count win numbers for the index of card
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
                .Select(l => l.Split(": ")[1]                                       //Get second part of split
                              .Split(" | ")                                         //Split to 2 sets: list of winning numbers & list of numbers you have
                              .Select(n => n.Split(" ")                             //Split and Parse to integer: each number
                                            .Where(x => !string.IsNullOrEmpty(x))
                                            .Select(x => int.Parse(x))
                                            .ToArray())
                              .ToArray())
                .Select(n => n[0].Intersect(n[1]).Count() - 1)                      //How much numbers from your list, is in winning list
                .Where(x => x >= 0)
                .Select(n => Math.Pow(2, n))                                        //one card give points 2^(n-1) where n is count of win numbers
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

    //Use the solution recursion with the cache
    //
    var result = 0;
    for (int i = cards.Count - 1; i >= 0; i--) 
    {
        var card = cards[i];
        result += CountWinRecursion(cards, i);
    }

    return result;
}


int CountWinRecursion(IList<int[][]> cards, int index)
{
    if (cache.TryGetValue(index, out var resultCache))
        return resultCache;

    int result = 1;
    var card = cards[index];

    for (int i = index + 1; i <= index + CountWin(card); i++)
        result += CountWinRecursion(cards, i);

    cache.Add(index, result);
    return result;
}

int CountWin(int[][] card) => card[0].Intersect(card[1]).Count();

//[OLD] Get score with iteration queue
//object Part2(string input)
//{
//    var cards = input.Split(Environment.NewLine)
//                     .Select(l => l.Split(": ")[1]
//                                   .Split(" | ")
//                                   .Select(n => n.Split(" ")
//                                                 .Where(x => !string.IsNullOrEmpty(x))
//                                                 .Select(x => int.Parse(x))
//                                                 .ToArray())
//                                   .ToArray())
//                     .ToList();

//    int CountWin(int[][] numbers) => numbers[0].Intersect(numbers[1]).Count();

//    //var cache = new Dictionary<int, int>();
//    var result = 0;
//    var queueCards = new Queue<(int Index, int CountWin)>(cards.Select((x, index) => (Index: index, CountWin: CountWin(cards[index]))));
//    while (queueCards.Count > 0)
//    {
//        result++;
//        var (index, countWin) = queueCards.Dequeue();
//        if (countWin == 0)
//            continue;

//        for (int i = index + 1; i <= index + countWin; i++)
//            queueCards.Enqueue((i, CountWin(cards[i])));
//    }

//    return result;
//}