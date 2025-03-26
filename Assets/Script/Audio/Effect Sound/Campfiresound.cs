using UnityEngine;

public class Campfiresound : MonoBehaviour
{
    private UIManager UIManager;
    public Transform player;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UIManager = FindAnyObjectByType<UIManager>();
    }
    void Update()
    {
        audioSource.volume = UIManager.sliderSFX.value;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < audioSource.maxDistance)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
