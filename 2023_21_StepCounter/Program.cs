using AdventCodeExtension.Models;
using System.Diagnostics;

//616583678588165 IS TO HIGH (Under -227 978 935)
//616583483179597
//616583450609230 IS TO LOW (Under -6 094 283 660)
//616577356325570 IS TO LOW

//Input map has size 131x131
//Required step to go outside: 65
//Required steps: 26 501 365
//Required maps in horizontal line (without center): (26 501 365 - 65)/131= 202 300 -> (101150 x2) Odd and Even

//Kind's of maps
//Middle: Start Map, where S is put on the middle
//Standard: Left, Top, Right, Bottom.
//          Each can start as odd or even
//          Start as odd means start from edge
//          Start as even means start from edge +1 inside



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
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray())
                   .ToArray();

    var startPoint = map.SelectMany((row, Y) => row.Select((Value, X) => (Point: new PointStruct(X, Y), Value))).Single(x => x.Value == 'S').Point;
    return GetScoreInfiniteAdjacent(map, startPoint, 64, true);
}

object Part2(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray())
                   .ToArray();

    return GetScoreInfinite(map, 26501365);
}

long GetScoreInfinite(char[][] map, int repetitionsNumber)
{
    if (map.Length != map[0].Length || map.Length % 2 == 0)
        throw new Exception("Input map is not square or size is not odd. ");

    var size = map.Length;
    var halfSize = (size - 1) / 2;
    long result = 0;
    var longRadius = ((repetitionsNumber - halfSize) / size);
    var centerPoint = new PointStruct(halfSize, halfSize);

    //Work Inside
    var (evenCountInside, oddCountInside) = CalculateNumbersInside(longRadius);
    result += GetScoreInfiniteAdjacent(map, centerPoint, int.MaxValue, true) * evenCountInside;
    result += GetScoreInfiniteAdjacent(map, centerPoint, int.MaxValue, false) * oddCountInside;

    //Work Outside: Edges
    var (evenCountEdges, oddCountEdges) = CalculateNumbersOneEdge(longRadius);
    List<PointStruct> cornerPoints = [new(size - 1, 0), new(size - 1, size - 1), new(0, size - 1), new(0, 0)];
    foreach (var cornerPoint in cornerPoints)
    {
        result += GetScoreInfiniteAdjacent(map, cornerPoint, (size - 1) - halfSize - 1, true) * evenCountEdges;
        result += GetScoreInfiniteAdjacent(map, cornerPoint, (size - 1) + halfSize, false) * oddCountEdges;
    }

    //Work Outside: Cross
    List<PointStruct> crossPoints = [new(halfSize, 0), new(size - 1, halfSize), new(halfSize, size - 1), new(0, halfSize)];
    foreach (var crossPoint in crossPoints)
        result += GetScoreInfiniteAdjacent(map, crossPoint, size - 1, true);

    return result;
}

//Return count of even/odd numbers inside rhombus
//Assumption: Center value is type even
(long EvenNumbers, long OddNumbers) CalculateNumbersInside(int longRadius)
{
    long CalculateEvenNumbers(long n) => ((n * 2) - 1) * 4;
    long CalculateOddNumbers(long n) => (n * 2) * 4;
    long CalculateTotalNumbers(long a1, long an, long n) => (a1 + an) / 2 * n;

    var evenCount = longRadius / 2;
    var oddCount = longRadius - evenCount - 1;

    var evenNumbers = CalculateTotalNumbers(CalculateEvenNumbers(1), CalculateEvenNumbers(evenCount), evenCount);
    var oddNumbers = CalculateTotalNumbers(CalculateOddNumbers(1), CalculateOddNumbers(oddCount), oddCount);

    return (evenNumbers, oddNumbers + 1);   
}

//Return count of even/odd numbers at the one edge rhombus
//Edge contains values partly inside and outside
//The points on the start/end edges are not returning by this method
(int EvenNumbers, int OddNumbers) CalculateNumbersOneEdge(int longRadius)
{
    return (longRadius, (longRadius - 1));
}

long GetScoreInfiniteAdjacent(char[][] map, PointStruct startPoint, int repetitionsNumber, bool isEven)
{
    var height = map.Length;
    var width = map[0].Length;

    HashSet<(PointStruct Point, bool Outside)> visitedPoints = [(startPoint, false)];
    List<int> visitedCountInStep = [1];

    var queue = new Queue<(PointStruct Point, bool Outside)>([(startPoint, false)]);
    for (var i = 1; i <= repetitionsNumber; i++)
    {
        Queue<(PointStruct Point, bool Outside)> newQueue = [];
        while (queue.Count > 0)
        {
            var (currentPoint, currentOutside) = queue.Dequeue();

            foreach (var (nextPoint, nextOutside) in currentPoint.GetInfiniteAdjacentPoints4(width, height))
            {
                var nextOutsideFix = nextOutside || (!nextOutside && currentOutside);

                if (visitedPoints.Contains((nextPoint, nextOutsideFix))) 
                    continue;

                if (map[nextPoint.Y][nextPoint.X] == '#')
                    continue;

                newQueue.Enqueue((nextPoint, nextOutsideFix));
                visitedPoints.Add((nextPoint, nextOutsideFix));
            }
        }

        var count = newQueue.Count(x => !x.Outside);
        if (count is 0)
            break;

        visitedCountInStep.Add(count);
        queue = newQueue;
    }

    var result = visitedCountInStep.Where((_, index) => index % 2 == (isEven ? 0 : 1)).Sum();
    return result;
}