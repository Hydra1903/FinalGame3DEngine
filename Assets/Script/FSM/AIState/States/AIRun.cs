using UnityEngine;

public class AIRun : AIBaseState
{
    public override void EnterState(AIStateManager StateMain)
    {
        StateMain.animator.Play("AIRun");
    }
    public override void UpdateState(AIStateManager StateMain)
    {

    }
    public override void ExitState(AIStateManager StateMain, AIBaseState state)
    {
        StateMain.SwitchState(state);
    }
}

