using UnityEngine;

public class Idle : MovementAIState
{

    public override void EnterStateAI(AiMoventd movement)
    {
        movement.animator.Play("GoatSheep_idle");
        Debug.Log("GoatSheep_idle");
    }
    public override void UpdateStateAI(AiMoventd movement)
    {
        if (movement.horizontal != 0 || movement.vertical != 0)
        {
           // ExitStateAI(movement, movement.Run);
        }
    }
    public override void ExitStateAI(AiMoventd movement, MovementAIState state)
    {
        movement.SwitchStateAI(state);
    }


}
