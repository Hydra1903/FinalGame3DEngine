using UnityEngine;
using UnityEngine.AI;

public class AIAnimal : MonoBehaviour
{
    [Header("Tham chiếu")]
    [SerializeField] private Transform playerTransform; // Tham chiếu đến người chơi

    [Header("Cài đặt phát hiện")]
    [SerializeField] private float detectionRange = 15f;  // Khoảng cách phát hiện người chơi
    [SerializeField] private LayerMask obstacleLayer;     // Layer của vật cản

    [Header("Cài đặt di chuyển")]
    [SerializeField] private float normalSpeed = 2f;      // Tốc độ đi bình thường
    [SerializeField] private float chaseSpeed = 10f;       // Tốc độ đuổi theo người chơi
    [SerializeField] private float wanderRadius = 20f;    // Bán kính đi lang thang

    [Header("Cài đặt tấn công")]
    [SerializeField] private float attackRange = 2f;      // Khoảng cách tấn công
    [SerializeField] private float attackCooldown = 2f;   // Thời gian chờ giữa các đòn tấn công
    [SerializeField] private int attackDamage = 10;       // Sát thương mỗi đòn tấn công
    [SerializeField] private float stunDuration = 1f;     // Thời gian đứng im sau khi tấn công

    [Header("Cài đặt thời gian")]
    [SerializeField] private float minIdleTime = 2f;      // Thời gian đứng yên tối thiểu
    [SerializeField] private float maxIdleTime = 6f;      // Thời gian đứng yên tối đa
    [SerializeField] private float minWanderTime = 3f;    // Thời gian di chuyển tối thiểu
    [SerializeField] private float maxWanderTime = 10f;   // Thời gian di chuyển tối đa

    // Các trạng thái
    private enum AnimalState { Idle, Wander, Chase, Attack } // Đứng yên, Đi lang thang, Đuổi theo, Tấn công
    private AnimalState currentState = AnimalState.Idle; // Trạng thái hiện tại là đứng yên

    // Các thành phần
    private NavMeshAgent agent; // Tác tử điều hướng
    private Animator animator; // Điều khiển hoạt ảnh

    // Bộ đếm thời gian
    private float stateTimer;
    private float attackTimer;

    // Tên các trạng thái hoạt ảnh
    private const string ANIM_IDLE = "Idle";   // Hoạt ảnh đứng yên
    private const string ANIM_WALK = "Walk";   // Hoạt ảnh đi bộ
    private const string ANIM_RUN = "Run";     // Hoạt ảnh chạy
    private const string ANIM_ATTACK = "Attack"; // Hoạt ảnh tấn công

    private void Start()
    {
        // Lấy các thành phần
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Khởi tạo NavMeshAgent
        agent.speed = normalSpeed;
        agent.stoppingDistance = attackRange; // Dừng lại khi đến khoảng cách tấn công
        agent.autoBraking = true;

        // Khởi tạo bộ đếm thời gian tấn công
        attackTimer = 0f;

        // Bắt đầu trạng thái đứng yên
        SetIdleState();
    }

    private void Update()
    {
        // Giảm bộ đếm thời gian tấn công
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        // Kiểm tra phát hiện người chơi
        bool playerDetected = IsPlayerInRange() && HasLineOfSightToPlayer();

        // Nếu thấy người chơi và chưa đuổi theo/tấn công thì bắt đầu đuổi theo
        if (playerDetected && currentState != AnimalState.Chase && currentState != AnimalState.Attack)
        {
            SetChaseState();
        }
        // Nếu không thấy người chơi và đang đuổi theo thì chuyển sang trạng thái đứng yên
        else if (!playerDetected && currentState == AnimalState.Chase)
        {
            SetIdleState();
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

            case AnimalState.Chase:
                // Liên tục cập nhật đích đến là vị trí người chơi
                if (playerTransform != null)
                {
                    agent.SetDestination(playerTransform.position);
                    
                    // Kiểm tra nếu đến đủ gần để tấn công
                    float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                    if (distanceToPlayer <= attackRange && attackTimer <= 0)
                    {
                        SetAttackState();
                    }
                }
                break;

            case AnimalState.Attack:
                // Khi hết thời gian tấn công, chuyển về đuổi theo nếu người chơi vẫn ở gần
                if (stateTimer <= 0f)
                {
                    if (IsPlayerInRange() && HasLineOfSightToPlayer())
                    {
                        SetChaseState();
                    }
                    else
                    {
                        SetIdleState();
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
            animator.Play(ANIM_IDLE);
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

    private void SetChaseState()
    {
        currentState = AnimalState.Chase;
        agent.isStopped = false;
        agent.speed = chaseSpeed;

        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }

        // Đặt hoạt ảnh chạy
        if (animator != null)
        {
            animator.Play(ANIM_RUN);
        }
    }

    private void SetAttackState()
    {
        currentState = AnimalState.Attack;
        stateTimer = stunDuration; // Thời gian cần để hoàn thành hoạt ảnh tấn công
        attackTimer = attackCooldown; // Đặt lại bộ đếm thời gian tấn công
        agent.isStopped = true; // Dừng lại khi tấn công

        // Đặt hoạt ảnh tấn công
        if (animator != null)
        {
            animator.Play(ANIM_ATTACK);
        }

        // Thực hiện tấn công
        PerformAttack();
    }

    private void PerformAttack()
    {
        if (playerTransform != null)
        {
            // Đảm bảo quay về phía người chơi
            Vector3 lookDirection = playerTransform.position - transform.position;
            lookDirection.y = 0; // Giữ nguyên độ cao y
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            // Kiểm tra khoảng cách để đảm bảo vẫn ở trong tầm tấn công
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange)
            {
                // Tìm và gây sát thương cho người chơi
                //PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
                //if (playerHealth != null)
               // {
                 //   playerHealth.TakeDamage(attackDamage);
               // }
                
                // Có thể thêm hiệu ứng âm thanh hoặc hình ảnh khi tấn công
                // PlayAttackSound();
                // SpawnAttackEffect();
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
        
        // Vẽ phạm vi tấn công
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}