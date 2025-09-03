using System.Collections.Generic;
using UnityEngine;

public class RandomPathProvider : IPathProvider
{
    private List<Vector3> waypoints;
    private int currentIndex;

    public RandomPathProvider(PathDefinition pathDef, int pathLength)
    {
        waypoints = GenerateRandomPath(pathDef, pathLength);
        currentIndex = 0;
    }

    public Vector3 GetNextDestination(Actor actor)
    {
        if (waypoints.Count == 0) return actor.transform.position;

        Vector3 destination = waypoints[currentIndex];

        if(Vector3.Distance(actor.transform.position, destination) <= 0.1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Count;
        }

        return destination;
    }

    private List<Vector3> GenerateRandomPath(PathDefinition path, int lenght)
    {
        List<Vector3> result = new List<Vector3>();

        if (path.nodes.Count == 0 || lenght <= 0) return result;

        PathNode currentNode = path.nodes[0];
        result.Add(currentNode.position);

        for (int i = 1; i < lenght; i++)
        {
            if (currentNode.nextPossibleNodes.Count == 0) break;

            string nextNodeId = currentNode.nextPossibleNodes[Random.Range(0, currentNode.nextPossibleNodes.Count)];            
            currentNode = path.nodes.Find(n => n.id == nextNodeId);

            if(currentNode == null) break;

            result.Add(currentNode.position);
        }

        return result;
    }
}
