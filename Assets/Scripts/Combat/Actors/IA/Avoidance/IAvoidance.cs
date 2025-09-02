using UnityEngine;

public interface IAvoidance
{
    Vector3 GetAvoidanceDirection(Actor actor, Vector3 currentDirection);
}