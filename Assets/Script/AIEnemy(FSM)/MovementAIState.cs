
    public abstract class MovementBaseStateAI
    {
        public abstract void EnterStateAI(MovementStateManager movement);
        public abstract void UpdateStateAI(MovementStateManager movement);
        public abstract void ExitStateAI(MovementStateManager movement, MovementBaseState state);
    }


