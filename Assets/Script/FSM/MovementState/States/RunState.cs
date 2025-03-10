using UnityEngine;
public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("RunN");
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
        if (Input.GetMouseButtonDown(1))
        {
            ExitState(movement, movement.Blocking);
        }
        if (Input.GetMouseButtonDown(3))
        {
            ExitState(movement, movement.MeleeAttack1);
        }
        if (Input.GetMouseButtonDown(0))
        {
            ExitState(movement, movement.BowShot);
        }
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
