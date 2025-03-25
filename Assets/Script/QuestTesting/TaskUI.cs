using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public GameObject taskPanel; 
    public Text taskNameText;    
    public Text taskDescriptionText; 
    public Button acceptButton; 

    private Task currentTask;

    public void ShowTask(Task task)
    {
        currentTask = task;
        taskPanel.SetActive(true);
        taskNameText.text = task.TaskName;
        taskDescriptionText.text = task.Description;

       
        acceptButton.onClick.AddListener(AcceptTask);
        Debug.Log("Button is pressed1");
    }

    // Khi ng??i ch?i nh?n nhi?m v?
    void AcceptTask()
    {
        Debug.Log($"Nhiem vu '{currentTask.TaskName}' da chap nhan.");
        
        taskPanel.SetActive(false); 
    }
}
