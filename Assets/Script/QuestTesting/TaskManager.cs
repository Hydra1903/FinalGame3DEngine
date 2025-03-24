using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<Task> tasks = new List<Task>(); // Danh sách nhi?m v?

    // Hàm ?? thêm nhi?m v?
    public void AddTask(Task newTask)
    {
        tasks.Add(newTask);
    }

    // Hàm ?? l?y danh sách nhi?m v?
    public List<Task> GetTasks()
    {
        return tasks;
    }
}

public class Task
{
    public string TaskName;
    public string Description;
    public bool IsCompleted;

    public Task(string taskName, string description)
    {
        TaskName = taskName;
        Description = description;
        IsCompleted = false;
    }
}
