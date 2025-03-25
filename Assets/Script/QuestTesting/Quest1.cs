using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quest1 : MonoBehaviour
{
    public GameObject questPanel; // B?ng nhi?m v?
    public Text questText; // Text UI hi?n th? nhi?m v?
    public Text moneyText; // Text UI hi?n th? s? ti?n
    public Button questButton1;
    public Button questButton2;
    public Button questButton3;
    public float interactionRadius = 3f; // B�n k�nh t��ng t�c
    private bool isPlayerNearby = false;
    private bool isPanelOpen = false;
    private Dictionary<int, (string description, int currentAmount, int requiredAmount, int reward)> activeQuests = new Dictionary<int, (string, int, int, int)>(); // Danh s�ch nhi?m v?
    public CinemachineOrbitalFollow orbitalTransposer;
    private int playerMoney = 0;

    void Start()
    {
        UpdateMoneyText();
        questButton1.onClick.AddListener(() => ReceiveQuest(1, "Nhi?m v?: Ti�u di?t 5 qu�i v?t", 5, 100));
        questButton2.onClick.AddListener(() => ReceiveQuest(2, "Nhi?m v?: Thu th?p 3 vi�n �� qu?", 3, 150));
        questButton3.onClick.AddListener(() => ReceiveQuest(3, "Nhi?m v?: Gi�p �? d�n l�ng", 1, 200));
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleQuestPanel(true);
        }
    }

    void ToggleQuestPanel(bool open)
    {
        questPanel.SetActive(open);
        isPanelOpen = open;

        if (open)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            orbitalTransposer.enabled = false; // T?t camera xoay
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            orbitalTransposer.enabled = true; // B?t l?i camera
        }
    }

    void ReceiveQuest(int questID, string description, int requiredAmount, int reward)
    {
        if (!activeQuests.ContainsKey(questID)) // Ch? th�m nhi?m v? n?u ch�a nh?n
        {
            activeQuests[questID] = (description, 0, requiredAmount, reward);
            UpdateQuestText();
            Debug.Log("Nh?n nhi?m v?: " + description);
        }
        ToggleQuestPanel(false); // ��ng b?ng sau khi nh?n nhi?m v?
    }

    public void UpdateQuestProgress(int questID)
    {
        if (activeQuests.ContainsKey(questID))
        {
            var quest = activeQuests[questID];
            if (quest.currentAmount < quest.requiredAmount)
            {
                quest.currentAmount++;
                activeQuests[questID] = (quest.description, quest.currentAmount, quest.requiredAmount, quest.reward);
                UpdateQuestText();
                Debug.Log("Ti?n �? nhi?m v?: " + quest.description + " - " + quest.currentAmount + "/" + quest.requiredAmount);

                if (quest.currentAmount >= quest.requiredAmount)
                {
                    CompleteQuest(questID);
                }
            }
        }
    }

    void CompleteQuest(int questID)
    {
        if (activeQuests.ContainsKey(questID))
        {
            var quest = activeQuests[questID];
            playerMoney += quest.reward;
            UpdateMoneyText();
            Debug.Log("Ho�n th�nh nhi?m v?: " + quest.description + " - Nh?n " + quest.reward + " ti?n!");
            activeQuests.Remove(questID);
            UpdateQuestText();
        }
    }

    void UpdateQuestText()
    {
        List<string> questDescriptions = new List<string>();
        foreach (var quest in activeQuests.Values)
        {
            questDescriptions.Add(quest.description + " (" + quest.currentAmount + "/" + quest.requiredAmount + ")");
        }
        questText.text = string.Join("\n", questDescriptions); // C?p nh?t danh s�ch nhi?m v?
    }

    void UpdateMoneyText()
    {
        moneyText.text = "Ti?n: " + playerMoney;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            ToggleQuestPanel(false);
        }
    }
}
