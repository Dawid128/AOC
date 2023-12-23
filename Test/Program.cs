using System;
using System.Collections.Generic;

Graph graph = new Graph();

// Add edges to the graph


graph.AddEdge(1, 2, 2);
graph.AddEdge(2, 1, 2);

graph.AddEdge(2, 3, 5);
graph.AddEdge(3, 2, 5);

graph.AddEdge(2, 4, 3);
graph.AddEdge(4, 2, 3);

graph.AddEdge(4, 3, 13);
graph.AddEdge(3, 4, 13);

graph.AddEdge(3, 5, 14);
graph.AddEdge(5, 3, 14);

int startNode = 1;
int endNode = 5;

List<int> maxPath = graph.FindMaxPath(startNode, endNode);

Console.WriteLine("Max Path: " + string.Join(" -> ", maxPath));

class Graph
{
    private Dictionary<int, List<Tuple<int, int>>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<int, List<Tuple<int, int>>>();
    }

    public void AddEdge(int fromNode, int toNode, int value)
    {
        if (!adjacencyList.ContainsKey(fromNode))
        {
            adjacencyList[fromNode] = new List<Tuple<int, int>>();
        }

        adjacencyList[fromNode].Add(new Tuple<int, int>(toNode, value));
    }

    public List<int> FindMaxPath(int startNode, int endNode)
    {
        List<int> path = new List<int>();
        HashSet<int> visitedNodes = new HashSet<int>();
        int maxPathValue = int.MinValue;

        DFS(startNode, endNode, visitedNodes, new List<int>(), ref path, ref maxPathValue);

        return path;
    }

    private void DFS(int currentNode, int endNode, HashSet<int> visitedNodes, List<int> currentPath, ref List<int> maxPath, ref int maxPathValue)
    {
        visitedNodes.Add(currentNode);
        currentPath.Add(currentNode);

        if (currentNode == endNode)
        {
            int pathValue = CalculatePathValue(currentPath);
            if (pathValue > maxPathValue)
            {
                maxPathValue = pathValue;
                maxPath = new List<int>(currentPath);
            }
        }
        else
        {
            if (adjacencyList.ContainsKey(currentNode))
            {
                foreach (var neighbor in adjacencyList[currentNode])
                {
                    if (!visitedNodes.Contains(neighbor.Item1))
                    {
                        DFS(neighbor.Item1, endNode, visitedNodes, currentPath, ref maxPath, ref maxPathValue);
                    }
                }
            }
        }

        visitedNodes.Remove(currentNode);
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    private int CalculatePathValue(List<int> path)
    {
        int pathValue = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            foreach (var neighbor in adjacencyList[path[i]])
            {
                if (neighbor.Item1 == path[i + 1])
                {
                    pathValue += neighbor.Item2;
                    break;
                }
            }
        }

        return pathValue;
    }
}