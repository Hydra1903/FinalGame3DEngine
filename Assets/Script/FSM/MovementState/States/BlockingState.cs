using UnityEngine;
public class BlockingState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("Blocking");
    }
    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetMouseButtonUp(1))
        {
            ExitState(movement, movement.Idle);
        }
        movement.Rotate2();
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
