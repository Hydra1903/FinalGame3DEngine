using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private StatsManager statsManager;
    public Image healthBar;
    public Image manaBar;
    public Text manaNumber;
    public Outline outlineBow;
    void Start()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
    }

    void Update()
    {
        manaBar.fillAmount = statsManager.Character1.mana / statsManager.Character1.maxMana;
        manaNumber.text = statsManager.Character1.mana.ToString("F0");
    }

}
