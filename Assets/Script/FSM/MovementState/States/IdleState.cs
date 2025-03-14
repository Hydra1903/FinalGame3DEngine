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
        if (Input.GetMouseButtonDown(1))
        {
            ExitState(movement, movement.Blocking);
        }
        if (Input.GetMouseButtonDown(0))
        {
            ExitState(movement, movement.MeleeAttack1);
        }
        if (Input.GetMouseButtonDown(3))
        {
            ExitState(movement, movement.BowShot);
        }
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
