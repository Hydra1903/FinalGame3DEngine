using UnityEngine;

public class Idle : MovementAIState
{
    public override void EnterStateAI(AiMovement movement)
    {
        movement.animator.SetBool("isRunning", false); // G?i animation ð?ng yên
        movement.agent.isStopped = true; // D?ng NavMeshAgent
        Debug.Log("AI ðang ð?ng yên.");
    }

    public override void UpdateStateAI(AiMovement movement)
    {   
        float distanceToPlayer = Vector3.Distance(movement.player.position, movement.transform.position);

        if (distanceToPlayer <= movement.fleeRange) // N?u phát hi?n ngý?i chõi trong ph?m vi ch?y tr?n
        {
            movement.animator.SetBool("isRunning", false);
            ExitStateAI(movement, movement.Run);
        }
    }

    public override void ExitStateAI(AiMovement movement, MovementAIState state)
    {
        movement.agent.isStopped = false; // B?t l?i NavMeshAgent khi r?i tr?ng thái Idle
        movement.SwitchStateAI(state);
    }
}

