public interface IActorState
{
    public void OnEnter(ActorBrain brain);
    public void Update(ActorBrain brain);
    public void OnExit(ActorBrain brain);
}
