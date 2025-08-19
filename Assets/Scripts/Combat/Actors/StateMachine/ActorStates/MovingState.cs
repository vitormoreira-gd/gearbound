using UnityEngine;

public class MovingState : IActorState
{
    public void OnEnter(ActorBrain brain)
    {
        // Entrar em estado de alerta
        // Definir coisas
    }

    public void OnExit(ActorBrain brain)
    {
        brain.Body.Stop();
    }

    public void Update(ActorBrain brain)
    {
        Vector3 direction = brain.Destination - brain.transform.position;

        brain.Body.Move(direction);

        if (brain.ReachDestination)
        {
            brain.StateMachine.ChangeState(new IdleState(), brain);
        }
    }
}
