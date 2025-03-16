namespace Multi2D
{
    public abstract class PlayerStateBase
    {
        public abstract void Enter();
        public virtual void Update() { }
        public abstract void Exit();
    }
}
