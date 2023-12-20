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
    var modules = input.Split(Environment.NewLine)
                       .Select(Parse)
                       .ToDictionary(x => x.Key, x => (x.Type, x.HighState, x.Output));

    (int LowPulseCount, int HighPulseCount) counter = new(0, 0);
    for (int i = 1; i <= 1000; i++)
        Execute(modules, ref counter);

    return counter.LowPulseCount * counter.HighPulseCount;
}

object Part2(string input)
{
    return -1;
}

(string Key, string Type, bool HighState, List<string> Output) Parse(string input)
{
    (string Key, string Type) SelectKeyAndType(string part)
    {
        if (part[0] == '%')
            return ((part[1..], "%"));

        if (part[0] == '&')
            return ((part[1..], "&"));

        return ((part, "S"));
    }

    var split = input.Split(" -> ");
    var (key, type) = SelectKeyAndType(split[0]);

    return (key, type, false, split[1].Split(", ").ToList());
}

void Execute(Dictionary<string, (string Type, bool HighState, List<string> OutputModules)> modules, ref (int LowPulseCount, int HighPulseCount) counter)
{
    counter.LowPulseCount++; //Every time, it is from button to broadcaster

    Queue<(string Key, bool HighPulse)> queue = [];
    foreach (var nextModuleKey in modules["broadcaster"].OutputModules)
        queue.Enqueue((nextModuleKey, false));

    while (queue.Count > 0)
    {
        Queue<(string Key, bool HighPulse)> tempQueue = [];
        while (queue.Count > 0)
        {
            var (key, highPulse) = queue.Dequeue();
            _ = highPulse ? counter.HighPulseCount++ : counter.LowPulseCount++;

            if (!modules.ContainsKey(key))
                continue;

            var module = modules[key];

            if (module.Type != "%" & module.Type != "&")
                throw new Exception($"Wrong module type: {module.Type}");

            if (module.Type == "%")
            {
                //If input is 1, do nothing
                if (highPulse)
                    continue;

                //Else reverse current state 
                //Send pulses to next modules
                module.HighState = !module.HighState;
                foreach (var nextModule in module.OutputModules)
                    tempQueue.Enqueue((nextModule, module.HighState));

                modules[key] = module; //Save changes in main list
                continue;
            }

            module.HighState = true;
            var previousModules = modules.Where(x => x.Value.OutputModules.Contains(key)).ToList();
            if (previousModules.All(x => x.Value.HighState))
                module.HighState = false;

            modules[key] = module; //Save changes in main list

            foreach (var nextModule in module.OutputModules)
                tempQueue.Enqueue((nextModule, module.HighState));
        }

        queue = tempQueue;
    }
}