using UnityEngine;
public class NPCInteraction : MonoBehaviour
{
    public GameObject taskUI; // UI bảng nhiệm vụ

    // Reference đến CameraControl script
    public MovementStateManager playermove;

    private bool playerInRange = false; // Kiểm tra người chơi có ở gần NPC không

    // Khi người chơi va chạm với Collider của NPC
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Nếu đối tượng là Player
        {
            playerInRange = true; // Người chơi ở gần NPC
            //cameraControl.LockCameraRotation(true); // Khóa quay camera khi tương tác với NPC
        }
    }

    // Khi người chơi rời khỏi phạm vi va chạm
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Người chơi ra khỏi phạm vi
           // cameraControl.LockCameraRotation(false); // Khôi phục quay camera khi người chơi ra khỏi phạm vi
            HideTaskUI(); // Ẩn bảng nhiệm vụ khi người chơi rời khỏi phạm vi
        }
    }

    private void Update()
    {
        // Nếu người chơi ở gần NPC và nhấn phím E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowTaskUI(); // Hiển thị bảng nhiệm vụ
        }
    }

    void ShowTaskUI()
    {
        taskUI.SetActive(true);
        playermove.canMove = false;// Bật UI bảng nhiệm vụ
    }

    void HideTaskUI()
    {
        taskUI.SetActive(false); // Ẩn UI bảng nhiệm vụ
        playermove.canMove = true;
    }
}
