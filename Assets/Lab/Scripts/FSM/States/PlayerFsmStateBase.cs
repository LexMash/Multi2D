using UnityEngine;

namespace Multi2D
{
    public abstract class PlayerFsmStateBase : IInputListener
    {
        public abstract void Enter();
        public virtual void Update(float deltaTime) { }
        public abstract void Exit();

        public abstract void MovePerformed(Vector2 normalizedDirection);
        public abstract void JumpPerformed(bool active);
        public abstract void FirePerformed(bool active);
    }
}
