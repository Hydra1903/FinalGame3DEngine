using Unity.Android.Gradle.Manifest;
using UnityEngine;


public class AnimalAI : MonoBehaviour
{
    public Animator animator;
    public float walkRadius = 10f;
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    

    public float gravity = -9.81f;
    public bool isGrounded;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float currentSpeed;

    public Vector3 velocity;
    public float groundYOffset;
    public LayerMask groundMask;
    public Vector3 spherePos;
    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        InvokeRepeating(nameof(RandomAction), 2f, 5f);
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveToTarget();
        }
        isGrounded = IsGrounded();
        Gravity();
    }

    private void RandomAction()
    {
        int action = Random.Range(0, 6); // Ch?n hành ð?ng ng?u nhiên

        ResetAnimationStates(); // Reset tr?ng thái animation

        switch (action)
        {
            case 0: // Ði b?
                animator.SetBool("isWalking", true);
                SetNewTarget(false);
                break;
            case 1: // Ch?y
                animator.SetBool("isRunning", true);
                SetNewTarget(true);
                break;
            case 2: // Ng?i xu?ng
                animator.SetBool("isSitting", true);
                isMoving = false;
                break;
            case 3: // Ð?ng d?y
                animator.SetBool("isStandingUp", true);
                isMoving = false;
                break;
            case 4: // Quay trái
                animator.SetBool("isTurningLeft", true);
                StartCoroutine(RotateSmoothly(-90f));
                break;
            case 5: // Quay ph?i
                animator.SetBool("isTurningRight", true);
                StartCoroutine(RotateSmoothly(90f));
                break;
        }
    }

    private void SetNewTarget(bool isRunning)
    {
        Vector3 randomDirection = new Vector3(Random.Range(-walkRadius, walkRadius), 0, Random.Range(-walkRadius, walkRadius));
        targetPosition = transform.position + randomDirection;
        
        currentSpeed = isRunning ? runSpeed : walkSpeed;
        isMoving = true;
    }

    private void MoveToTarget()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
       
        transform.position = newPosition;
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z)); // Nh?n v? hý?ng di chuy?n

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            isMoving = false;
            ResetAnimationStates(); // Khi ð?n nõi th? d?ng animation
        }
    }

    
    public bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, 0.1f, groundMask)) return true;
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
    }
    private System.Collections.IEnumerator RotateSmoothly(float angle)
    {
        float duration = 0.5f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, angle, 0));

        float time = 0;
        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRotation;
    }

    private void ResetAnimationStates()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalkingBack", false);
        animator.SetBool("isTrotting", false);
        animator.SetBool("isTurningLeft", false);
        animator.SetBool("isTurningRight", false);
        animator.SetBool("isSitting", false);
        animator.SetBool("isStandingUp", false);
    }
}
