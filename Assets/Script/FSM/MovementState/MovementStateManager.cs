using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementStateManager : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public bool isGrounded;
    public float horizontal;
    public float vertical;

    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;
    public Transform cameraTransform;
    private Vector3 moveDirection;

    public float groundYOffset;
    public LayerMask groundMask;
    public Vector3 spherePos;
    public float gravity = -9.81f;
    public Vector3 velocity;
    public float jumpForce = 5f;
    public bool isEndJump;

    MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public RunState Run = new RunState();
    public JumpState Jump = new JumpState();
    public FallState Fall = new FallState();

    void Start()
    {
        animator = GetComponent<Animator>();
        SwitchState(Idle);
    }
    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void Update()
    {
        isGrounded = IsGrounded();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Rotate();
        Gravity();
        currentState.UpdateState(this);
    }
    public void Move()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        moveDirection = (forward * vertical + right * horizontal).normalized * moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);
    }
    void Rotate()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos,0.1f, groundMask)) return true;
        return false;
    }
    void Gravity()
    {
        if (!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2;
        }
        controller.Move(velocity * Time.deltaTime);
    }
    void OnDrawGizmos()
    {
        Vector3 spherePos = transform.position + Vector3.down * groundYOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, 0.1f);
    }
    void OFFJump()
    {
        isEndJump = true;
    }
    void ONJump()
    {
        isEndJump = false;
    }

}
