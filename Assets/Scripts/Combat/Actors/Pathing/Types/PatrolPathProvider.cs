using System.Collections.Generic;
using UnityEngine;

public class PatrolPathProvider : IPathProvider
{
    private List<Vector3> waypoints;
    private int currentIndex;

    public PatrolPathProvider(
        List<Vector3> waypoints)
    {
        this.waypoints = waypoints;
        currentIndex = 0;
    }

    public Vector3 GetNextDestination(Actor actor)
    {
        if(waypoints.Count == 0) return actor.transform.position;

        Vector3 destination = waypoints[currentIndex];

        if(Vector3.Distance(actor.transform.position, destination) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Count;
        }

        return waypoints[currentIndex];
    }
}
