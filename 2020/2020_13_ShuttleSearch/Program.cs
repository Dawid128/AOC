using System.Diagnostics;
using System.Numerics;

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
    var (estimation, busIds) = GetData(input);
    var map = busIds.ToDictionary(x => x, x => x - estimation % x); //Create map Bus ID and time to wait for bus. 
    var min = map.MinBy(x => x.Value); //Find the first BUS that leaves

    return min.Value * min.Key;
}

object Part2(string input)
{
    //0 mod NUMBER = T + INDEX
    //-INDEX mod NUMBER = T
    //-INDEX + NUMBER * RoundUP(INDEX/NUMBER): Calculate to change INDEX to positive
    BigInteger CalculateReminder(int number, int index) => (-index + number * (int)Math.Ceiling((double)index / (double)number)) % number;

    var numbers = input.Split(Environment.NewLine)[1]
                       .Split(",")
                       .Select((numberStr, index) => (numberStr, index))
                       .Where(x => x.numberStr != "x")
                       .Select(x => (number: int.Parse(x.numberStr), x.index))
                       .Select(x => (Modul: new BigInteger(x.number), Reminder: CalculateReminder(x.number, x.index)))
                       .ToList();

    return CRT(numbers);
}

(int Estimation, HashSet<int> BusIds) GetData(string input)
{
    var split = input.Split(Environment.NewLine);
    return (int.Parse(split[0]), split[1].Split(",").Where(x => x != "x").Select(x => int.Parse(x)).ToHashSet());
}

//Chinese Remainder Theorem
static BigInteger CRT(IList<(BigInteger Modulo, BigInteger Reminder)> numbers)
{
    var product = numbers.Select(x => x.Modulo).Aggregate((x, y) => x * y);

    BigInteger result = 0;
    foreach (var (modulo, reminder) in numbers)
    {
        //Calculate part bn
        var partialProduct = product / modulo;
        var inverseProduct = BigInteger.ModPow(partialProduct, modulo - 2, modulo);

        //Calculate result
        result += reminder * partialProduct * inverseProduct;
    }

    return result % product;
}