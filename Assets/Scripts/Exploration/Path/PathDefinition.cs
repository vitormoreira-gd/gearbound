using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PathDefinition
{
    public string name;
    public List<PathNode> nodes = new ();

    public PathDefinition()
    {
        nodes = new();
    }

    public void SetNodes(List<PathNode> nodes)
    {
        this.nodes.Clear();
        this.nodes = new(nodes);
    }

    public PathNode GetStartNode() => nodes.FirstOrDefault();
    public PathNode GetFarthestNode(PathNode fromNode) => nodes
        .Where(node => node != fromNode)
        .OrderByDescending(node => (fromNode.position - node.position).sqrMagnitude)
        .FirstOrDefault();

    public List<PathNode> GetShortestPath(PathNode start, PathNode end)
    {
        Dictionary<PathNode, PathNode> previous = new();
        Queue<PathNode> queue = new();
        HashSet<PathNode> visited = new();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            PathNode current = queue.Dequeue();

            if(current == end)
            {
                return ReconstructPath(previous, start, end);
            }

            List<PathNode> possibleNodes = GetNodesByAdList(current.nextPossibleNodes);
            List<PathNode> nextNodesByDistance = possibleNodes
                .OrderBy(n => Vector3.Distance(current.position, n.position))
                .ToList();

            foreach (var neighbor in nextNodesByDistance)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    previous[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return null;
    }

    public List<PathNode> GetLongestPath(PathNode start, PathNode end)
    {
        List<PathNode> bestPath = new();
        DFS(start, new List<PathNode>(), ref bestPath);
        return bestPath;
    }

    public List<PathNode> GetRandomPath(PathNode start, int maxLength)
    {
        List<PathNode> path = new();
        HashSet<PathNode> visited = new();
        PathNode current = start;
        path.Add(current);

        List<PathNode> possibleNodes = GetNodesByAdList(current.nextPossibleNodes);

        for (int i = 0; i < maxLength - 1 ; i++ )
        {
            var neighbors = possibleNodes.Where(n => !visited.Contains(n) && !path.Contains(n)).ToList();

            if (neighbors.Count == 0)
            {
                break;
            }

            visited.Add(current);
            current = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
            path.Add(current);
        }

        return path;
    }

    private List<PathNode> ReconstructPath(Dictionary<PathNode, PathNode> previous, PathNode start, PathNode end)
    {
        List<PathNode> path = new();
        PathNode current = end;

        while (current != start)
        {
            path.Add(current);
            current = previous[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    private void DFS(PathNode current, List<PathNode> path, ref List<PathNode> bestPath)
    {
        path.Add(current);

        List<PathNode> possibleNodes = GetNodesByAdList(current.nextPossibleNodes);

        if (path.Count > bestPath.Count)
        {
            bestPath = new List<PathNode>(path);
        }

        foreach (var neighbor in possibleNodes)
        {
            if (!path.Contains(neighbor))
            {
                DFS(neighbor, path, ref bestPath);
            }
        }

        path.RemoveAt(path.Count - 1);
    }

    private List<PathNode> GetNodesByAdList(List<string> ids)
    {
        List<PathNode> nodesById = new();

        foreach (var node in nodes)
        {
            if (ids.Contains(node.id))
            {
                nodesById.Add(node);
            }
        }

        return nodesById;
    }
}