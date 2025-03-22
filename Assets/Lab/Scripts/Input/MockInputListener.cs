using UnityEngine;

namespace Multi2D
{
    public partial class LocalInputReceiver
    {
        public class MockInputListener : IInputListener
        {
            public void FirePerformed(bool active) => Debug.Log($"I'm firing {active}");
            public void JumpPerformed(bool active) => Debug.Log($"I'm jumping {active}");
            public void MovePerformed(Vector2 normalizedDirection) => Debug.Log($"I'm moving {normalizedDirection}");
        }
    }
}
