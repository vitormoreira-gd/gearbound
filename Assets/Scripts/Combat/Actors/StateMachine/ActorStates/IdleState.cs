public class IdleState : IActorState
{
    public void OnEnter(ActorBrain brain)
    {
        brain.Body.Stop();
    }

    public void OnExit(ActorBrain brain)
    {
        //
    }

    public void Update(ActorBrain brain)
    {
        if (brain.HasDestination)
        {
            brain.StateMachine.ChangeState(new MovingState(), brain);
        }
    }
}
