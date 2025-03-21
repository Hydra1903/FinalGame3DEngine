using UnityEngine;
using UnityEngine.AI;

public class AIAnimalNormal : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform animalTransform;
    [SerializeField] private Transform playerTransform;

    [Header("Detection Settings")]
    [SerializeField] private float earlyAwarenessRange = 20f;  // Phạm vi nhận biết từ xa
    [SerializeField] private float criticalAwarenessRange = 10f;  // Phạm vi báo động ngay lập tức
    [SerializeField] private LayerMask obstacleLayer;  // Layer chứa các vật cản

    [Header("Movement Settings")]
    [SerializeField] private float idleSpeed = 1.5f;
    [SerializeField] private float alertSpeed = 3f;
    [SerializeField] private float panicSpeed = 7f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Behavior Settings")]
    [SerializeField] private float minWanderTime = 5f;
    [SerializeField] private float maxWanderTime = 15f;
    [SerializeField] private float minIdleTime = 3f;
    [SerializeField] private float maxIdleTime = 8f;
    [SerializeField] private float fleeDistance = 30f;
    [SerializeField] private float safetyRadius = 50f;

    private NavMeshAgent agent;
    private Animator animator; // Thêm nếu bạn có animator

    private enum AnimalState { Idle, Wander, Alert, Flee, Safe }
    private AnimalState currentState = AnimalState.Idle;

    private Vector3 wanderTarget;
    private float stateTimer;
    private bool isPlayerDetected = false;
    private float lastCheckedTime = 0f;
    private float detectionCheckInterval = 0.1f; // Kiểm tra phát hiện mỗi 0.1 giây

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>(); // Tìm Animator trong con của GameObject này

        // Cấu hình NavMeshAgent
        agent.speed = idleSpeed;
        agent.acceleration = acceleration;
        agent.angularSpeed = 480f;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;

        // Bắt đầu hành vi tự nhiên
        SetNewIdleState();
    }

    private void Update()
    {
        // Kiểm tra phát hiện người chơi với tần suất cố định
        if (Time.time - lastCheckedTime > detectionCheckInterval)
        {
            lastCheckedTime = Time.time;
            DetectPlayer();
        }

        // Cập nhật hành vi dựa trên trạng thái hiện tại
        UpdateState();

        // Cập nhật các animation (nếu có)
        UpdateAnimations();
    }

    private void UpdateState()
    {
        // Giảm thời gian trạng thái
        stateTimer -= Time.deltaTime;

        // Cập nhật trạng thái hiện tại
        switch (currentState)
        {
            case AnimalState.Idle:
                if (stateTimer <= 0f)
                {
                    SetNewWanderState();
                }
                break;

            case AnimalState.Wander:
                // Chạy đến điểm đích khi đi lang thang
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    if (stateTimer <= 0f)
                    {
                        SetNewIdleState();
                    }
                }
                break;

            case AnimalState.Alert:
                // Trạng thái cảnh giác - theo dõi người chơi
                LookAtPlayer();

                // Nếu quá gần, chạy trốn
                if (IsPlayerInCriticalRange())
                {
                    SetFleeState();
                }
                // Nếu người chơi đi xa, quay lại trạng thái bình thường
                else if (!IsPlayerInAwarenessRange())
                {
                    SetNewWanderState();
                }
                break;

            case AnimalState.Flee:
                // Kiểm tra xem đã chạy xong chưa
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    // Nếu đã an toàn
                    if (!IsPlayerInAwarenessRange())
                    {
                        SetSafeState();
                    }
                    else
                    {
                        // Tiếp tục chạy nếu vẫn gặp nguy hiểm
                        CalculateAndSetFleeDestination();
                    }
                }
                // Nếu đang chạy nhưng người chơi lại gần hơn
                else if (IsPlayerInCriticalRange())
                {
                    // Tính toán lại hướng chạy trốn
                    CalculateAndSetFleeDestination();
                }
                break;

            case AnimalState.Safe:
                // Sau khi an toàn, nghỉ ngơi một lúc rồi quay lại hoạt động bình thường
                if (stateTimer <= 0f)
                {
                    if (IsPlayerInAwarenessRange())
                    {
                        SetAlertState();
                    }
                    else
                    {
                        SetNewIdleState();
                    }
                }
                break;
        }
    }

    private void DetectPlayer()
    {
        // Tính khoảng cách đến người chơi
        float distanceToPlayer = Vector3.Distance(animalTransform.position, playerTransform.position);

        // Phát hiện trong phạm vi báo động ngay lập tức
        if (distanceToPlayer <= criticalAwarenessRange)
        {
            if (HasLineOfSightToPlayer())
            {
                isPlayerDetected = true;
                if (currentState != AnimalState.Flee)
                {
                    SetFleeState();
                    return;
                }
            }
        }

        // Phát hiện trong phạm vi nhận biết từ xa - không cần kiểm tra góc nhìn
        if (distanceToPlayer <= earlyAwarenessRange)
        {
            // Chỉ kiểm tra tầm nhìn thẳng, không cần kiểm tra góc nhìn
            if (HasLineOfSightToPlayer())
            {
                isPlayerDetected = true;
                if (currentState != AnimalState.Alert && currentState != AnimalState.Flee)
                {
                    SetAlertState();
                    return;
                }
            }
        }

        // Nếu người chơi đã ra khỏi tầm nhận biết
        if (distanceToPlayer > earlyAwarenessRange && isPlayerDetected)
        {
            isPlayerDetected = false;
            if (currentState == AnimalState.Alert)
            {
                SetNewWanderState();
            }
        }
    }

    private bool HasLineOfSightToPlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - animalTransform.position;

        // Kiểm tra xem có vật cản nào giữa động vật và người chơi không
        if (Physics.Raycast(animalTransform.position, directionToPlayer.normalized,
                           out RaycastHit hit, directionToPlayer.magnitude, obstacleLayer))
        {
            return false;
        }
        return true;
    }

    private bool IsPlayerInAwarenessRange()
    {
        return Vector3.Distance(animalTransform.position, playerTransform.position) <= earlyAwarenessRange;
    }

    private bool IsPlayerInCriticalRange()
    {
        return Vector3.Distance(animalTransform.position, playerTransform.position) <= criticalAwarenessRange;
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (playerTransform.position - animalTransform.position).normalized;
        direction.y = 0; // Giữ nguyên trục y

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            animalTransform.rotation = Quaternion.Slerp(animalTransform.rotation, targetRotation,
                                                      Time.deltaTime * rotationSpeed);
        }
    }

    private void SetNewIdleState()
    {
        currentState = AnimalState.Idle;
        stateTimer = Random.Range(minIdleTime, maxIdleTime);
        agent.isStopped = true;
    }

    private void SetNewWanderState()
    {
        currentState = AnimalState.Wander;
        stateTimer = Random.Range(minWanderTime, maxWanderTime);

        FindWanderDestination();

        agent.isStopped = false;
        agent.speed = idleSpeed;
        agent.SetDestination(wanderTarget);
    }

    private void SetAlertState()
    {
        currentState = AnimalState.Alert;
        agent.isStopped = true;
        agent.speed = alertSpeed;
    }

    private void SetFleeState()
    {
        currentState = AnimalState.Flee;
        agent.isStopped = false;
        agent.speed = panicSpeed;

        CalculateAndSetFleeDestination();
    }

    private void SetSafeState()
    {
        currentState = AnimalState.Safe;
        stateTimer = Random.Range(minIdleTime, maxIdleTime);
        agent.isStopped = true;
        agent.speed = idleSpeed;
    }

    private void FindWanderDestination()
    {
        NavMeshHit hit;
        Vector3 randomDirection = Random.insideUnitSphere * safetyRadius;
        randomDirection += animalTransform.position;
        randomDirection.y = animalTransform.position.y;

        if (NavMesh.SamplePosition(randomDirection, out hit, safetyRadius, NavMesh.AllAreas))
        {
            wanderTarget = hit.position;
        }
        else
        {
            // Nếu không tìm được điểm hợp lệ, thử lại gần hơn
            randomDirection = Random.insideUnitSphere * (safetyRadius * 0.5f);
            randomDirection += animalTransform.position;
            randomDirection.y = animalTransform.position.y;

            if (NavMesh.SamplePosition(randomDirection, out hit, safetyRadius * 0.5f, NavMesh.AllAreas))
            {
                wanderTarget = hit.position;
            }
        }
    }

    private void CalculateAndSetFleeDestination()
    {
        // Hướng ngược lại với người chơi
        Vector3 directionFromPlayer = (animalTransform.position - playerTransform.position).normalized;

        // Tìm các điểm chạy trốn và đánh giá chúng
        Vector3 bestFleePosition = FindBestFleePosition(directionFromPlayer);

        // Đặt đích mới
        agent.SetDestination(bestFleePosition);
    }

    private Vector3 FindBestFleePosition(Vector3 directionFromPlayer)
    {
        // Tạo một số vị trí tiềm năng để chạy trốn
        Vector3[] potentialPositions = new Vector3[5];
        NavMeshHit hit;
        Vector3 bestPosition = animalTransform.position + directionFromPlayer * fleeDistance;
        float bestScore = 0f;

        // Tạo 5 hướng tiềm năng để chạy trốn
        float baseAngle = Mathf.Atan2(directionFromPlayer.z, directionFromPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < potentialPositions.Length; i++)
        {
            // Tính toán hướng với một góc ngẫu nhiên xung quanh hướng chính
            float angle = baseAngle + Random.Range(-60f, 60f);
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

            Vector3 targetPos = animalTransform.position + dir * fleeDistance;

            // Lấy mẫu vị trí trên NavMesh
            if (NavMesh.SamplePosition(targetPos, out hit, fleeDistance, NavMesh.AllAreas))
            {
                potentialPositions[i] = hit.position;

                // Đánh giá vị trí này (khoảng cách từ người chơi)
                float distanceFromPlayer = Vector3.Distance(hit.position, playerTransform.position);

                // Kiểm tra xem có đường đi đến vị trí này không
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(animalTransform.position, hit.position, NavMesh.AllAreas, path))
                {
                    float pathLength = CalculatePathLength(path);

                    // Đánh giá dựa trên khoảng cách và độ dài đường đi
                    float score = distanceFromPlayer - (pathLength * 0.5f);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestPosition = hit.position;
                    }
                }
            }
        }

        // Nếu không tìm được vị trí nào tốt, sử dụng hướng mặc định
        if (bestScore == 0f)
        {
            if (NavMesh.SamplePosition(animalTransform.position + directionFromPlayer * fleeDistance,
                                     out hit, fleeDistance, NavMesh.AllAreas))
            {
                bestPosition = hit.position;
            }
        }

        return bestPosition;
    }

    private float CalculatePathLength(NavMeshPath path)
    {
        float length = 0f;
        if (path.corners.Length < 2)
            return length;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return length;
    }

    private void UpdateAnimations()
    {
        // Cập nhật các animation nếu có Animator
        if (animator != null)
        {
            // Thiết lập các tham số Animator dựa trên trạng thái
            switch (currentState)
            {
                case AnimalState.Idle:
                    animator.SetFloat("Speed", 0f);
                    animator.SetBool("IsAlert", false);
                    animator.SetBool("IsFleeing", false);
                    break;

                case AnimalState.Wander:
                    animator.SetFloat("Speed", 0.5f);
                    animator.SetBool("IsAlert", false);
                    animator.SetBool("IsFleeing", false);
                    break;

                case AnimalState.Alert:
                    animator.SetFloat("Speed", 0f);
                    animator.SetBool("IsAlert", true);
                    animator.SetBool("IsFleeing", false);
                    break;

                case AnimalState.Flee:
                    animator.SetFloat("Speed", 1f);
                    animator.SetBool("IsAlert", true);
                    animator.SetBool("IsFleeing", true);
                    break;

                case AnimalState.Safe:
                    animator.SetFloat("Speed", 0f);
                    animator.SetBool("IsAlert", true);
                    animator.SetBool("IsFleeing", false);
                    break;
            }
        }
    }

    // Vẽ gizmo để dễ debug
    private void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi nhận biết
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, earlyAwarenessRange);

        // Vẽ phạm vi báo động
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, criticalAwarenessRange);

        // Vẽ phạm vi an toàn
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safetyRadius);
    }
}
