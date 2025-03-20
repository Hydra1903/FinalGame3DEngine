
    public abstract class MovementAIState
    {
        public abstract void EnterStateAI(AiMovement movement);
        public abstract void UpdateStateAI(AiMovement movement);
        public abstract void ExitStateAI(AiMovement movement, MovementAIState state);
    }


