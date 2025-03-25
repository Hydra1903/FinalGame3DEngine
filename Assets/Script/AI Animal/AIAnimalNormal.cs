using UnityEngine;
using UnityEngine.AI;

public class AIAnimalNormal : MonoBehaviour
{
    [Header("Tham chiếu")]
    [SerializeField] private Transform playerTransform; // Tham chiếu đến người chơi

    [Header("Cài đặt phát hiện")]
    [SerializeField] private float detectionRange = 15f;  // Khoảng cách phát hiện người chơi
    [SerializeField] private LayerMask obstacleLayer;     // Layer của vật cản

    [Header("Cài đặt di chuyển")]
    [SerializeField] private float normalSpeed = 2f;      // Tốc độ đi bình thường
    [SerializeField] private float runAwaySpeed = 5f;     // Tốc độ chạy trốn
    [SerializeField] private float wanderRadius = 20f;    // Bán kính đi lang thang
    [SerializeField] private float runAwayDistance = 25f; // Khoảng cách chạy trốn

    [Header("Cài đặt thời gian")]
    [SerializeField] private float minIdleTime = 2f;      // Thời gian đứng yên tối thiểu
    [SerializeField] private float maxIdleTime = 6f;      // Thời gian đứng yên tối đa
    [SerializeField] private float minWanderTime = 3f;    // Thời gian di chuyển tối thiểu
    [SerializeField] private float maxWanderTime = 10f;   // Thời gian di chuyển tối đa

    // Các trạng thái
    private enum AnimalState { Idle, Wander, RunAway } // Đứng yên, Đi lang thang, Chạy trốn
    private AnimalState currentState = AnimalState.Idle; // Trạng thái hiện tại là đứng yên

    // Các thành phần
    private NavMeshAgent agent; // Tác tử điều hướng
    private Animator animator; // Điều khiển hoạt ảnh

    // Bộ đếm thời gian
    private float stateTimer;

    // Tên các trạng thái hoạt ảnh (có thể tùy chỉnh dựa trên thiết lập hoạt ảnh của bạn)
    private const string ANIM_IDLE = "Idle"; // Hoạt ảnh đứng yên
    private const string ANIM_WALK = "Walk"; // Hoạt ảnh đi bộ
    private const string ANIM_RUN = "Run"; // Hoạt ảnh chạy

    private void Start()
    {
        // Lấy các thành phần
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Khởi tạo NavMeshAgent
        agent.speed = normalSpeed;
        agent.stoppingDistance = 1f;
        agent.autoBraking = true;

        // Bắt đầu trạng thái đứng yên
        SetIdleState();
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi ở gần
        if (IsPlayerInRange() && HasLineOfSightToPlayer() && currentState != AnimalState.RunAway)
        {
            SetRunAwayState();
        }

        // Cập nhật trạng thái hiện tại
        UpdateCurrentState();

        // Cập nhật bộ đếm thời gian
        stateTimer -= Time.deltaTime;
    }

    private void UpdateCurrentState()
    {
        switch (currentState)
        {
            case AnimalState.Idle:
                // Khi hết thời gian đứng yên, bắt đầu đi lang thang
                if (stateTimer <= 0f)
                {
                    SetWanderState();
                }
                break;

            case AnimalState.Wander:
                // Nếu đến đích hoặc hết thời gian, chuyển sang đứng yên
                if ((agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending) || stateTimer <= 0f)
                {
                    SetIdleState();
                }
                break;

            case AnimalState.RunAway:
                // Nếu đến khoảng cách an toàn hoặc đến đích, chuyển sang đứng yên
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    if (!IsPlayerInRange())
                    {
                        SetIdleState();
                    }
                    else
                    {
                        // Vẫn còn nguy hiểm, tìm đường thoát mới
                        FindRunAwayDestination();
                    }
                }
                break;
        }
    }

    private void SetIdleState()
    {
        currentState = AnimalState.Idle;
        stateTimer = Random.Range(minIdleTime, maxIdleTime);
        agent.isStopped = true;

        // Đặt hoạt ảnh đứng yên
        if (animator != null)
        {
            animator.Play("Idle");
        }
    }

    private void SetWanderState()
    {
        currentState = AnimalState.Wander;
        stateTimer = Random.Range(minWanderTime, maxWanderTime);

        // Tìm vị trí đi lang thang ngẫu nhiên
        Vector3 wanderPosition = FindRandomNavMeshPoint(transform.position, wanderRadius);

        // Đặt tác tử di chuyển đến vị trí đó
        agent.isStopped = false;
        agent.speed = normalSpeed;
        agent.SetDestination(wanderPosition);

        // Đặt hoạt ảnh đi bộ
        if (animator != null)
        {
            animator.Play(ANIM_WALK);
        }
    }

    private void SetRunAwayState()
    {
        currentState = AnimalState.RunAway;
        agent.isStopped = false;
        agent.speed = runAwaySpeed;

        // Tìm đích để chạy trốn
        FindRunAwayDestination();

        // Đặt hoạt ảnh chạy
        if (animator != null)
        {
            animator.Play(ANIM_RUN);
        }
    }

    private void FindRunAwayDestination()
    {
        // Hướng đi xa khỏi người chơi
        Vector3 directionAwayFromPlayer = transform.position - playerTransform.position;
        directionAwayFromPlayer.Normalize();

        // Vị trí đích là vị trí hiện tại cộng với hướng xa khỏi người chơi nhân với khoảng cách chạy trốn
        Vector3 targetPosition = transform.position + directionAwayFromPlayer * runAwayDistance;

        // Tìm điểm hợp lệ trên NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, runAwayDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            // Nếu không tìm thấy điểm hợp lệ, thử vị trí gần hơn
            if (NavMesh.SamplePosition(transform.position + directionAwayFromPlayer * (runAwayDistance * 0.5f),
                                      out hit, runAwayDistance * 0.5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                // Phương án cuối cùng - tìm điểm ngẫu nhiên để chạy đến
                Vector3 randomPoint = FindRandomNavMeshPoint(transform.position, runAwayDistance);
                agent.SetDestination(randomPoint);
            }
        }
    }

    private Vector3 FindRandomNavMeshPoint(Vector3 center, float radius)
    {
        // Tìm điểm ngẫu nhiên trên NavMesh
        NavMeshHit hit;
        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        randomPoint.y = center.y; // Giữ nguyên độ cao y

        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // Nếu không tìm thấy điểm hợp lệ, trả về vị trí ban đầu
        return center;
    }

    private bool IsPlayerInRange()
    {
        if (playerTransform == null)
            return false;

        return Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    }

    private bool HasLineOfSightToPlayer()
    {
        if (playerTransform == null)
            return false;

        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // Kiểm tra nếu có vật cản giữa động vật và người chơi
        if (Physics.Raycast(transform.position, directionToPlayer.normalized,
                          out RaycastHit hit, detectionRange, obstacleLayer))
        {
            return false; // Phát hiện vật cản
        }

        return true; // Không có vật cản
    }

    // Để gỡ lỗi
    private void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi phát hiện
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Vẽ bán kính đi lang thang
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }

}
