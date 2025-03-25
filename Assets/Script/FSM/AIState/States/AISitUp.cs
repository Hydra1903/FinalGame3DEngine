using UnityEngine;

public class AISitUp : AIBaseState
{
    public override void EnterState(AIStateManager StateMain)
    {
        StateMain.animator.Play("AISitUp");
    }
    public override void UpdateState(AIStateManager StateMain)
    {
        if (StateMain.animator.GetCurrentAnimatorStateInfo(0).IsName("AISitUp") && StateMain.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            ExitState(StateMain, StateMain.AIIdle);
        }
    }
    public override void ExitState(AIStateManager StateMain, AIBaseState state)
    {
        StateMain.SwitchState(state);
    }
}

