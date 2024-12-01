//[Array2D][Mirror][Horizontal][Vertical]
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
    var maps = input.Split(Environment.NewLine + Environment.NewLine)
                    .Select(x => x.Split(Environment.NewLine)
                                  .Select(l => l.Select(c => c == '#')
                                                .ToArray())
                                  .ToArray()
                                  .To2DArray())
                    .ToList();

    int score = 0;
    foreach (var map in maps)
        score += GetMirrorScore(map); ;

    return score;
}

object Part2(string input)
{
    var maps = input.Split(Environment.NewLine + Environment.NewLine)
                    .Select(x => x.Split(Environment.NewLine)
                                  .Select(l => l.Select(c => c == '#')
                                                .ToArray())
                                  .ToArray()
                                  .To2DArray())
                    .ToList();

    int score = 0;
    foreach (var map in maps)
        score += GetMirrorWithSmudgeScore(map); ;

    return score;
}

int GetMirrorScore(bool[,] map)
{
    int GetMirrorLineIndex(List<bool[]> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            var l = i;
            var r = i + 1;
            var mirrorLine = true;
            while (l >= 0 && r <= list.Count - 1)
            {
                if (!list[l].SequenceEqual(list[r]))
                {
                    mirrorLine = false;
                    break;
                }
                l--;
                r++;
            }

            if (mirrorLine)
                return i;
        }

        return -1;
    }

    var rows = map.TakeRows().ToList();
    var index = GetMirrorLineIndex(rows);
    if (index >= 0)
        return (index + 1) * 100;

    var columns = map.TakeColumns().ToList();
    index = GetMirrorLineIndex(columns);
    if (index >= 0)
        return (index + 1);

    return 0;
}

int GetMirrorWithSmudgeScore(bool[,] map)
{
    int GetDifferencesCount<T>(T[] array1, T[] array2)
    {
        int differencesCount = 0;

        for (int i = 0; i < array1.Length; i++)
            if (!EqualityComparer<T>.Default.Equals(array1[i], array2[i]))
                differencesCount++;

        return differencesCount;
    }

    int GetMirrorLineIndex(List<bool[]> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            var l = i;
            var r = i + 1;
            int differencesCount = 0;
            var mirrorLine = true;
            while (l >= 0 && r <= list.Count - 1)
            {
                differencesCount += GetDifferencesCount(list[l], list[r]);
                if (differencesCount > 1)
                {
                    mirrorLine = false;
                    break;
                }
                l--;
                r++;
            }

            if (mirrorLine && differencesCount == 1)
                return i;
        }

        return -1;
    }

    var rows = map.TakeRows().ToList();
    var index = GetMirrorLineIndex(rows);
    if (index >= 0)
        return (index + 1) * 100;

    var columns = map.TakeColumns().ToList();
    index = GetMirrorLineIndex(columns);
    if (index >= 0)
        return (index + 1);

    return 0;
}