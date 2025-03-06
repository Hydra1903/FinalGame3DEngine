using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    Fall
}
public class TPSController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;
    public Transform cameraTransform;

    public float groundYOffset;
    public LayerMask groundMask;
    public Vector3 spherePos;
    public float gravity = -9.81f;
    public Vector3 velocity;
    public float jumpForce = 5f;


    private CharacterController controller;
    private Vector3 moveDirection;

    private Animator animator;

    private PlayerState currentState;
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentState = PlayerState.Idle;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        HandleState(horizontal, vertical);
    }
    public void HandleState(float horizontal, float vertical)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.Play("Idle");
                Rotate();
                Gravity();
                Run(horizontal, vertical);
                Jump();
                break;
            case PlayerState.Run:
                animator.Play("RunN");
                Move(horizontal, vertical);
                Idle(horizontal, vertical);
                Rotate();
                Gravity();
                Jump();
                break;
            case PlayerState.Jump:
                animator.Play("Jump");
                Rotate();
                Gravity();
                Jump();
                break;
            case PlayerState.Fall:
                animator.Play("Fall");
                Rotate();
                Gravity();
                break;
        }
    }
    void Move(float horizontal, float vertical)
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
    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }
    void Gravity()
    {
        if(!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0 )
        {
            velocity.y = -2;
        }
        controller.Move(velocity * Time.deltaTime);
    }
    void Idle(float horizontal, float vertical)
    {
        if (horizontal == 0 && vertical == 0)
        {
            currentState = PlayerState.Idle;
        }
    }
    void Run(float horizontal, float vertical)
    {
        if (controller.isGrounded)
        {
            if (horizontal != 0 || vertical != 0)
            {
                currentState = PlayerState.Run;
            }
        }
    }
    void Jump()
    {
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentState = PlayerState.Jump;
                velocity.y = jumpForce;
            }
        }
    }
    public void EndJump()
    {
        Fall();
    }
    void Fall()
    {
        currentState = PlayerState.Fall;
        if (controller.isGrounded)
        {
            currentState = PlayerState.Idle;
        }
    }
}