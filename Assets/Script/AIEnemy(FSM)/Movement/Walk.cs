using UnityEngine;

public class Walk : MovementAIState
{
    public override void EnterStateAI(AiMoventd movement)
    {
        movement.animator.Play("Run");
        Debug.Log("running");
    }
    public override void UpdateStateAI(AiMoventd movement)
    {
       /* movement.Move();
        if (movement.horizontal == 0 && movement.vertical == 0)
        {
            ExitStateAI(movement, movement.Idle);
        }
        if (movement.IsGrounded())
        {
            
        }*/
    }
    public override void ExitStateAI(AiMoventd movement, MovementAIState state)
    {
        movement.SwitchStateAI(state);
    }
}

