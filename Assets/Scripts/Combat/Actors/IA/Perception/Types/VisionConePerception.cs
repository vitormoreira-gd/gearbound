using System.Collections.Generic;
using UnityEngine;

public class VisionConePerception : IPerception
{
    private float radius;
    private float angle;

    public VisionConePerception(
        float radius,
        float angle)
    {
        this.radius = radius;
        this.angle = angle;
    }

    public List<Actor> GetVisibleActors(Actor actor)
    {
        List<Actor> result = new();

        Actor[] allCandidates = Object.FindObjectsByType<Actor>(FindObjectsSortMode.None);

        foreach (var candidate in allCandidates)
        {
            if (candidate == actor) continue;

            Vector3 dir = (candidate.transform.position - actor.transform.position).normalized;
            float dist = Vector3.Distance(actor.transform.position, candidate.transform.position);

            if (dist <= radius)
            {
                float dot = Vector3.Dot(actor.transform.forward, dir);
                float angleToCandidate = Mathf.Acos(dot) * Mathf.Rad2Deg;
                
                if (angleToCandidate <= angle / 2f)
                {
                    result.Add(candidate);
                }
            }
        }

        return result;
    }
}
