using UnityEngine;

public class CameraTPS : MonoBehaviour
{
    public Transform target;
    public float sensitivity = 300f;
    public float distance = 3f;
    public Vector2 pitchLimits = new Vector2(-30f, 60f);

    private float yaw, pitch;

    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = target.position + rotation * direction;
        transform.LookAt(target.position);
    }
}