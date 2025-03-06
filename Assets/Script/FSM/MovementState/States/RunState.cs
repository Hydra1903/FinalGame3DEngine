using UnityEngine;
public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("RunN");
        Debug.Log("run");
    }
    public override void UpdateState(MovementStateManager movement)
    {
        movement.Move();
        if (movement.horizontal == 0 && movement.vertical == 0)
        {
            ExitState(movement, movement.Idle);
        }
        if (movement.IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ExitState(movement, movement.Jump);
            }
        }
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
