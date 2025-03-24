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
    public Animator Map;
    public GameObject Panel;
    public Animator Setting;

    public Animator Logo;
    public Button ClickToStart;
    public Text textClickToStart;
    public GameObject Vici;
    public void ChangeState(UIState newState)
    {
        currentState = newState;
    }
    void Start()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        ClickToStart.onClick.AddListener(ButtonClickToStart);
        currentState = UIState.Lobby;
    }

    void Update()
    {
        Debug.Log(currentState);
        if (Input.GetKeyDown(KeyCode.Escape) && currentState == UIState.Default)
        {
            OnUISetting();
            OffUIDefault();
            currentState = UIState.Setting;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && currentState == UIState.Setting)
        {
            OffUISetting();
            OnUIDefault();
            currentState = UIState.Default;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
        else if (currentState == UIState.Lobby)
        {
            OnUILobby();
        }
        manaBar.fillAmount = statsManager.Character1.mana / statsManager.Character1.maxMana;
        manaNumber.text = statsManager.Character1.mana.ToString("F0");

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
        Map.Play("Map(ON)");
    }
    void OffUIDefault()
    {

        HealthBar.Play("HealthBar(OFF)");
        Tool.Play("Tool(OFF)");
        GoldMission.Play("Gold&Mission(OFF)");
        Map.Play("Map(OFF)");
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
