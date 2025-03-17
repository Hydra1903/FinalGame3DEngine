using UnityEngine;

public class Rotation : MovementBaseStateAI
{
    public override void EnterStateAI(MovementStateManager movement)
    {
        movement.animator.Play("GoatSheep_turn_90_R");
        Debug.Log("GoatSheep is turning");
    }
    public override void UpdateStateAI(MovementStateManager movement)
    {
        
    }
    public override void ExitStateAI(MovementStateManager movement, MovementBaseState state)
    {
        movement.SwitchState(state);
    }
}
