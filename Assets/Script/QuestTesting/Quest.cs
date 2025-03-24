using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Quest : MonoBehaviour
{
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string QuestName {  get; set; }
    public string Description { get; set; }
    public int MoneyReward {  get; set; }
    public bool Completed {  get; set; }

    public void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
    }
    void GiveReward()
    {
        if (MoneyReward != null) ;   
            //code nhan tien va hien thi so tien da nhan
    }
}
