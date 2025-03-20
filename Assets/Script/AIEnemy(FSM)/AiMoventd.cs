using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Transform player;
    public Animator animator;

    [SerializeField] private float detectionRange = 15.0f; // Kho?ng c�ch AI ph�t hi?n ng�?i ch�i
    [SerializeField] public float fleeRange = 8.0f; // Kho?ng c�ch AI b?t �?u ch?y tr?n
    [SerializeField] private float fleeDistance = 10.0f; // Kho?ng c�ch AI ch?y xa kh?i ng�?i ch�i
    [SerializeField] public float safeDistance = 12.0f; // Kho?ng c�ch an to�n �? d?ng ch?y
    [SerializeField] private float rotationSpeed = 15.0f; // T?c �? quay �?u

    public float horizontal;
    public float vertical;
    private Vector3 initialPosition;
    private bool isFleeing = false;
    private MovementAIState currentStateAI;

    public Idle Idle = new Idle();
    public Run Run = new Run();
    

    void Start()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        SwitchStateAI(Idle);
        agent.updateRotation = false; // T?t auto rotation �? t? �i?u khi?n h�?ng quay
        agent.isStopped = false;
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= fleeRange)
        {
            if (!isFleeing)
            {
                isFleeing = true;
                agent.speed = 6.0f; // T�ng t?c �? khi ch?y tr?n
                SwitchStateAI(Run);
            }
            FleeFromPlayer();
        }
        else if (isFleeing && distanceToPlayer >= safeDistance)
        {
            isFleeing = false;
            agent.speed = 3.5f; // Gi?m t?c �? v? b?nh th�?ng khi an to�n
            agent.SetDestination(transform.position); // D?ng l?i khi an to�n
            SwitchStateAI(Idle);
        }
        else if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(transform.position); // D?ng l?i khi ch? ph�t hi?n nh�ng ch�a t?i ph?m vi ch?y
        }
        

        RotateTowardsMovementDirection();
    }

    public void FleeFromPlayer()
    {
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.isStopped = false; // �?m b?o AI kh�ng b? d?ng
            agent.SetDestination(hit.position);
            Debug.Log("Fleeing to: " + hit.position);
        }
    }



    void RotateTowardsMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.1f) // Ki?m tra xem c� �ang di chuy?n kh�ng
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed * 200);
        }
    }

    public void SwitchStateAI(MovementAIState state)
    {
        currentStateAI = state;
        currentStateAI.EnterStateAI(this);
    }
}
