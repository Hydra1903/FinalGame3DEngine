using System.Collections;
using UnityEngine;

public class AIIdle : AIBaseState
{
    private int randomValue;
    private bool hasRandomized = false;

    public override void EnterState(AIStateManager StateMain)
    {
        StateMain.animator.Play("AIIdle");
        StateMain.StartCoroutine(Randomize(StateMain));
        Debug.Log("Goi Ham");
        hasRandomized = false;
    }

    public override void UpdateState(AIStateManager StateMain)
    {
        if (!hasRandomized) return;

        switch (randomValue)
        {
            case 0:
                ExitState(StateMain, StateMain.AIWalk);
                break;
            case 1:
                ExitState(StateMain, StateMain.AILie);
                break;
        }
    }

    public override void ExitState(AIStateManager StateMain, AIBaseState state)
    {
        hasRandomized = false; 
        StateMain.SwitchState(state);
    }

    private IEnumerator Randomize(AIStateManager StateMain)
    {
        yield return new WaitForSeconds(Random.Range(8, 11));
        randomValue = Random.Range(0, 2);
        hasRandomized = true;
    }
}

