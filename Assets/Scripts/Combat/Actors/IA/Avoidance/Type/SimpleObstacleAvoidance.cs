using UnityEngine;

public class SimpleObstacleAvoidance : IAvoidance
{
    private float avoidRadius;

    public SimpleObstacleAvoidance(float avoidRadius)
    {
        this.avoidRadius = avoidRadius;
    }

    public Vector3 GetAvoidanceDirection(Actor actor, Vector3 currentDirection)
    {
        RaycastHit hit;

        if (Physics.Raycast(actor.transform.position, currentDirection, out hit, avoidRadius))
        {
            return Vector3.Reflect(currentDirection, hit.normal).normalized;
        }

        return currentDirection;
    }
}
