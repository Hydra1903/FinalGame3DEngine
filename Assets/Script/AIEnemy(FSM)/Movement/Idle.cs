using UnityEngine;

public class Idle : MovementAIState
{
    public override void EnterStateAI(AiMovement movement)
    {
        movement.animator.SetBool("isRunning", false); // G?i animation �?ng y�n
        movement.agent.isStopped = true; // D?ng NavMeshAgent
        Debug.Log("AI �ang �?ng y�n.");
    }

    public override void UpdateStateAI(AiMovement movement)
    {   
        float distanceToPlayer = Vector3.Distance(movement.player.position, movement.transform.position);

        if (distanceToPlayer <= movement.fleeRange) // N?u ph�t hi?n ng�?i ch�i trong ph?m vi ch?y tr?n
        {
            movement.animator.SetBool("isRunning", false);
            ExitStateAI(movement, movement.Run);
        }
    }

    public override void ExitStateAI(AiMovement movement, MovementAIState state)
    {
        movement.agent.isStopped = false; // B?t l?i NavMeshAgent khi r?i tr?ng th�i Idle
        movement.SwitchStateAI(state);
    }
}

