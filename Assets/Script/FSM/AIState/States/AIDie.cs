using UnityEngine;

public class AIDie: AIBaseState
{
    public override void EnterState(AIStateManager StateMain)
    {
        StateMain.animator.Play("AIDie");
    }
    public override void UpdateState(AIStateManager StateMain)
    {

    }
    public override void ExitState(AIStateManager StateMain, AIBaseState state)
    {
        StateMain.SwitchState(state);
    }
}
