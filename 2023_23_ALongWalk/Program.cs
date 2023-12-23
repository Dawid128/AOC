//[DuplicateList]
using AdventCodeExtension;
using AdventCodeExtension.Models;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;

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

    var height = map.Length;
    var width = map[0].Length;
    var startPoint = new PointStruct(1, 0);
    var endPoint = new PointStruct(width - 2, height - 1);

    return JustGo(map, height, width, startPoint, endPoint, []).Max();

    //var paths = GetPaths(map, height, width, startPoint, endPoint);
    //var duplicatePaths = paths.SelectMany(x => new[] { x, new(x.PointEnd, x.PointStart, x.Length) }).ToList();

    //var graph = new Graph();
    //foreach (var path in duplicatePaths)
    //    graph.AddEdge(path.PointStart, path.PointEnd, path.Length);

    //return graph.FindMaxValue(startPoint, endPoint);
}

object Part2(string input)
{
    return -1;
}

List<int> JustGo(char[][] map, int height, int width, PointStruct startPoint, PointStruct endPoint, HashSet<PointStruct> visited)
{
    var score = 0;
    var nextPoint = startPoint;
    while (nextPoint != endPoint)
    {
        visited.Add(nextPoint);

        //Get all possible next points
        List<PointStruct> candidates = [];
        foreach (var (x, y, block) in new[] { (-1, 0, '>'), (0, 1, '^'), (1, 0, '<'), (0, -1, 'v') }) 
        {
            var newPoint = nextPoint.Move(x, y);

            //Ignore the points, if is outside of map
            if ((newPoint.X < 0 || newPoint.X >= width) || (newPoint.Y < 0 || newPoint.Y >= height))
                continue;

            if (visited.Contains(newPoint))
                continue;

            var value = map[newPoint.Y][newPoint.X];
            if (value == '#') 
                continue;

            if (value == block)
                continue;

            candidates.Add(newPoint);
        }

        //If not possible is step, stop.
        if (candidates.Count == 0)
            return [];

        //If possible in only one step, go iteration. 
        if (candidates.Count == 1)
        {
            nextPoint = candidates.Single();
            score++;
            continue;
        }

        return candidates.SelectMany(x => JustGo(map, height, width, x, endPoint, [.. visited]).Select(s => s + score + 1)).ToList();
    }

    return [score];
}

HashSet<Path> GetPaths(char[][] map, int height, int width, PointStruct startPoint, PointStruct endPoint)
{
    HashSet<Path> paths = [];
    var queue = new Queue<(PointStruct? PreviousCross, PointStruct Point)>([(null, startPoint)]);
    while (queue.Count > 0)
    {
        var (previousCross, point) = queue.Dequeue();

        var (path, candidates, blindPath) = GetNextPath(map, height, width, point, endPoint, previousCross);
        if (blindPath is true || paths.Contains(path))
            continue;

        paths.Add(path);
        foreach (var candidate in candidates)
            queue.Enqueue((path.PointEnd, candidate));
    }

    return paths;
}

(Path Path, List<PointStruct> NextPoints, bool BlindPath) GetNextPath(char[][] map, int height, int width, PointStruct startPoint, PointStruct endPoint, PointStruct? previousCross)
{
    HashSet<PointStruct> visited = [];
    if (previousCross.HasValue)
        visited.Add(previousCross.Value);

    var nextPoint = startPoint;
    while (nextPoint != endPoint)
    {
        visited.Add(nextPoint);

        //Get all possible next points
        List<PointStruct> candidates = [];
        foreach (var neighbour in nextPoint.GetAdjacentPoints4(1, width, height))
        {
            if (visited.Contains(neighbour))
                continue;

            if (map[neighbour.Y][neighbour.X] == '#')
                continue;

            candidates.Add(neighbour);
        }

        //If not possible is step, stop.
        if (candidates.Count == 0)
            return (default, [], true);

        //If possible in only one step, go iteration. 
        if (candidates.Count == 1)
        {
            nextPoint = candidates.Single();
            continue;
        }

        return (new(visited.First(), nextPoint, visited.Count), candidates, false);
    }

    return (new(visited.First(), endPoint, visited.Count + 1), [], false);
}

readonly struct Path(PointStruct pointStart, PointStruct pointEnd, int length)
{
    private readonly int _hashCode = CreateHashCode(pointStart, pointEnd, length);

    public PointStruct PointStart { get; } = pointStart;
    public PointStruct PointEnd { get; } = pointEnd;
    public int Length { get; } = length;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not Path path)
            return false;

        if (PointStart != path.PointStart && PointStart != path.PointEnd)
            return false;

        if (PointEnd != path.PointStart && PointEnd != path.PointEnd)
            return false;

        if (Length != path.Length)
            return false;

        return true;
    }

    public override int GetHashCode() => _hashCode;

    private static int CreateHashCode(PointStruct pointStart, PointStruct pointEnd, int length)
    {
        var min = new PointStruct(Math.Min(pointStart.X, pointEnd.X), Math.Min(pointStart.Y, pointEnd.Y));
        var max = new PointStruct(Math.Max(pointStart.X, pointEnd.X), Math.Max(pointStart.Y, pointEnd.Y));
        return HashCode.Combine(min, max, length);
    }
}

class Graph
{
    private readonly Dictionary<PointStruct, List<(PointStruct Point, int Value)>> _map = [];

    public void AddEdge(PointStruct fromNode, PointStruct toNode, int value)
    {
        if (!_map.ContainsKey(fromNode))
            _map[fromNode] = [];

        _map[fromNode].Add((toNode, value));
    }

    public List<PointStruct> FindMaxPath(PointStruct startNode, PointStruct endNode)
    {
        List<PointStruct> path = [];
        int maxPathValue = int.MinValue;

        DFS(startNode, endNode, [], [], ref path, ref maxPathValue);

        return path;
    }

    public int FindMaxValue(PointStruct startNode, PointStruct endNode)
    {
        List<PointStruct> path = [];
        int maxPathValue = int.MinValue;

        DFS(startNode, endNode, [], [], ref path, ref maxPathValue);

        return maxPathValue - path.Count + 2; //because each crosspoint was count 2 times (exept start and end)
    }

    private void DFS(PointStruct currentNode, PointStruct endNode, HashSet<PointStruct> visitedNodes, List<PointStruct> currentPath, ref List<PointStruct> maxPath, ref int maxPathValue)
    {
        visitedNodes.Add(currentNode);
        currentPath.Add(currentNode);

        if (currentNode == endNode)
        {
            int pathValue = CalculatePathValue(currentPath);
            if (pathValue > maxPathValue)
            {
                maxPath = [..currentPath];
                maxPathValue = pathValue;
            }
        }
        else
        {
            if (_map.TryGetValue(currentNode, out var value))
                foreach (var (point, _) in value)
                    if (!visitedNodes.Contains(point))
                        DFS(point, endNode, visitedNodes, currentPath, ref maxPath, ref maxPathValue);
        }

        visitedNodes.Remove(currentNode);
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    private int CalculatePathValue(List<PointStruct> path)
    {
        var pathValue = 0;
        for (int i = 0; i < path.Count - 1; i++)
            foreach (var (point, value) in _map[path[i]])
                if (point == path[i + 1])
                {
                    pathValue += value;
                    break;
                }

        return pathValue;
    }
}