using System.Collections.Generic;
using UnityEngine;

public class ActorBrain : MonoBehaviour
{
    public ActorStateMachine StateMachine { get; private set; }

    private Actor _actorInstance;
    private Vector3? _destination;
    private Actor _currentTarget;

    public Actor ActorInstance => _actorInstance;
    public ActorBody Body => ActorInstance.Body;
    public ActorInfo Info => ActorInstance.Info;
    public Actor CurrentTarget => _currentTarget;
    public Vector3 Destination => _destination ?? transform.position;

    public bool HasDestination => _destination.HasValue;
    public bool ReachDestination => Vector3.Distance(transform.position, Destination) <= 0.1f;

    private IPerception perception;
    private ITargetingRule targeting;
    private IPathProvider pathProvider;
    private IAvoidance avoidance;

    private List<Vector3> waypoints = new();

    private void Awake()
    {
        _actorInstance = GetComponent<Actor>();
        StateMachine = new();
    }

    private void Start()
    {
        StateMachine?.ChangeState(new IdleState(), this);

        var path = PathLoader.GetPathByName(ActorInstance.pathName);

        if (path == null) return;

        waypoints = new List<Vector3>();

        foreach (var node in path.nodes)
        {
            waypoints.Add(node.position);
        }

        //pathProvider = new RandomPathProvider(path, 5);
        pathProvider = new PatrolPathProvider_PingPong(waypoints);
    }

    private void Update()
    {
        StateMachine?.Update(this);
        Tick();
    }

    public void InitFromCreature(Creature creautre)
    {
        //switch ()
        //{

        //}
    }

    public void SetDestination(Vector3 destination)
    {
        _destination = destination;
    }

    public void Tick()
    {
        // Path
        Vector3 destination = pathProvider?.GetNextDestination(ActorInstance) ?? ActorInstance.transform.position;

        // Perception
        List<Actor> visibleActors = perception?.GetVisibleActors(ActorInstance) ?? new List<Actor>();

        // Targeting
        _currentTarget = targeting?.SelectTarget(ActorInstance, visibleActors);

        // Avoidance
        Vector3 desiredDirection = (destination - ActorInstance.transform.position).normalized;
        desiredDirection = avoidance?.GetAvoidanceDirection(ActorInstance, desiredDirection) ?? desiredDirection;

        // Body delegate
        ActorInstance.Body.Move(desiredDirection);
    }

    private void OnDrawGizmos()
    {
        if (waypoints.Count <= 0) return;

        Gizmos.color = Color.blue;
        foreach (var waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint, 0.2f);
        }
    }
}
