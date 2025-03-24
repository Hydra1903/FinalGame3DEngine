using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5.0f;

    private bool canRotateCamera = true; // Ki?m tra camera có quay ???c hay không

    void Update()
    {
        // Ch? cho phép quay camera n?u canRotateCamera là true
        if (canRotateCamera)
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = -Input.GetAxis("Mouse Y") * rotationSpeed;

            // C?p nh?t góc quay c?a camera (ch? là ví d?, có th? thay ??i tùy vào yêu c?u c?a b?n)
            player.Rotate(Vector3.up * horizontal);
            Camera.main.transform.Rotate(Vector3.right * vertical);
        }
    }

    // Hàm này s? ???c g?i khi b?t ??u t??ng tác v?i NPC ?? khóa quay camera
    public void LockCameraRotation(bool lockRotation)
    {
        canRotateCamera = !lockRotation;
    }
}
