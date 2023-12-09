//[BitArray][DuplicateList][ForEach]
using AdventCodeExtension;
using System.Collections;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output1 = Part1(input);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output1}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
var output2 = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var data = Parse(input);

    var memory = new Dictionary<int, long>();
    foreach (var (mask, values) in data)
        foreach (var (memoryAdress, value) in values)
        {
            memory.Remove(memoryAdress);
            memory.Add(memoryAdress, ApplyMaskToValue(mask, value));
        }

    return memory.Values.Sum();
}

object Part2(string input)
{
    var data = Parse(input);

    var memory = new Dictionary<long, long>();
    foreach (var (mask, values) in data)
        foreach (var (adress, value) in values)
            foreach (var nextAdress in ApplyMaskToAdress(mask, adress))
            {
                memory.Remove(nextAdress);
                memory.Add(nextAdress, value);
            }

    return memory.Values.Sum();
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

long ApplyMaskToValue(string mask, long value)
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

IEnumerable<long> ApplyMaskToAdress(string mask, long adress)
{
    var bitArray = adress.ToBitArray(36);

    var result = new List<BitArray>() { new(36) };
    for (var i = 0; i < 36; i++)
    {
        if (mask[i] == 'X')
        {
            result = result.Select(x => new BitArray[2] { x, new(x) })
                           .ForEach(x => { x[0].Set(i, true); x[1].Set(i, false); })
                           .SelectMany(x => x)
                           .ToList();
            continue;
        }

        if (mask[i] == '1')
            result.ForEach(x => x[i] = true);
        else
            result.ForEach(x => x[i] = bitArray[i]);
    }

    return result.Select(x => x.ToLong());
}