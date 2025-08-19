public class ActorStateMachine
{
    private IActorState currentState;

    public void ChangeState(IActorState newState, ActorBrain brain)
    {
        currentState?.OnExit(brain);
        currentState = newState;
        currentState?.OnEnter(brain);
    }

    public void Update(ActorBrain brain)
    {
        currentState?.Update(brain);
    }
}
