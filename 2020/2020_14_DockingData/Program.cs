using AdventCodeExtension;
using System.Collections;
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
    var data = Parse(input);

    var memory = new Dictionary<int, long>();
    foreach (var (mask, values) in data)
        foreach (var (memoryAdress, value) in values)
        {
            memory.Remove(memoryAdress);
            memory.Add(memoryAdress, ApplyMask(mask, value));
        }

    return memory.Values.Sum();
}

object Part2(string input)
{
    return -1;
}

List<(string Mask, List<(int MemoryAdress, int Value)> Values)> Parse(string input)
=> input.Replace(" ", string.Empty)
        .Replace("mem[", string.Empty)
        .Replace("]", string.Empty)
        .Split("mask=")
        .Skip(1)
        .Select(x => x.Split(Environment.NewLine)
                      .Where(x => x != string.Empty)
                      .ToArray())
        .Select(x => (Mask: new string(x[0].Reverse().ToArray()), Values: x.Skip(1)
                                                                           .Select(s => s.Split("="))
                                                                           .Select(s => (MemoryAdress: int.Parse(s[0]), Value: int.Parse(s[1])))
                                                                           .ToList()))
        .ToList();

long ApplyMask(string mask, long value)
{
    var bitArray = value.ToBitArray(36);

    var result = new BitArray(36);
    for (var i = 0; i < 36; i++)
        if (mask[i] == 'X') 
            result[i] = bitArray[i];
        else
        {
            if (mask[i] == '1')
                result[i] = true;
            else
                result[i] = false;
        }

    return result.ToLong();
}