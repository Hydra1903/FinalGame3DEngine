using UnityEngine;
public class MeleeAttack1State : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("MeleeAttack1");
    }
    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.animator.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack1") && movement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            if (Input.GetMouseButton(0))
            {
                ExitState(movement, movement.MeleeAttack1);
            }
            else
            {
                ExitState(movement, movement.Idle);
            }
        }
        movement.Rotate2();
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
