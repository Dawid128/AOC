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
    var split = input.Split(Environment.NewLine + Environment.NewLine);
    var workslows = WorkflowProcessor.ParseToWorkflows(split[0]);
    var itemRatings = WorkflowProcessor.ParseToItemRatings(split[1]);

    var score = 0;
    foreach (var itemRating in itemRatings)
        if (WorkflowProcessor.CheckIfAccepted(itemRating, workslows))
            score += itemRating.Values.Sum();

    return score;
}

object Part2(string input)
{
    return -1;
}

public static class WorkflowProcessor
{
    public static Dictionary<string, List<(string? Key, int Value, string? Operator, string Result)>> ParseToWorkflows(string input)
    {
        (string Key, List<(string? Key, int Value, string? Operator, string Result)> Value) S1(string part)
        {
            var split = part.Replace("}", string.Empty).Split("{");

            return (split[0], split[1].Split(",").Select(S2).ToList());
        }

        (string? Key, int Value, string? Operator, string Result) S2(string part)
        {
            var split = part.Split(":");
            if (split.Length == 1)
                return (default, default, default, split[0]);

            if (split[0].Contains('>'))
            {
                var partSplit = split[0].Split(">");
                return (partSplit[0], int.Parse(partSplit[1]), ">", split[1]);
            }

            if (split[0].Contains('<'))
            {
                var partSplit = split[0].Split("<");
                return (partSplit[0], int.Parse(partSplit[1]), "<", split[1]);
            }

            throw new Exception("Invalid input. ParseToWorkflows failed");
        }

        return input.Split(Environment.NewLine)
                    .Select(S1)
                    .ToDictionary(x => x.Key, x => x.Value);
    }

    public static List<Dictionary<string, int>> ParseToItemRatings(string input)
    {
        Dictionary<string, int> S1(string part)
        {
            var split = part[1..^1].Split(",");

            return split.Select(x => x.Split("="))
                        .ToDictionary(x => x[0], x => int.Parse(x[1]));
        }

        return input.Split(Environment.NewLine)
                    .Select(S1)
                    .ToList();
    }

    public static bool CheckIfAccepted(Dictionary<string, int> itemRating, Dictionary<string, List<(string? Key, int Value, string? Operator, string Result)>> workflows)
    {
        var instructions = workflows["in"];
        while (true)
        {
            foreach (var (key, value, @operator, result) in instructions)
            {
                var success = @operator switch
                {
                    ">" when key is not null => itemRating[key] > value,
                    "<" when key is not null => itemRating[key] < value,
                    _ => true
                };

                if (!success)
                    continue;

                if (result == "A")
                    return true;

                if (result == "R")
                    return false;

                instructions = workflows[result];
                break;
            }
        }
    }
}

public class Workflow
{
    public string Name { get; }
    public List<WorkflowRule> Rules { get; }

    public Workflow(string name, IList<WorkflowRule> rules)
    {
        Name = name;
        Rules = [.. rules];
    }
}

public class WorkflowRule
{
    public string Key { get; }
    public string Operator { get; }
    public int Value { get; }
    public string Result { get; }

    public WorkflowRule(string key, string @operator, int value, string result)
    {
        Key = key;
        Operator = @operator;
        Value = value;
        Result = result;
    }
}