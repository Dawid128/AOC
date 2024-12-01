using AdventCodeExtension;
using AdventCodeExtension.Models;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Security.AccessControl;

//var a = new Node(null, new PointStruct(1, 2), 1);
//var b = new Node(null, new PointStruct(2, 1), 1);
//var c = new PointStruct(1, 2);
//var d = new PointStruct(2, 1);
//var e = new Node(null, new PointStruct(1, 2), 1);

//Console.WriteLine("HashCode c: " + c.GetHashCode());
//Console.WriteLine("HashCode d: " + d.GetHashCode());

//if (a == b)
//    Console.WriteLine("a is equal b (What is false)");
//else
//    Console.WriteLine("a is not equal b (What is true)");

//if (c == d)
//    Console.WriteLine("c is equal d (What is false)");
//else
//    Console.WriteLine("c is not equal d (What is true)");

//if (a == e)
//    Console.WriteLine("a is equal e (What is true)");
//else
//    Console.WriteLine("a is not equal e (What is false)");

//Console.ReadLine();

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
                   .Select(x => x.Select(c => int.Parse(c.ToString()))
                                 .ToArray())
                   .ToArray()
                   .To2DArray();

    var start = new Node(new(0, 0), default, default);
    var end = new Node(new(map.GetLength(1) - 1, map.GetLength(0) - 1), default, default);
    start.H = AStar.CalculateHeuristic(start, end);
    return AStar.FindMinCost(map, start, end);
}

object Part2(string input)
{
    return -1;
}

public class Node(PointStruct point, PointStruct previousMove, int moveOneDirectionCount)
{
    public PointStruct Point { get; set; } = point;
    public PointStruct PreviousMove { get; set; } = previousMove;
    public int MoveOneDirectionCount { get; set; } = moveOneDirectionCount;

    public int G { get; set; } // Cost from start node to current node
    public int H { get; set; } // Heuristic estimate from current node to goal node
    public int F { get => G + H; } // Total cost: F = G + H

    public static bool operator ==(Node? nodeL, Node? nodeR) => nodeL?.Equals(nodeR) ?? nodeL is null && nodeR is null;
    public static bool operator !=(Node? nodeL, Node? nodeR) => !nodeL?.Equals(nodeR) ?? !(nodeL is null && nodeR is null);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not Node node)
            return false;

        return GetHashCode() == node.GetHashCode();
    }

    public override int GetHashCode() => HashCode.Combine(Point.X, Point.Y, PreviousMove.X, PreviousMove.Y, MoveOneDirectionCount);
}

public static class AStar
{
    public static int FindMinCost(int[,] map, Node start, Node end)
    {
        List<Node> openSet = [start];
        HashSet<Node> closedSet = [];

        while (openSet.Count > 0)
        {
            var current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
                if (openSet[i].F < current.F || (openSet[i].F == current.F && openSet[i].H < current.H))
                    current = openSet[i];

            openSet.Remove(current);
            closedSet.Add(current);

            if (current.Point == end.Point && current.MoveOneDirectionCount >= 4) 
                return current.G;

            var neighbors = GetNeighbors(map, current);
            foreach (var neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                int tentativeG = current.G + map[neighbor.Point.Y, neighbor.Point.X];
                if (!openSet.Contains(neighbor) || tentativeG < neighbor.G)
                {
                    neighbor.G = tentativeG;
                    neighbor.H = CalculateHeuristic(neighbor, end);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return -1;
    }

    public static int CalculateHeuristic(Node a, Node b)
    {
        return Math.Abs(a.Point.X - b.Point.X) + Math.Abs(a.Point.Y - b.Point.Y);
    }

    private static List<Node> GetNeighbors(int[,] map, Node node)
    {
        var rowsNumber = map.GetLength(0);
        var columnsNumber = map.GetLength(1);

        List<Node> result = []; 
        foreach (var (x, y) in new[] { (0, -1), (1, 0), (0, 1), (-1, 0) })
        {
            //Ignore the points from which it comes
            if (node.PreviousMove == new PointStruct(x * -1, y * -1)) 
                continue;

            var continueMoveOneDirection = false;
            if (node.PreviousMove == new PointStruct(x, y)) 
                continueMoveOneDirection = true;

            if (node.MoveOneDirectionCount > 0)
            {
                //Ignore the points if continue straight is not possible
                if (node.MoveOneDirectionCount < 4 && !continueMoveOneDirection)
                    continue;

                //Ignore the points if continue straight is not possible
                if (node.MoveOneDirectionCount >= 10 && continueMoveOneDirection)
                    continue;
            }

            var newPoint = new PointStruct(node.Point.X + x, node.Point.Y + y);

            //Ignore the points, if is outside of map
            if ((newPoint.X < 0 || newPoint.X >= columnsNumber) || (newPoint.Y < 0 || newPoint.Y >= rowsNumber))
                continue;

            result.Add(new(newPoint, new PointStruct(x, y), continueMoveOneDirection ? node.MoveOneDirectionCount + 1 : 1));
        }

        return result;
    }
}