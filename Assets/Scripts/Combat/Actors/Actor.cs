using UnityEngine;

[RequireComponent(typeof(ActorInfo), typeof(ActorBrain), typeof(ActorBody))]
public class Actor : MonoBehaviour
{
    public ActorInfo Info => _info;
    public ActorBody Body => _body;
    public ActorBrain Brain => _brain;

    private ActorInfo _info;
    private ActorBody _body;
    private ActorBrain _brain;

    public string pathName;
    [SerializeField] private Transform objectToFollow;

    private void Awake()
    {
        _info = GetComponent<ActorInfo>();
        _body = GetComponent<ActorBody>();
        _brain = GetComponent<ActorBrain>();

        Creature creature = CreatureFactory.CreateCreature(
            name: "Player",
            creatureType: CreatureType.Player,
            vigorLevel: 10,
            powerLevel: 10,
            agilityLevel: 10);

        Info.Init(creature);
        //Placeholder_GoToOtherPlace();
    }

    public void Init(Creature creature)
    {
        Info.Init(creature);

        Brain.InitFromCreature(creature);
    }

    [ContextMenu("Move")]
    private void Placeholder_GoToOtherPlace()
    {
        Brain.SetDestination(objectToFollow.position);
    }
}
