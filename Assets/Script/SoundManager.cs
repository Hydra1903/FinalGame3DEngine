using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private UIManager uiManager;

    public AudioSource audioMusic;
    public AudioSource audioMusic2;
    public AudioSource audioSfx;
    public AudioSource audioMove;

    public AudioClip MeleeAttack;
    public AudioClip BowShoot1;
    public AudioClip BowShoot2;
    public AudioClip Walk;
    public AudioClip Run;

    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    void Update()
    {
        audioMusic.volume = uiManager.sliderMusic.value;
        audioSfx.volume = uiManager.sliderSFX.value;
        audioMove.volume = uiManager.sliderSFX.value;
    }
}
