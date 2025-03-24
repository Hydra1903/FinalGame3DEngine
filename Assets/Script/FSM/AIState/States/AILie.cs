using System.Collections;
using UnityEngine;

public class AILie : AIBaseState
{
    public override void EnterState(AIStateManager StateMain)
    {
        StateMain.animator.Play("AILie");
        StateMain.StartCoroutine(ChangeStateAfterDelay(StateMain));
    }

    public override void UpdateState(AIStateManager StateMain)
    {
        
    }

    public override void ExitState(AIStateManager StateMain, AIBaseState state)
    {
        StateMain.SwitchState(state);
    }

    private IEnumerator ChangeStateAfterDelay(AIStateManager StateMain)
    {
        yield return new WaitForSeconds(Random.Range(6, 8)); 
        ExitState(StateMain, StateMain.AISitUp); 
    }
}
