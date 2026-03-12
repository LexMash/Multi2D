namespace Multi2D
{
    public interface IPlayerStatesRegistrator
    {
        IPlayerStatesRegistrator RegisterState<T>(T state, bool isInitial = false) where T : PlayerFsmStateBase;
    }
}
