using System.Collections;
using UnityEngine;

public class AIWalk : AIBaseState
{
    private int randomValue;
    private bool hasRandomized = false;

    public override void EnterState(AIStateManager StateMain)
    {
        StateMain.animator.Play("AIWalk");
        StateMain.StartCoroutine(Randomize(StateMain));
        hasRandomized = false;
    }

    public override void UpdateState(AIStateManager StateMain)
    {
        if (!hasRandomized) return;

        switch (randomValue)
        {
            case 0:
                ExitState(StateMain, StateMain.AIIdle);
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
        yield return new WaitForSeconds(Random.Range(5, 7));
        randomValue = Random.Range(0, 2);
        hasRandomized = true;
    }
}
