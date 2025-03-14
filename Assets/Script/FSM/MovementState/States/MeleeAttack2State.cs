using UnityEngine;
public class MeleeAttack2State : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("MeleeAttack2");
    }
    public override void UpdateState(MovementStateManager movement)
    {

    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {

    }
}
