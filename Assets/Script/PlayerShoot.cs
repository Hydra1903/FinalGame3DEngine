using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public Vector3 arrowOffset = Vector3.zero;
    public Transform Camera;
    public Vector3 angleOffset = Vector3.zero; 

    public void ShootArrow()
    {
        Vector3 spawnPosition = shootPoint.position + arrowOffset;
        Quaternion newRotation = Quaternion.Euler(
            Camera.eulerAngles.x + angleOffset.x,
            shootPoint.eulerAngles.y + angleOffset.y,
            shootPoint.eulerAngles.z + angleOffset.z
        );
        Instantiate(arrowPrefab, spawnPosition, newRotation);
    }
}
