////[DuplicateList]
//using AdventCodeExtension;
//using AdventCodeExtension.Models;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics.CodeAnalysis;
//using System.IO;
//using System.Net;

//var input = File.ReadAllText($"Resources\\Input.txt");
//var stopwatch = Stopwatch.StartNew();

////Part 1
//stopwatch.Start();
//var output = Part1(input);
//stopwatch.Stop();
//Console.WriteLine($"Output Part1: {output}");
//Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

////Part 2
//stopwatch.Start();
//output = Part2(input);
//stopwatch.Stop();
//Console.WriteLine($"Output Part2: {output}");
//Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//object Part1(string input)
//{
//    var map = input.Split(Environment.NewLine)
//                   .Select(x => x.ToCharArray())
//                   .ToArray();

//    var height = map.Length;
//    var width = map[0].Length;
//    var startPoint = new PointStruct(1, 0);
//    var endPoint = new PointStruct(width - 2, height - 1);

//    var paths = GetPaths(map, height, width, startPoint, endPoint);

//    //Test 2 
//    var aa = paths.SelectMany(x => new[] { x, new(x.PointEnd, x.PointStart, x.Length) }).ToList();

//    //Test
//    Dictionary<PointStruct, List<(PointStruct Point, int Length)>> test = [];
//    var crossPoints = paths.SelectMany(x => new[] { x.PointStart, x.PointEnd }).ToHashSet();
//    var queue = new Queue<PointStruct>([startPoint]);
//    while (queue.Count > 0)
//    {
//        var point = queue.Dequeue();
//        if (!test.ContainsKey(point))
//            test.Add(point, []);

//        var candidates = paths.Where(x => x.PointStart == point || x.PointEnd == point).ToList();
//        foreach (var candidate in candidates)
//        {
//            var nextPoint = point == candidate.PointStart ? candidate.PointEnd : candidate.PointStart;
//            queue.Enqueue(nextPoint);
//            paths.Remove(candidate);
//            test[point].Add((nextPoint, candidate.Length));
//        }


//    }











//    var mapPaths = GetMapPaths2(paths);


//    //Calculate Score
//    int GetScore(Path path, HashSet<Path> visited)
//    {
//        var score = path.Length;
//        var paths = mapPaths[path];

//        var candidates = paths.Where(x => !visited.Contains(path)).ToList();
//        if (candidates.Count > 0) 
//            score += candidates.Select(x => GetScore(x, [.. visited.Concat([path])])).Max();
        
//        score--;
//        return score;
//    }

//    var startPath = mapPaths.Single(x => x.Key.PointStart == startPoint).Key;
//    return GetScore(startPath, []);
//}

//object Part2(string input)
//{
//    return -1;
//}

////void AAA(HashSet<Path> paths, Path path, PointStruct crossPoint)
////{
////    var nextPaths = paths.Where(x => !x.Equals(path))
////                         .Where(x => x.PointEnd == crossPoint || x.PointStart == crossPoint)
////                         .Where(x => x.PointEnd == path.PointEnd || x.PointStart == path.PointEnd || x.PointEnd == path.PointStart || x.PointStart == path.PointStart)
////                         .ToList();

////    var candidates = 
////}

////Dictionary<Path, List<Path>> GetMapPaths(HashSet<Path> paths, Path startPath)
////{
////    //var pathsCopy = paths.ToHashSet();
////    //Dictionary<Path, List<Path>> map = [];

////    //var queue = new Queue<Path>([startPath]);
////    //while (queue.Count > 0)
////    //{
////    //    var nextPath = queue.Dequeue();

////    //}






////    Dictionary<Path, List<Path>> map = [];
////    foreach (var path in paths)
////    {
////        var related = paths.Where(x => !x.Equals(path))
////                           .Where(x=> )
////                           .Where(x => x.PointEnd == path.PointEnd || x.PointStart == path.PointEnd || x.PointEnd == path.PointStart || x.PointStart == path.PointStart);
////        map.Add(path, [.. related]);
////    }

////    return map;
////}

//Dictionary<Path, List<Path>> GetMapPaths2(HashSet<Path> paths)
//{
//    Dictionary<Path, List<Path>> map = [];
//    foreach (var path in paths)
//    {
//        var related = paths.Where(x => !x.Equals(path))
//                           .Where(x => x.PointEnd == path.PointEnd || x.PointStart == path.PointEnd || x.PointEnd == path.PointStart || x.PointStart == path.PointStart);
//        map.Add(path, [.. related]);
//    }

//    return map;
//}

//HashSet<Path> GetPaths(char[][] map, int height, int width, PointStruct startPoint, PointStruct endPoint)
//{
//    HashSet<Path> paths = [];
//    var queue = new Queue<(PointStruct? PreviousCross, PointStruct Point)>([(null, startPoint)]);
//    while (queue.Count > 0)
//    {
//        var (previousCross, point) = queue.Dequeue();

//        var (path, candidates, blindPath) = GetNextPath(map, height, width, point, endPoint, previousCross);
//        if (blindPath is true || paths.Contains(path))
//            continue;

//        paths.Add(path);
//        foreach (var candidate in candidates)
//            queue.Enqueue((path.PointEnd, candidate));
//    }

//    return paths;
//}

//(Path Path, List<PointStruct> NextPoints, bool BlindPath) GetNextPath(char[][] map, int height, int width, PointStruct startPoint, PointStruct endPoint, PointStruct? previousCross)
//{
//    HashSet<PointStruct> visited = [];
//    if (previousCross.HasValue)
//        visited.Add(previousCross.Value);

//    var nextPoint = startPoint;
//    while (nextPoint != endPoint)
//    {
//        visited.Add(nextPoint);

//        //Get all possible next points
//        List<PointStruct> candidates = [];
//        foreach (var neighbour in nextPoint.GetAdjacentPoints4(1, width, height))
//        {
//            if (visited.Contains(neighbour))
//                continue;

//            if (map[neighbour.Y][neighbour.X] == '#')
//                continue;

//            candidates.Add(neighbour);
//        }

//        //If not possible is step, stop.
//        if (candidates.Count == 0)
//            return (default, [], true);

//        //If possible in only one step, go iteration. 
//        if (candidates.Count == 1)
//        {
//            nextPoint = candidates.Single();
//            continue;
//        }

//        return (new(visited.First(), nextPoint, visited.Count), candidates, false);
//    }

//    return (new(visited.First(), endPoint, visited.Count + 1), [], false);
//}

//readonly struct Path(PointStruct pointStart, PointStruct pointEnd, int length)
//{
//    private readonly int _hashCode = CreateHashCode(pointStart, pointEnd, length);

//    public PointStruct PointStart { get; } = pointStart;
//    public PointStruct PointEnd { get; } = pointEnd;
//    public int Length { get; } = length;

//    public override bool Equals([NotNullWhen(true)] object? obj)
//    {
//        if (obj is not Path path)
//            return false;

//        if (PointStart != path.PointStart && PointStart != path.PointEnd)
//            return false;

//        if (PointEnd != path.PointStart && PointEnd != path.PointEnd)
//            return false;

//        if (Length != path.Length)
//            return false;

//        return true;
//    }

//    public override int GetHashCode() => _hashCode;

//    private static int CreateHashCode(PointStruct pointStart, PointStruct pointEnd, int length)
//    {
//        var min = new PointStruct(Math.Min(pointStart.X, pointEnd.X), Math.Min(pointStart.Y, pointEnd.Y));
//        var max = new PointStruct(Math.Max(pointStart.X, pointEnd.X), Math.Max(pointStart.Y, pointEnd.Y));
//        return HashCode.Combine(min, max, length);
//    }
//}
