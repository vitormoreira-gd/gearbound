using System.Collections.Generic;
using UnityEngine;

public class ClosestTargetRule : ITargetingRule
{
    public Actor SelectTarget(Actor actor, List<Actor> perceivedActors)
    {
        Actor closest = null;
        float minDist = float.MaxValue;

        foreach (var candidate in perceivedActors)
        {
            float dist = Vector3.Distance(actor.transform.position, candidate.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = candidate;
            }
        }

        return closest;
    }
}
