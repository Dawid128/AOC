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
var output2 = Part2(input, 8);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var list = input.Select(x => int.Parse($"{x}"))
                    .Select((x, index) => index % 2 == 0
                                          ? new FileItem(index / 2, x) as Item
                                          : new FreeSpaceItem(x))
                    .ToList();

    var result = new List<FileItem>();
    while (list.Any(x => x is FileItem)) 
    {
        var firstItem = list.First();
        if (firstItem is FileItem firstFileItem)
        {
            list.RemoveAt(0);
            result.Add(firstFileItem);
        }

        var lastFileItem = list.LastOrDefault(x => x is FileItem) as FileItem;
        if (lastFileItem is null)
            break;

        var firstFreeSpaceItem = (FreeSpaceItem)list.First(x => x is FreeSpaceItem);

        if (lastFileItem.Size <= firstFreeSpaceItem.Size)
        {
            list.Remove(lastFileItem);
            firstFreeSpaceItem.Size -= lastFileItem.Size;
            if (firstFreeSpaceItem.Size == 0) 
                list.Remove(firstFreeSpaceItem);

            result.Add(new FileItem(lastFileItem.Id, lastFileItem.Size));
        }
        else
        {
            list.Remove(firstFreeSpaceItem);
            lastFileItem.Size -= firstFreeSpaceItem.Size;

            result.Add(new FileItem(lastFileItem.Id, firstFreeSpaceItem.Size));
        }
    }

    //Calculate Score
    long score = 0;
    var index = 0;
    foreach (var item in result)
        for (var i = 0; i < item.Size; i++)
            score += item.Id * index++;

    return score;
}

object Part2(string input, int maxLength)
{
    var list = input.Select(x => int.Parse($"{x}"))
                    .Select((x, index) => index % 2 == 0
                                          ? new FileItem(index / 2, x) as Item
                                          : new FreeSpaceItem(x))
                    .ToList();

    foreach (var nextLastFileItem in list.Where(x => x is FileItem).Select(x => (FileItem)x).Reverse().ToList())
    {
        var firstFreeSpaceItem = list.FirstOrDefault(x => x is FreeSpaceItem && x.Size >= nextLastFileItem.Size) as FreeSpaceItem;
        if (firstFreeSpaceItem is null)
            continue;

        var indexNextLastFileItem = list.IndexOf(nextLastFileItem);
        var indexFirstFreeSpaceItem = list.IndexOf(firstFreeSpaceItem);
        if (indexFirstFreeSpaceItem > indexNextLastFileItem)
            continue;

        firstFreeSpaceItem.Size -= nextLastFileItem.Size;
        list.RemoveAt(indexNextLastFileItem);
        list.Insert(indexNextLastFileItem, new FreeSpaceItem(nextLastFileItem.Size));

        var curr = (FreeSpaceItem)list.ElementAt(indexNextLastFileItem);

        var next = list.ElementAtOrDefault(indexNextLastFileItem + 1) as FreeSpaceItem;
        if (next is not null)
        {
            curr.Size += next.Size;
            list.Remove(next);
        }

        var prev = list.ElementAtOrDefault(indexNextLastFileItem - 1) as FreeSpaceItem;
        if (prev is not null)
        {
            curr.Size += prev.Size;
            list.Remove(prev);
        }

        list.Insert(indexFirstFreeSpaceItem, nextLastFileItem);
    }

    //Calculate Score
    long score = 0;
    var index = 0;
    foreach (var item in list)
        for (var i = 0; i < item.Size; i++)
        {
            if (item is FileItem fileItem)
                score += fileItem.Id * index;
            index++;
        }

    return score;
}

abstract class Item(int size)
{
    public int Size { get; set; } = size;
}

[DebuggerDisplay("{Id}x{Size}")]
class FileItem(int Id, int size) : Item(size)
{
    public int Id { get; } = Id;
}

[DebuggerDisplay("{Size}")]
class FreeSpaceItem(int size) : Item(size)
{

}