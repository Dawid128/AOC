//[Array2D][JaggedArray][Cache][RepeatInIteration][BigIteration][Mod][SearchRepeatIndex][DuplicateArrayNotDeep]
using AdventCodeExtension;
using System.Diagnostics;

Dictionary<string, (char[,] Current, char[,] Next)> _cache = [];

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
output = Part2(input, 1000000000);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

Console.ReadKey();

object Part1(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray())
                   .ToArray()
                   .To2DArray();

    TiltToNorth(map);
    return map.ToJaggedArray()
              .Reverse()
              .Select((x, index) => (index + 1) * x.Count(c => c == 'O'))
              .Sum();
}

object Part2(string input, int repatCount)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray())
                   .ToArray()
                   .To2DArray();

    //Execute as long as will finish or will find the first repeat
    for (int i = 1; i <= repatCount; i++)
    {
        var uniqueId = map.GenerateUniqueId();
        if (_cache.ContainsKey(uniqueId))
            break;

        var newMap = NextCycle(map.Duplicate());
        _cache.Add(uniqueId, (map, newMap));
        map = newMap;
    }

    //Calculate which repeat is your result
    var startId = _cache.Last().Value.Next.GenerateUniqueId(); //Get ID of item which is first item of loop
    var startIndex = _cache.IndexOfKey(startId); //Get Index of item which is first item of loop
    var count = _cache.Count - startIndex; //Get Count of items in loop 

    var searchIndex = startIndex + ((repatCount - startIndex) % count); //Calculate Index of the search item
    map = _cache.ElementAt(searchIndex).Value.Current;

    //Return score 
    var key = map.GenerateUniqueId();
    return map.ToJaggedArray()
              .Reverse()
              .Select((x, index) => (index + 1) * x.Count(c => c == 'O'))
              .Sum();
}

//Each iteration inside cycle is creating new array.
char[,] NextCycle(char[,] map)
{
    var newMap = map;
    for (int i = 0; i < 4; i++)
    {
        TiltToNorth(newMap);
        newMap = newMap.RotateRight();
    }

    return newMap;
}

void TiltToNorth(char[,] map)
{
    var rowsLength = map.GetLength(0);
    var columnsLength = map.GetLength(1);

    for (int columnId = 0; columnId < columnsLength; columnId++)
    {
        var column = map.TakeColumn(columnId);

        var roundedIndexes = new Queue<int>(column.Select((Value, Index) => (Value, Index)).Where(x => x.Value == 'O').Select(x => x.Index));
        var shapedIndexes = new Queue<int>(column.Select((Value, Index) => (Value, Index)).Where(x => x.Value == '#').Select(x => x.Index));

        var position = 0;
        while (roundedIndexes.Count > 0)
        {
            //If the rounded rock is at the correct position, delete from list and go to next position
            if (roundedIndexes.Peek() == position)
            {
                roundedIndexes.Dequeue();
                position++;
                continue;
            }

            //If exist shaped rock at the lower level, than the next rounded rock, go to position above the shaped rock (only if position is less of course)
            if (shapedIndexes.Count > 0 && roundedIndexes.Peek() > shapedIndexes.Peek() && position <= shapedIndexes.Peek())
            {
                position = shapedIndexes.Dequeue() + 1;
                continue;
            }

            //If the rounded rock is above the position, move the rock to this position
            if (roundedIndexes.Peek() > position)
            {
                map[position, columnId] = 'O';
                map[roundedIndexes.Dequeue(), columnId] = '.';
            }

            position++;
        }
    }
}