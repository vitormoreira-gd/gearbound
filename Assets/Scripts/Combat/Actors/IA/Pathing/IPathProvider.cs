using UnityEngine;

public interface IPathProvider
{
    Vector3 GetNextDestination(Actor actor);
}
