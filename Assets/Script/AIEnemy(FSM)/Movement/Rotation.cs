using UnityEngine;

public class Rotation : MovementAIState
{
    public override void EnterStateAI(AiMoventd movement)
    {
        movement.animator.Play("GoatSheep_turn_90_R");
        Debug.Log("GoatSheep is turning");
    }
    public override void UpdateStateAI(AiMoventd movement)
    {
        
    }
    public override void ExitStateAI(AiMoventd movement, MovementAIState state)
    {
        movement.SwitchStateAI(state);
    }
}
