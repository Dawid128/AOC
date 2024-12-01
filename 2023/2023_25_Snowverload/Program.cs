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

object Part1(string input)
{
    var data = input.Split(Environment.NewLine)
                    .Select(x => x.Split(": "))
                    .Select(x => (Key: x[0], Value: x[1].Split(" ").ToArray()))
                    .ToList();

    //Create map (all connections)
    Dictionary<string, HashSet<string>> map = [];
    foreach (var (key, value) in data)
    {
        if (!map.ContainsKey(key))
            map[key] = [];

        map[key].AddRange(value);
        foreach (var otherKey in value)
        {
            if (!map.ContainsKey(otherKey))
                map[otherKey] = [];

            map[otherKey].Add(key);
        }
    }
    
    //Create matrix
    var keys = map.Keys.ToArray();
    var matrix = new int[map.Count][];
    for (int i = 0; i < map.Count; i++)
    {
        matrix[i] = new int[map.Count];
        for (int j = 0; j < map.Count; j++)
            matrix[i][j] = map[keys[i]].Contains(keys[j]) ? 1 : 0;
    }

    //TEST
    var currentPosition = Console.GetCursorPosition();
    for (int i = 0; i < map.Count; i++)
        for (int j = 0; j < map.Count; j++)
        {
            Console.SetCursorPosition(currentPosition.Left + i, currentPosition.Top + j);
            Console.WriteLine(matrix[i][j]);
        }

    return MaxFlowCalculator.FordFulkerson(matrix.To2DArray(), 0, 5);
}

static class MaxFlowCalculator
{
    public static int FordFulkerson(int[,] graph, int source, int sink)
    {
        int vertices = graph.GetLength(0);
        int[] parent = new int[vertices];
        int maxFlow = 0;

        while (BreadthFirstSearch(graph, source, sink, parent))
        {
            int pathFlow = int.MaxValue;

            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                pathFlow = Math.Min(pathFlow, graph[u, v]);
            }

            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                graph[u, v] -= pathFlow;
                graph[v, u] += pathFlow;
            }

            maxFlow += pathFlow;
        }

        return maxFlow;
    }

    private static bool BreadthFirstSearch(int[,] graph, int source, int sink, int[] parent)
    {
        int vertices = graph.GetLength(0);
        bool[] visited = new bool[vertices];

        Queue<int> queue = new Queue<int>();
        queue.Enqueue(source);
        visited[source] = true;
        parent[source] = -1;

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();

            for (int v = 0; v < vertices; v++)
            {
                if (!visited[v] && graph[u, v] > 0)
                {
                    queue.Enqueue(v);
                    parent[v] = u;
                    visited[v] = true;

                    if (v == sink)
                        return true;
                }
            }
        }

        return false;
    }
}