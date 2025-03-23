using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 30f; 
    public float lifetime = 5f; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            Destroy(other.gameObject); 
            Destroy(gameObject); 
        }
    }
}