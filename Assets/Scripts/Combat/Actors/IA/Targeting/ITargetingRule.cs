using System.Collections.Generic;

public interface ITargetingRule
{
    Actor SelectTarget(Actor actor, List<Actor> perceivedActors);
}
