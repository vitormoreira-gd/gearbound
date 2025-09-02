using UnityEngine;
using System.Collections.Generic;

public interface IPerception
{
    List<Actor> GetVisibleActors(Actor actor);
}
