//[AggregateWithStartItem][TAccumulate][Seed][Instructions][Tuple][MultiSelect][CreateListWithNET8]
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
    return input.Split(",")
                .Select(x => x.Select(c => (int)c)
                              .Aggregate(0, (n1, n2) => ((n1 + n2) * 17) % 256))
                .Sum();
}

object Part2(string input)
{
    var data = input.Split(",")
                    .Select(x => (Name: x.Contains('=') ? x.Split("=")[0] : x.Split("-")[0],
                                  Action: x.Contains('=') ? '=' : '-',
                                  Value: x.Contains("=") ? int.Parse(x.Split("=")[1]) : 0))
                    .Select(x=> (x.Name, x.Action, x.Value, Code: x.Name.Select(c => (int)c).Aggregate(0, (n1, n2) => ((n1 + n2) * 17) % 256)))
                    .ToList();

    var boxes = new Dictionary<int, List<(string Name, int Value)>>();
    foreach (var (name, action, value, code) in data)
    {
        if (!boxes.TryGetValue(code, out var list))
        {
            if (action == '=')
                boxes.Add(code, [(name, value)]);
            continue;
        }

        var exist = list.SingleOrDefault(x => x.Name == name);
        if (exist.Name is null)
        {
            if (action == '=')
                list.Add((name, value));
            continue;
        }

        var index = list.IndexOf(exist);
        list.RemoveAt(index);
        if (action == '=')
            list.Insert(index, (name, value));
    }

    return boxes.SelectMany(x => x.Value.Select(s => (x.Key + 1) * (x.Value.IndexOf(s) + 1) * s.Value))
                .Sum();
}
