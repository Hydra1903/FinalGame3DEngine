using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public GameObject taskPanel; // Panel ch?a nhi?m v?
    public Text taskNameText;    // Hi?n th? tên nhi?m v?
    public Text taskDescriptionText; // Hi?n th? mô t? nhi?m v?
    public Button acceptButton; // Nút nh?n nhi?m v?

    private Task currentTask;

    // Hi?n th? nhi?m v? lên UI
    public void ShowTask(Task task)
    {
        currentTask = task;
        taskPanel.SetActive(true);
        taskNameText.text = task.TaskName;
        taskDescriptionText.text = task.Description;

        // L?ng nghe s? ki?n khi ng??i ch?i nh?n nút nh?n nhi?m v?
        acceptButton.onClick.AddListener(AcceptTask);
    }

    // Khi ng??i ch?i nh?n nhi?m v?
    void AcceptTask()
    {
        Debug.Log($"Nhi?m v? '{currentTask.TaskName}' ?ã ???c nh?n.");
        // B?n có th? thêm mã x? lý nhi?m v? ?ã ???c ch?n t?i ?ây
        taskPanel.SetActive(false); // ?n b?ng nhi?m v? sau khi nh?n
    }
}
