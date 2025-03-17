using UnityEngine;

public class Idle : MovementBaseStateAI
{

    public override void EnterStateAI(MovementStateManager movement)
    {
        movement.animator.Play("GoatSheep_idle");
        Debug.Log("GoatSheep_idle");
    }
    public override void UpdateStateAI(MovementStateManager movement)
    {
        if (movement.horizontal != 0 || movement.vertical != 0)
        {
            ExitStateAI(movement, movement.Run);
        }
    }
    public override void ExitStateAI(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }


}
