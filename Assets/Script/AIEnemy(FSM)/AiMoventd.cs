using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Transform player;
    public Animator animator;

    [SerializeField] private float detectionRange = 15.0f; // Kho?ng cách AI phát hi?n ngý?i chõi
    [SerializeField] public float fleeRange = 8.0f; // Kho?ng cách AI b?t ð?u ch?y tr?n
    [SerializeField] private float fleeDistance = 10.0f; // Kho?ng cách AI ch?y xa kh?i ngý?i chõi
    [SerializeField] public float safeDistance = 12.0f; // Kho?ng cách an toàn ð? d?ng ch?y
    [SerializeField] private float rotationSpeed = 15.0f; // T?c ð? quay ð?u

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
        agent.updateRotation = false; // T?t auto rotation ð? t? ði?u khi?n hý?ng quay
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
                agent.speed = 6.0f; // Tãng t?c ð? khi ch?y tr?n
                SwitchStateAI(Run);
            }
            FleeFromPlayer();
        }
        else if (isFleeing && distanceToPlayer >= safeDistance)
        {
            isFleeing = false;
            agent.speed = 3.5f; // Gi?m t?c ð? v? b?nh thý?ng khi an toàn
            agent.SetDestination(transform.position); // D?ng l?i khi an toàn
            SwitchStateAI(Idle);
        }
        else if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(transform.position); // D?ng l?i khi ch? phát hi?n nhýng chýa t?i ph?m vi ch?y
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
            agent.isStopped = false; // Ð?m b?o AI không b? d?ng
            agent.SetDestination(hit.position);
            Debug.Log("Fleeing to: " + hit.position);
        }
    }



    void RotateTowardsMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.1f) // Ki?m tra xem có ðang di chuy?n không
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
