using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestGiver : NPCScript
{
    public bool AssignedQuest { get; set; }
    public bool Helped {  get; set; }
    /*public override void Interact()
    {
        base.Interact();
        if(!AssignedQuest && !Helped)
        {

        }
    }*/
}
