using System;

namespace Multi2D
{
    public interface IAnimationEventReceiver
    {
        event Action<AnimationEventType> EventTriggered;
        public void Receive(AnimationEventType eventType);
    }
}
