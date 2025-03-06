using UnityEngine;

public class FallState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("Fall");
    }
    public override void UpdateState(MovementStateManager movement)
    {

    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {

    }
}
