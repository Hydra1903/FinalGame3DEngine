using UnityEngine;

public enum Weaponstate
{
    Bow,
    Sword
}
public class WeaponState : MonoBehaviour
{
    private UIManager uiManager;
    public Weaponstate currentState;
    public GameObject Bow1;
    public GameObject Bow2;
    public GameObject Sword;

    public void ChangeState(Weaponstate newState)
    {
        currentState = newState;
    }

    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        currentState = Weaponstate.Bow;
    }

    void Update()
    {
        HandleState();
    }
    public void HandleState()
    {
        switch (currentState)
        {
            case Weaponstate.Bow:
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Bow1.SetActive(false);
                    Bow2.SetActive(false);
                    Sword.SetActive(true);
                    uiManager.highlightBow.SetActive(false);
                    uiManager.highlightSword.SetActive(true);
                    currentState = Weaponstate.Sword;
                }

                break;
            case Weaponstate.Sword:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Bow1.SetActive(true);
                    Bow2.SetActive(true);
                    Sword.SetActive(false);
                    uiManager.highlightSword.SetActive(false);
                    uiManager.highlightBow.SetActive(true);
                    currentState = Weaponstate.Bow;
                }
                break;
        }
    }
}
