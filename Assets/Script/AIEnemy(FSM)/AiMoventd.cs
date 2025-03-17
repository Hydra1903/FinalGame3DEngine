using UnityEngine;
using UnityEngine.AI;

public class AiMoventd : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;

    float _attackRange = 0.5f;
    float _chaseRange = 20.0f;
    Vector3 initialPosition;
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(target.position, initialPosition);
        if(distance > _chaseRange) 
        {
            GoToInitialPostion();
        }
        else if(distance > _attackRange)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
        agent.SetDestination(target.position);
    }
    void GoToInitialPostion()
    {
        agent.SetDestination(initialPosition);
    }
    void ChaseTarget()
    {
        agent.SetDestination(target.position);
    }
    void AttackTarget()
    {

    }
}   
