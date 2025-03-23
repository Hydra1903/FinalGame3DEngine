using UnityEngine;

public class KillGoal : Goal
{
    public int EnemyID { get; set; }
    public KillGoal(Quest quest,int enemyID, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.EnemyID = enemyID;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }
    public override void Init()
    {
        base.Init();
        //CombatEvent.OnenemyDeath += EnemyDied; 
        //for example if we on combat with enemy and enemy die this thing will plus to EnemyDied in script kill goal
    }
    void EnemyDied(IEnemy enemy)
    {
        if (enemy.ID == this.EnemyID)
        {
            this.CurrentAmount++;
            Evaluate();
        } 
    }
}
