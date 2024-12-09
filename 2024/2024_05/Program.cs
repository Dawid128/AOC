using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input, out var rules, out var wrongOrders);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
var output2 = Part2(rules, wrongOrders);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, out Dictionary<int, List<int>> rules, out List<int[]> wrongOrders)
{
    var split = input.Split(Environment.NewLine + Environment.NewLine);

    rules = split[0].Split(Environment.NewLine)
                    .Select(x => x.Split('|')
                                  .Select(int.Parse)
                                  .ToArray())
                    .GroupBy(x => x[0])
                    .ToDictionary(x => x.Key, x => x.SelectMany(y => y.Skip(1))
                                                    .ToList());

    var orders = split[1].Split(Environment.NewLine)
                         .Select(x => x.Split(',')
                                       .Select(int.Parse)
                                       .ToArray())
                         .ToList();

    wrongOrders = [];
    var score = 0;
    foreach (var order in orders)
    {
        var valid = true;
        for (int i = 0; i < order.Length - 1; i++)
        {
            var currentNumber = order[i];
            var nextNumber = order[i + 1];

            if (!rules.TryGetValue(currentNumber, out var validNextNumbers))
            {
                valid = false;
                break;
            }

            if(!validNextNumbers.Contains(nextNumber))
            {
                valid = false;
                break;
            }
        }

        if (valid)
            score += order[(order.Length + 1) / 2 - 1];
        else
            wrongOrders.Add(order);
    }

    return score;
}

object Part2(Dictionary<int, List<int>> rules, List<int[]> wrongOrders)
{
    var score = 0;
    foreach (var order in wrongOrders.Select(x => x.ToList())) 
    {
        for (int i = 0; i < order.Count - 1; i++)
        {
            var currentNumber = order[i];
            if (!rules.ContainsKey(currentNumber) || !order.Skip(i + 1).All(x => rules[currentNumber].Contains(x)))
            {
                order.RemoveAt(i);
                order.Add(currentNumber);
                i--;
                continue;
            }
        }

        score += order[(order.Count + 1) / 2 - 1];
    }

    return score;
}