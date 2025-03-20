using UnityEngine;
using UnityEngine.AI;

public class Run : MovementAIState
{
    public override void EnterStateAI(AiMovement movement)
    {
        movement.animator.SetBool("isRunning", true); // G?i animation ch?y
        movement.agent.speed = 6.0f; // T�ng t?c �? ch?y
        Debug.Log("AI �ang ch?y tr?n!");
    }

    public override void UpdateStateAI(AiMovement movement)
    {
        float distanceToPlayer = Vector3.Distance(movement.player.position, movement.transform.position);

        if (distanceToPlayer >= movement.safeDistance) // Khi �?t kho?ng c�ch an to�n, d?ng l?i
        {
            movement.animator.SetBool("isRunning", false);
            ExitStateAI(movement, movement.Idle);
        }
        else
        {
            movement.FleeFromPlayer(); // Ti?p t?c ch?y tr?n
        }
    }

    public override void ExitStateAI(AiMovement movement, MovementAIState state)
    {
        movement.agent.speed = 3.5f; // Gi?m t?c �? v? b?nh th�?ng
        movement.SwitchStateAI(state);
    }
}