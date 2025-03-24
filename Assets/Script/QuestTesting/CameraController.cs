using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5.0f;

    private bool canRotateCamera = true; // Ki?m tra camera c� quay ???c hay kh�ng

    void Update()
    {
        // Ch? cho ph�p quay camera n?u canRotateCamera l� true
        if (canRotateCamera)
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = -Input.GetAxis("Mouse Y") * rotationSpeed;

            // C?p nh?t g�c quay c?a camera (ch? l� v� d?, c� th? thay ??i t�y v�o y�u c?u c?a b?n)
            player.Rotate(Vector3.up * horizontal);
            Camera.main.transform.Rotate(Vector3.right * vertical);
        }
    }

    // H�m n�y s? ???c g?i khi b?t ??u t??ng t�c v?i NPC ?? kh�a quay camera
    public void LockCameraRotation(bool lockRotation)
    {
        canRotateCamera = !lockRotation;
    }
}
