namespace Multi2D
{
    public abstract class PlayerFsmStateBase
    {
        public abstract void Enter();
        public virtual void Update() { }
        public abstract void Exit();
    }
}
