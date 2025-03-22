namespace Multi2D
{
    public interface IPlayerStateMachine
    {
        void Initialize();
        void SetState<T>() where T : PlayerFsmStateBase;
        void Update(float deltaTime);
    }
}
