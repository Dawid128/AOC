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
    var instructions = input.Split(Environment.NewLine)
                            .Select(x => (Name: string.Concat(x.Take(3)), Value: int.Parse(string.Concat(x.Skip(4)))))
                            .ToList();

    TryExecuteInstructions(instructions, out var accumulator);
    return accumulator;
}

object Part2(string input)
{
    var instructions = input.Split(Environment.NewLine)
                            .Select(x => (Name: string.Concat(x.Take(3)), Value: int.Parse(string.Concat(x.Skip(4)))))
                            .ToList();

    foreach (var (name, index) in instructions.Select((item, index) => (item.Name, Index: index)).Where(x => x.Name == "nop" || x.Name == "jmp")) 
    {
        var instructionsCopy = instructions.ToList();
        instructionsCopy[index] = (name == "nop" ? "jmp" : "nop", instructionsCopy[index].Value);
        if (TryExecuteInstructions(instructionsCopy, out var accumulator))
            return accumulator;
    }

    return -1;
}
bool TryExecuteInstructions(IList<(string Name, int Value)> instructions, out int accumulator)
{
    accumulator = 0;

    var visited = new HashSet<int>();
    var index = 0;
    while (true)
    {
        //Return false, when loop is infinite
        if (visited.Contains(index))
            return false;

        visited.Add(index);

        var (name, value) = instructions[index];
        switch (name)
        {
            case "nop":
                index++;
                break;
            case "acc":
                index++;
                accumulator += value;
                break;
            case "jmp":
                index += value;
                break;
        }

        //Return true, when the instructions in the loop meets the condition
        if (index >= instructions.Count)
            return true;

        index %= instructions.Count;
    }
}

int ExecuteInstructions(IList<(string Name, int Value)> instructions)
{
    var visited = new HashSet<int>();
    var accumulator = 0;
    var index = 0;
    while (true)
    {
        if (visited.Contains(index))
            break;
        visited.Add(index);

        var (name, value) = instructions[index];
        switch (name)
        {
            case "nop":
                index++;
                break;
            case "acc":
                index++;
                accumulator += value;
                break;
            case "jmp":
                index += value;
                break;
        }
        index %= instructions.Count;
    }

    return accumulator;
}
