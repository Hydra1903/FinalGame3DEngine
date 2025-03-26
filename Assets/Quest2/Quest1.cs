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
    public float interactionRadius = 3f; // Bán kính týõng tác
    private bool isPlayerNearby = false;
    private bool isPanelOpen = false;
    private Dictionary<int, (string description, int currentAmount, int requiredAmount, int reward)> activeQuests = new Dictionary<int, (string, int, int, int)>(); // Danh sách nhi?m v?
    public CinemachineOrbitalFollow orbitalTransposer;
    private int playerMoney = 0;

    void Start()
    {
        UpdateMoneyText();
        questButton1.onClick.AddListener(() => ReceiveQuest(1, "Quest 1 Kill: 5 Goat", 5, 100, questButton1));
        questButton2.onClick.AddListener(() => ReceiveQuest(2, "Quest 2 Kill: 10 Sheep", 10, 200, questButton2));
        questButton3.onClick.AddListener(() => ReceiveQuest(3, "Quest 3 Kill: 1 Bear", 1, 1000, questButton3));

        questButton2.gameObject.SetActive(false); // ?n nhi?m v? 2 ban ð?u
        questButton3.gameObject.SetActive(false); // ?n nhi?m v? 3 ban ð?u
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


    void ReceiveQuest(int questID, string description, int requiredAmount, int reward, Button questButton)
    {
        if (!activeQuests.ContainsKey(questID)) // Ch? thêm nhi?m v? n?u chýa nh?n
        {
            activeQuests[questID] = (description, 0, requiredAmount, reward);
            UpdateQuestText();
            Debug.Log("Nhan nhiem vu: " + description);
            questButton.gameObject.SetActive(false); // ?n nút nh?n nhi?m v?
        }
        ToggleQuestPanel(false); // Ðóng b?ng sau khi nh?n nhi?m v?
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
                Debug.Log("Tien ðo nhiem vu: " + quest.description + " - " + quest.currentAmount + "/" + quest.requiredAmount);

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
            Debug.Log("Hoàn thành nhiem vu: " + quest.description + " - Nhan " + quest.reward + " tien!");
            activeQuests.Remove(questID);
            UpdateQuestText();

            if (questID == 1) questButton2.gameObject.SetActive(true);
            if (questID == 2) questButton3.gameObject.SetActive(true);
        }
    }

    void UpdateQuestText()
    {
        List<string> questDescriptions = new List<string>();
        foreach (var quest in activeQuests.Values)
        {
            questDescriptions.Add(quest.description + " (" + quest.currentAmount + "/" + quest.requiredAmount + ")");
        }
        questText.text = string.Join("\n", questDescriptions); // C?p nh?t danh sách nhi?m v?
    }

    void UpdateMoneyText()
    {
        moneyText.text = " " + playerMoney;
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
