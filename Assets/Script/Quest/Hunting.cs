using UnityEngine;

public class Hunting : Quest
{
    private void Start()
    {
        QuestName = "GoatKiller";
        Description = "Kill A Bunch Of Stuff.";

        Goals.Add(new KillGoal(this,0, "Kill 5 Goat or Sheep", false, 0 , 5));
        Goals.Add(new KillGoal(this,1, "Kill 10 Goat or Sheep", false, 0 ,2));
        Goals.ForEach(g => g.Init());

    }
}
