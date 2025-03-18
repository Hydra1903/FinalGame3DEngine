using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private StatsManager statsManager;
    public Image healthBar;
    public Image manaBar;
    void Start()
    {
        statsManager = FindAnyObjectByType<StatsManager>();
    }

    void Update()
    {
        manaBar.fillAmount = statsManager.Character1.mana / statsManager.Character1.maxMana;
    }
}
