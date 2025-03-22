using System;
using UnityEngine;

namespace Multi2D
{
    public interface IInputReceiver
    {
        void SetListener(IInputListener listener);
        void Disable();
        void Enable();
    }
}