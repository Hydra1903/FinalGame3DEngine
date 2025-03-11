using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    public Animator animator;
    public float walkRadius = 10f;
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    public LayerMask groundLayer; // X�c �?nh layer c?a m?t �?t

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float currentSpeed;

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
    }

    private void RandomAction()
    {
        int action = Random.Range(0, 6); // Ch?n h�nh �?ng ng?u nhi�n

        ResetAnimationStates(); // Reset tr?ng th�i animation

        switch (action)
        {
            case 0: // �i b?
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
            case 3: // �?ng d?y
                animator.SetBool("isStandingUp", true);
                isMoving = false;
                break;
            case 4: // Quay tr�i
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
        targetPosition.y = GetGroundY(targetPosition); // C?p nh?t Y theo m?t �?t
        currentSpeed = isRunning ? runSpeed : walkSpeed;
        isMoving = true;
    }

    private void MoveToTarget()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
        newPosition.y = GetGroundY(newPosition); // C?p nh?t Y theo m?t �?t
        transform.position = newPosition;
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z)); // Nh?n v? h�?ng di chuy?n

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            isMoving = false;
            ResetAnimationStates(); // Khi �?n n�i th? d?ng animation
        }
    }

    private float GetGroundY(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 2f, Vector3.down, out hit, 10f, groundLayer))
        {
            return hit.point.y;
        }
        return position.y; // Gi? nguy�n n?u kh�ng c� m?t �?t
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
