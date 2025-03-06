using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Controller : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Run,
        Dead,
        TakingDamage,
        Spawn,
        Tele
    }

    private PlayerState currentState;
    private Animator animator;
    private CharacterController controller;
    public float moveSpeed = 5f;
    public float diagonalMoveSpeed;

    public Transform cameraTransform;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        currentState = PlayerState.Idle;

        diagonalMoveSpeed = moveSpeed / 1.41f;
    }


    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Debug.Log(moveX + " " + moveZ);
        HandleState(moveX,moveZ);


        Vector3 rotation = cameraTransform.eulerAngles;
        transform.eulerAngles = new Vector3(0, rotation.y, 0);
    }
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }
    public void HandleState(float moveX,float moveZ)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.Play("Idle");
                if (moveX != 0 || moveZ != 0)
                {
                    currentState = PlayerState.Run;
                }
                break;
            case PlayerState.Run:
                if (moveX != 0 && moveZ != 0)
                {
                    Vector3 move = transform.right * moveX + transform.forward * moveZ;
                    controller.Move(move * diagonalMoveSpeed * Time.deltaTime);
                }
                else
                {
                    Vector3 move = transform.right * moveX + transform.forward * moveZ;
                    controller.Move(move * moveSpeed * Time.deltaTime);
                }
                if (moveX > 0 && moveZ > 0)
                {
                    animator.Play("RunNE");
                }
                else if (moveX > 0 && moveZ < 0)
                {
                    animator.Play("RunSE");
                }
                else if (moveX < 0 && moveZ < 0)
                {
                    animator.Play("RunSW");
                }
                else if (moveX < 0 && moveZ > 0)
                {
                    animator.Play("RunNW");
                }
                else if (moveX > 0)
                {
                    animator.Play("RunE");
                }
                else if (moveZ > 0)
                { 
                    animator.Play("RunN");
                }
                else if (moveX < 0)
                {
                    animator.Play("RunW");
                }
                else if (moveZ < 0)
                {
                    animator.Play("RunS");
                }
                else
                {
                    currentState = PlayerState.Idle;
                }
                break;
        } 
    }
}
