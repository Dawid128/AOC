using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input, 1, 7, 20201227);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, int value, int subjectNumber, int divisor)
{
    var keys = input.Split(Environment.NewLine)
                    .Select(x => int.Parse(x))
                    .Select(x => (PublicKey: x, LoopSize: CalculateLoopSize(value, subjectNumber, divisor, x)))
                    .ToList();

    return CalculateEncryptionKey(value, keys[0].PublicKey, divisor, keys[1].LoopSize);
}

object Part2(string input)
{
    return -1;
}

int CalculateEncryptionKey(int value, int subjectNumber, int divisor, int loopSize)
{
    for (int i = 0; i < loopSize; i++)
        value = TransformValue(value, subjectNumber, divisor);

    return value;
}

int CalculateLoopSize(int value, int subjectNumber, int divisor, int publicKey)
{
    int loopSizeNumber = 1;
    while (true)
    {
        value = TransformValue(value, subjectNumber, divisor);
        if (value == publicKey)
            return loopSizeNumber;

        loopSizeNumber++;
    }
}

int TransformValue(int value, int subjectNumber, int divisor)
{
    var remainder = (value * (long)subjectNumber) % divisor;
    return (int)remainder;
}