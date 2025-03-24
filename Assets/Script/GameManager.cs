using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using Unity.Cinemachine;
public enum GameState
{
    Start,
    Lobby
}
public class GameManager : MonoBehaviour
{
    public GameState currentState;
    private UIManager uiManager;

    public Camera camera1;
    public Camera camera2;
    public CinemachineInputAxisController controllerCamera;
    public Animator animator;
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        currentState = GameState.Start;
    }
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    void Update()
    {
        HandleState();
    }
    void HandleState()
    {
        switch (currentState)
        {
            case GameState.Start:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("CameraTransition") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    SwitchCamera();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    ChangeState(GameState.Lobby);
                    controllerCamera.enabled = true;
                }
                break;
            case GameState.Lobby:
                break;
        }
    }
    public void SwitchCamera()
    {
        camera1.targetDisplay = 1; 
        camera2.targetDisplay = 0;
    }
}
