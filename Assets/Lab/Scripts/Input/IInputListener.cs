using UnityEngine;

namespace Multi2D
{
    public interface IInputListener
    {
        void MovePerformed(Vector2 normalizedDirection);
        void JumpPerformed(bool active);
        void FirePerformed(bool active);
    }
}