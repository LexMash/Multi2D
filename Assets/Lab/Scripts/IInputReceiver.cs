using System;
using UnityEngine;

namespace Multi2D
{
    public interface IInputReceiver
    {
        event Action AttackPerformed;
        event Action JumpPerformed;
        event Action<Vector2> MovePerformed;

        void Disable();
        void Enable();
    }
}