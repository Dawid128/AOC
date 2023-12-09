using AdventCodeExtension;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part(input, new Options(4), GetNeighboursPart1);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part(input, new Options(5), GetNeighboursPart2);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part(string input, Options options, GetNeighbours getNeighbours)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray()
                                 .Select(c => (SeatStatus)(int)c)
                                 .ToArray())
                   .ToArray();

    while (true)
    {
        NextRound(map, out var changesCounter, options, getNeighbours);
        if (changesCounter is 0)
            return map.SelectMany(x => x)
                      .Count(x => x == SeatStatus.Occupied);
    }
}

//Improve by working only with JaggedArray
void NextRound(SeatStatus[][] map, out int changesCounter, Options options, GetNeighbours getNeighbours)
{
    changesCounter = 0;

    var mapCopy = map.Select(x => x.ToArray()).ToArray();
    var rowsNumber = map.Length;
    var columnsNumber = map[0].Length;

    for (int rowId = 0; rowId < rowsNumber; rowId++)
    {
        for (int columnId = 0; columnId < columnsNumber; columnId++)
        {
            var currentSeat = map[rowId][columnId];
            if (currentSeat is SeatStatus.Null)
                continue;

            var neighboursSeats = getNeighbours(mapCopy, rowId, columnId);

            //If current seat is empty, change to occupied only if there are no occupied seats adjacent to it
            if (currentSeat is SeatStatus.Empty)
            {
                if (!neighboursSeats.Contains(SeatStatus.Occupied))
                {
                    map[rowId][columnId] = SeatStatus.Occupied;
                    changesCounter++;
                }

                continue;
            }

            //Else current seat is occupied, change to empty only if four or more seats adjacent to it are also occupied
            if (neighboursSeats.Count(x => x == SeatStatus.Occupied) >= options.NumberOfOccupiedSeatsCritic)
            {
                map[rowId][columnId] = SeatStatus.Empty;
                changesCounter++;
            }
        }
    }
}

//Get Neighbours directly from each direction (max 8, min 0)
List<SeatStatus> GetNeighboursPart1(SeatStatus[][] map, int rowId, int columnId)
{
    var rowsNumber = map.Length;
    var columnsNumber = map[0].Length;
    var currentSeat = map[rowId][columnId];

    var starRange = (rowId > 0 ? rowId - 1 : 0, columnId > 0 ? columnId - 1 : 0);
    var endRange = (rowId + 1 < rowsNumber ? rowId + 1 : rowsNumber - 1, columnId + 1 < columnsNumber ? columnId + 1 : columnsNumber - 1);

    var result = map.TakeRange(starRange, endRange).SelectMany(x => x)
                                                   .Where(x => x != SeatStatus.Null)
                                                   .ToList();
    result.Remove(currentSeat);

    return result;
}

//Get Neighbours large distance from each direction (max 8, min 0)
List<SeatStatus> GetNeighboursPart2(SeatStatus[][] map, int rowId, int columnId)
{
    var rowsNumber = map.Length;
    var columnsNumber = map[0].Length;
    var currentSeat = map[rowId][columnId];

    //Directions Vertical and Horizontal
    var result = new List<SeatStatus>();
    foreach (var dh in new[] { -1, 0, 1 })
        foreach (var dv in new[] { -1, 0, 1 })
        {
            if (dv == 0 && dh == 0)
                continue;

            var (y, x) = (rowId, columnId);
            while (true)
            {
                y += dv;
                x += dh;

                if ((y < 0 || y >= rowsNumber) || (x < 0 || x >= columnsNumber)) 
                    break;

                var status = map[y][x];
                if (status == SeatStatus.Null)
                    continue;

                result.Add(status);
                break;
            }
        }

    return result;
}

delegate List<SeatStatus> GetNeighbours(SeatStatus[][] map, int rowId, int columnId);

public record Options(int NumberOfOccupiedSeatsCritic);

public enum SeatStatus
{
    Null = 46,
    Empty = 76,
    Occupied = 35,
}