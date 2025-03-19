using UnityEngine;
using UnityEngine.AI;
using Unity.Android.Gradle.Manifest;
using UnityEngine.EventSystems;
public class AiMoventd : MonoBehaviour 
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    public Animator animator;
    float _attackRange = 0.5f;
    float _chaseRange = 20.0f;
    Vector3 initialPosition;

    public bool isGrounded;
    public float horizontal;
    public float vertical;


    MovementAIState currentStateAI;
    public Idle Idle = new Idle();
    public Run Run = new Run();
    public Walk Walk = new Walk();
    
        
    void Start()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        SwitchStateAI(Idle);
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
            RunAway();
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
    void RunAway()
    {
        //agent.SetDestination(target.position);
    }
    void AttackTarget()
    {

    }
    public void SwitchStateAI(MovementAIState state)
    {
        currentStateAI = state;
        currentStateAI.EnterStateAI(this);
    }
}   
