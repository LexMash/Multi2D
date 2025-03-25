namespace Multi2D
{
    public interface IInputReader : IInputDataProvider
    {
        void UpdateFrameInput();
        void Disable();
        void Enable();
    }
}