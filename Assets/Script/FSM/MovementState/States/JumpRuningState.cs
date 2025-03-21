using UnityEngine;

public class JumpRuningState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("JumpRuning");
        movement.velocity.y = movement.jumpForce;
    }
    public override void UpdateState(MovementStateManager movement)
    {
        movement.Move(1.3f);
        if (movement.IsGrounded() && movement.isEndJump)
        {
            ExitState(movement, movement.Idle);
        }
        else if (movement.IsGrounded() && movement.isEndJump)
        {
            ExitState(movement, movement.Idle);
        }
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
