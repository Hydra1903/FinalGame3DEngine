
    public abstract class MovementAIState
    {
        public abstract void EnterStateAI(AiMoventd movement);
        public abstract void UpdateStateAI(AiMoventd movement);
        public abstract void ExitStateAI(AiMoventd movement, MovementAIState state);
    }


