using UnityEngine;
public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("Idle");
    }
    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.horizontal != 0 || movement.vertical != 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && movement.statsManager.Character1.mana > 0)
            {
                ExitState(movement, movement.Run);
            }
            else
            {
                ExitState(movement, movement.Walk);
            }
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
            if (movement.weaponState.currentState == Weaponstate.Bow)
            {
                ExitState(movement, movement.BowShot);
            }
            else if (movement.weaponState.currentState == Weaponstate.Sword)
            {
                ExitState(movement, movement.MeleeAttack1);
            }
            else
            {
                Debug.Log("Khong co trang thai");
            }
        }
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
