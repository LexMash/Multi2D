using System;

namespace Multi2D.FSM
{
    public sealed class PlayerFsmStateChangeRequester : IChangeStateRequestedNotifier
    {
        public event Action<Type> ChangeStateRequested = delegate { };

        public void RequestToChangeStateTo<T>() where T : PlayerFsmStateBase 
            => ChangeStateRequested.Invoke(typeof(T));
    }
}
