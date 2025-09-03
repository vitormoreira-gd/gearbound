using System.Collections.Generic;
using UnityEngine;

public class PatrolPathProvider_PingPong : IPathProvider
{
    private List<Vector3> waypoints = new();
    private int currentIndex;

    public PatrolPathProvider_PingPong(
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
            currentIndex = (currentIndex + 1);

            if (currentIndex >= waypoints.Count)
            {
                waypoints.Reverse();
                currentIndex = 0;
            }
        }

        return waypoints[currentIndex];
    }
}