//[Recursion?][GetAllChildren?][FlatList?][HelpDictionary]
using AdventCodeExtension.Models;
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
    var bricks = input.Split(Environment.NewLine)
                      .Select(x => x.Split("~")
                                    .Select(ParseToPoint3D)
                                    .ToArray())
                      .Select(x => (StartPoint: new PointStruct(x[0][0], x[0][1]),
                                    EndPoint: new PointStruct(x[1][0], x[1][1]),
                                    MinZ: Math.Min(x[0][2], x[1][2]),
                                    MaxZ: Math.Max(x[0][2], x[1][2])))
                      .OrderBy(x => x.MinZ)
                      .ToList();

    //Calculate Score
    var score = 0;
    var map = GetMap(bricks);
    foreach (var (key, (parents, _)) in map)
    {
        if (parents.Count == 0)
        {
            score++;
            continue;
        }

        score += parents.Any(x => map[x].Children.Count <= 1) ? 0 : 1;
    }
    return score;
}

object Part2(string input)
{
    var bricks = input.Split(Environment.NewLine)
                      .Select(x => x.Split("~")
                                    .Select(ParseToPoint3D)
                                    .ToArray())
                      .Select(x => (StartPoint: new PointStruct(x[0][0], x[0][1]),
                                    EndPoint: new PointStruct(x[1][0], x[1][1]),
                                    MinZ: Math.Min(x[0][2], x[1][2]),
                                    MaxZ: Math.Max(x[0][2], x[1][2])))
                      .OrderBy(x => x.MinZ)
                      .ToList();

    //Calculate Score
    var map = GetMap(bricks);
    int GetScore(Brick previousBrick, List<Brick> bricks, Dictionary<Brick, (List<Brick> Parents, List<Brick> Children)> helpMap)
    {
        var score = 0;
        foreach (var brick in bricks)
        {
            if (!helpMap.TryGetValue(brick, out var value))
            {
                if (!map.TryGetValue(brick, out value))
                    throw new Exception("Not found brick");

                helpMap.Add(brick, ([.. value.Parents], [.. value.Children]));
                value = helpMap[brick];
            }

            value.Children.Remove(previousBrick);
            if (value.Children.Count > 0)
                continue;

            score++;
            score += GetScore(brick, value.Parents, helpMap);
        }

        return score;
    }

    var score = 0;
    foreach (var (key, (parents, _)) in map)
        score += GetScore(key, [.. parents], []);

    return score;
}

int[] ParseToPoint3D(string input) => input.Split(",").Select(int.Parse).ToArray();

Dictionary<Brick, (List<Brick> Parents, List<Brick> Children)> GetMap(IList<(PointStruct StartPoint, PointStruct EndPoint, int MinZ, int MaxZ)> bricks)
{
    //Drop all bricks to ground
    Dictionary<Brick, (List<Brick> Parents, List<Brick> Children)> map = [];
    foreach (var (startPoint, endPoint, minZ, maxZ) in bricks)
    {
        var firstBrick = map.Select(x => x.Key)
                            .Where(x => x.MaxZ < minZ)
                            .OrderByDescending(x => x.MaxZ)
                            .FirstOrDefault(x => HasCommonArea((x.StartPoint, x.EndPoint), (startPoint, endPoint)));

        var children = map.Select(x => x.Key)
                          .Where(x => x.MaxZ == firstBrick.MaxZ && HasCommonArea((x.StartPoint, x.EndPoint), (startPoint, endPoint)))
                          .ToList();

        if (children.Count == 0)
        {
            map.Add(new(startPoint, endPoint, minZ - minZ, maxZ - minZ), ([], []));
            continue;
        }

        var diffZ = minZ - (firstBrick.MaxZ + 1);
        Brick newBrick = new(startPoint, endPoint, minZ - diffZ, maxZ - diffZ);
        foreach (var child in children)
            map[child].Parents.Add(newBrick);

        map.Add(newBrick, ([], children));
    }

    return map;
}

bool HasCommonArea((PointStruct Start, PointStruct End) Rect1, (PointStruct Start, PointStruct End) Rect2)
{
    bool overlapX = Math.Max(Rect1.Start.X, Rect2.Start.X) <= Math.Min(Rect1.End.X, Rect2.End.X);
    bool overlapY = Math.Max(Rect1.Start.Y, Rect2.Start.Y) <= Math.Min(Rect1.End.Y, Rect2.End.Y);

    return overlapX && overlapY;
}

bool IsPointInsideRectangle(PointStruct point, PointStruct pointRect1, PointStruct pointRect2)
{
    bool isInsideX = point.X >= Math.Min(pointRect1.X, pointRect2.X) && point.X <= Math.Max(pointRect1.X, pointRect2.X);
    bool isInsideY = point.Y >= Math.Min(pointRect1.Y, pointRect2.Y) && point.Y <= Math.Max(pointRect1.Y, pointRect2.Y);

    return isInsideX && isInsideY;
}

readonly struct Brick(PointStruct startPoint, PointStruct endPoint, int minZ, int maxZ)
{
    public PointStruct StartPoint { get; } = startPoint;
    public PointStruct EndPoint { get; } = endPoint;
    public int MinZ { get; } = minZ;
    public int MaxZ { get; } = maxZ;
}