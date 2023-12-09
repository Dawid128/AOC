using _2020_05_BinaryBoarding.Helpers;
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

//Drawing
//Draw(input);

object Part1(string input)
{
    return input.Split(Environment.NewLine)
                .Select(TakePlaceID)
                .ToArray()
                .Max();
}

object Part2(string input)
{
    var seats = input.Split(Environment.NewLine)
                     .Select(TakePlaceID)
                     .ToHashSet();

    var seatsAll = Enumerable.Range(0, 127 * 8 + 7).ToHashSet(); //All possible seats
    var seatsMissing = seatsAll.Except(seats).ToHashSet();

    foreach (var seatMissing in seatsMissing)
        if (seats.Contains(seatMissing + 1) && seats.Contains(seatMissing - 1))
            return seatMissing;

    return -1;
}

void Draw(string input)
{
    var seats = input.Split(Environment.NewLine)
                     .Select(TakePlaceID)
                     .ToHashSet();

    DrawingHelper.DrawPlane((128, 8), seats);
}

int TakePlaceID(string input)
{
    int TakePartID((int From, int To) range, IList<char> input)
    {
        foreach (var character in input)
        {
            var half = (range.To - range.From + 1) / 2;
            if (character == 'B' || character == 'R')
                range.From += half;
            else
                range.To -= half;
        }

        return range.From;
    }

    var rowID = TakePartID(new(0, 127), input.Take(7).ToList());
    var columnID = TakePartID(new(0, 7), input.Skip(7).ToList());

    return rowID * 8 + columnID;
}

