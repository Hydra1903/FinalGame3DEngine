using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public GameObject taskPanel; // Panel ch?a nhi?m v?
    public Text taskNameText;    // Hi?n th? t�n nhi?m v?
    public Text taskDescriptionText; // Hi?n th? m� t? nhi?m v?
    public Button acceptButton; // N�t nh?n nhi?m v?

    private Task currentTask;

    // Hi?n th? nhi?m v? l�n UI
    public void ShowTask(Task task)
    {
        currentTask = task;
        taskPanel.SetActive(true);
        taskNameText.text = task.TaskName;
        taskDescriptionText.text = task.Description;

        // L?ng nghe s? ki?n khi ng??i ch?i nh?n n�t nh?n nhi?m v?
        acceptButton.onClick.AddListener(AcceptTask);
    }

    // Khi ng??i ch?i nh?n nhi?m v?
    void AcceptTask()
    {
        Debug.Log($"Nhi?m v? '{currentTask.TaskName}' ?� ???c nh?n.");
        // B?n c� th? th�m m� x? l� nhi?m v? ?� ???c ch?n t?i ?�y
        taskPanel.SetActive(false); // ?n b?ng nhi?m v? sau khi nh?n
    }
}
