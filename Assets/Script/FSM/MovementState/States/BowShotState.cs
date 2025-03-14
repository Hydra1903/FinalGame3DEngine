using UnityEngine;
public class BowShotState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.Play("BowShot");
    }
    public override void UpdateState(MovementStateManager movement)
    {
        movement.Rotate2();
        if (Input.GetMouseButton(0) && movement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.50f)
        {
            if (movement.animator.GetCurrentAnimatorStateInfo(0).IsName("BowShot") && movement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.40f)
            {
                movement.animator.speed = 0;
            }
        }
        else
        { 
            movement.animator.speed = 1;
        }
        if (movement.animator.GetCurrentAnimatorStateInfo(0).IsName("BowShot") && movement.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            ExitState(movement, movement.Idle);
        }
    }
    public override void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
