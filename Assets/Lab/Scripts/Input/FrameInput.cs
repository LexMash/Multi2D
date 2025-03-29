using UnityEngine;

namespace Multi2D
{
    public struct FrameInput
    {
        public Vector2 Direction;
        public bool JumpPerformed;
        public bool AttackPerformed;
        public float JumpPerformedTime;

        public override string ToString()
            => $"Direction {Direction}, JumpPerformed {JumpPerformed}, AttackPerformed {AttackPerformed}, JumpPerformedTime {JumpPerformedTime}";
    }
}