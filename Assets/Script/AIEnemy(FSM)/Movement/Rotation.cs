using UnityEngine;

public class Rotation : MovementAIState
{
    public override void EnterStateAI(AiMovement movement)
    {
        movement.animator.Play("GoatSheep_turn_90_R");
        Debug.Log("GoatSheep is turning");
    }
    public override void UpdateStateAI(AiMovement movement)
    {
        
    }
    public override void ExitStateAI(AiMovement movement, MovementAIState state)
    {
        movement.SwitchStateAI(state);
    }   
}
