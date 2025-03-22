using System;

namespace Multi2D.FSM
{
    public interface IChangeStateRequestedNotifier
    {
        event Action<Type> ChangeStateRequested;
    }
}
