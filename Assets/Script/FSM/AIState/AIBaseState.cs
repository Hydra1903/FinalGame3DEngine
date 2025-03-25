using UnityEngine;
public abstract class AIBaseState
{
    public abstract void EnterState(AIStateManager StateMain);
    public abstract void UpdateState(AIStateManager StateMain);
    public abstract void ExitState(AIStateManager StateMain, AIBaseState state);
}
