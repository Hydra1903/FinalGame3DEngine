using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditorInternal.ReorderableList;
public enum UIState
{
    Lobby,
    Default,
    Setting 
}
public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    private MovementStateManager movementStateManager;

    private UIState currentState;
    private StatsManager statsManager;
    public Image healthBar;
    public Image manaBar;
    public Text manaNumber;
    public GameObject highlightBow;
    public GameObject highlightSword;

    public Animator HealthBar;
    public Animator Tool;
    public Animator GoldMission;
    public GameObject Panel;
    public Animator Setting;

    public Animator Logo;
    public Button ClickToStart;
    public Text textClickToStart;
    public GameObject Vici;

    public Button English;
    public Button Vietnamese;

    public Text ClickOnStart;
    public Text Mission;
    public Text Settingg;
    public Text Sound;
    public Text Music;
    public Text Language;
    public Text Control;
    public Text CameraSpeed;
    public Text Control2;
    public Text Run;
    public Text UseBow;
    public Text UseSword;
    public Text Jump;
    public Text Attack;
    public Text Blocking;

    public Text valueMusic;
    public Text valueSFX;
    public Text valueCameraSpeed;

    public Slider sliderMusic;
    public Slider sliderSFX;
    public Slider sliderCameraSpeed;
    public Slider sliderFOV;

    public void ChangeState(UIState newState)
    {
        currentState = newState;
    }
    // LƯU NGÔN NGỮ TRƯỚC KHI THOÁT GAME
    
    void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("Music", sliderMusic.value);
        PlayerPrefs.SetFloat("SFX", sliderSFX.value);
        PlayerPrefs.SetFloat("CameraSpeed", sliderCameraSpeed.value);
        PlayerPrefs.SetFloat("FOV", sliderFOV.value);
        PlayerPrefs.Save();
    }
    
    void Start()
    {
        LocalizationManager.Language = PlayerPrefs.GetString("Language", "Vietnamese");
        UpdateUIText();

        sliderMusic.value = PlayerPrefs.GetFloat("Music",50);
        sliderSFX.value = PlayerPrefs.GetFloat("SFX", 50);
        sliderCameraSpeed.value = PlayerPrefs.GetFloat("CameraSpeed", 50);
        sliderFOV.value = PlayerPrefs.GetFloat("FOV", 60);

        statsManager = FindAnyObjectByType<StatsManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        movementStateManager = FindAnyObjectByType<MovementStateManager>();
        ClickToStart.onClick.AddListener(ButtonClickToStart);
        currentState = UIState.Lobby;

        English.onClick.AddListener(LanguageEnglish);
        Vietnamese.onClick.AddListener(LanguageVietnamese);

    }

    void UpdateUIText()
    {
        ClickOnStart.text = LocalizationManager.GetLocalizedText("Start");
        Mission.text = LocalizationManager.GetLocalizedText("Lobby.Mission");
        Settingg.text = LocalizationManager.GetLocalizedText("Lobby.Setting");
        Sound.text = LocalizationManager.GetLocalizedText("Lobby.Sound");
        Music.text = LocalizationManager.GetLocalizedText("Lobby.Music");
        Language.text = LocalizationManager.GetLocalizedText("Lobby.Language");
        Control.text = LocalizationManager.GetLocalizedText("Lobby.Control");
        CameraSpeed.text = LocalizationManager.GetLocalizedText("Lobby.CameraSpeed");
        Control2.text = LocalizationManager.GetLocalizedText("Lobby.Control");
        Run.text = LocalizationManager.GetLocalizedText("Lobby.Run");
        UseBow.text = LocalizationManager.GetLocalizedText("Lobby.UseBow");
        UseSword.text = LocalizationManager.GetLocalizedText("Lobby.UseSword");
        Jump.text = LocalizationManager.GetLocalizedText("Lobby.Jump");
        Attack.text = LocalizationManager.GetLocalizedText("Lobby.Attack");
        Blocking.text = LocalizationManager.GetLocalizedText("Lobby.Blocking");
    }
    void LanguageEnglish()
    {
        LocalizationManager.Language = "English";
        PlayerPrefs.SetString("Language", "English");
        PlayerPrefs.Save();
        UpdateUIText();
    }
    void LanguageVietnamese()
    {
        LocalizationManager.Language = "Vietnamese";
        PlayerPrefs.SetString("Language", "Vietnamese");
        PlayerPrefs.Save();
        UpdateUIText();
    }
    void Update()
    {
        Debug.Log(PlayerPrefs.GetString("Language", "Chưa có"));
        if (Input.GetKeyDown(KeyCode.Escape) && currentState == UIState.Default)
        {
            OnUISetting();
            OffUIDefault();
            currentState = UIState.Setting;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && currentState == UIState.Setting)
        {
            OffUISetting();
            OnUIDefault();
            currentState = UIState.Default;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
        else if (currentState == UIState.Lobby)
        {
            OnUILobby();
        }
        manaBar.fillAmount = statsManager.Character1.mana / statsManager.Character1.maxMana;
        manaNumber.text = statsManager.Character1.mana.ToString("F0");

        valueMusic.text = ((int)(sliderMusic.value * 100f)).ToString() + "%";
        valueSFX.text = ((int)(sliderSFX.value * 100f)).ToString() + "%";
        valueCameraSpeed.text = ((int)(sliderCameraSpeed.value * 100f)).ToString() + "%";

        movementStateManager.Camera.fieldOfView = sliderFOV.value;

        Color color = textClickToStart.color;
        color.a = Mathf.Lerp(0.05f, 1f, Mathf.PingPong(Time.time * 1f, 1));
        textClickToStart.color = color;
    }
    public void ButtonClickToStart()
    {
        ChangeState(UIState.Default);
        OnUIDefault();
        OffUILobby();
        gameManager.animator.Play("CameraTransition");
    }
    void OnUIDefault()
    {
        HealthBar.Play("HealthBar(ON)");
        Tool.Play("Tool(ON)");
        GoldMission.Play("Gold&Mission(ON)");
    }
    void OffUIDefault()
    {

        HealthBar.Play("HealthBar(OFF)");
        Tool.Play("Tool(OFF)");
        GoldMission.Play("Gold&Mission(OFF)");
    }
    void OffUILobby()
    {
        Logo.Play("LOGO(OFF)");
        ClickToStart.animator.Play("Button(ClickOnStart)(OFF)");
        Vici.SetActive(false);
    }
    void OnUILobby()
    {
        Logo.Play("LOGO(ON)");
        ClickToStart.animator.Play("Button(ClickOnStart)(ON)");
    }
    void OnUISetting()
    {
        Panel.SetActive(true);
        Setting.Play("Setting(ON)");
    }
    void OffUISetting()
    {
        Panel.SetActive(false);
        Setting.Play("Setting(OFF)");
    }
}
