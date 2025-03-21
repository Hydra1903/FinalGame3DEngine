using UnityEngine;
public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("RunN");
    }
    public override void UpdateState(MovementStateManager movement)
    {
        movement.Move(1.6f);
        if (movement.horizontal == 0 && movement.vertical == 0 )
        {
            ExitState(movement, movement.Idle);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || movement.statsManager.Character1.mana == 0)
        {
            ExitState(movement, movement.Walk);
        }
        if (movement.IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ExitState(movement, movement.JumpRuning);
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
