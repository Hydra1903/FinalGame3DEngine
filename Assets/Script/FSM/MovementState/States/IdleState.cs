using UnityEngine;
public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("Idle");
        Debug.Log("idle");
    }
    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.horizontal != 0 || movement.vertical != 0)
        {
            ExitState(movement, movement.Run);
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
