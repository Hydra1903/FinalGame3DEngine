using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour
{
    public Animator animator;

    public AIBaseState currentState;
    public AIIdle AIIdle = new AIIdle();
    public AIWalk AIWalk = new AIWalk();
    public AIRun AIRun = new AIRun();
    public AILie AILie = new AILie();
    public AISitUp AISitUp = new AISitUp();
    public AIDie AIDie = new AIDie();


    public Transform player;  
    public float runDistance = 5f; 
    private NavMeshAgent agent;

    public void SwitchState(AIBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    void Start()
    {
        SwitchState(AIIdle);
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        currentState.UpdateState(this);


        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < runDistance) // Nếu người chơi đến gần
        {
            Vector3 runDirection = (transform.position - player.position).normalized; // Hướng ngược lại
            Vector3 newTarget = transform.position + runDirection * runDistance;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newTarget, out hit, 2.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position); // Đặt vị trí hợp lệ
            }
            else
            {
                Debug.Log("Không tìm thấy vị trí hợp lệ để chạy!");
            }
        }
    }
}
