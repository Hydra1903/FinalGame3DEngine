using UnityEngine;
public class BowShotState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("BowShot");
    }
    public override void UpdateState(MovementStateManager movement)
    {
        if (Input.GetMouseButtonUp(0))
        {
            ExitState(movement, movement.Idle);
        }
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
