using UnityEngine;

namespace Multi2D
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField, Range(1f, 10f)] public float Speed { get; private set; } = 5f;
        [field: SerializeField, Range(1f, 10f)] public float ClimbingSpeed { get; private set; } = 4f;
        [field: SerializeField, Range(1f, 20f)] public float InAirHorizontalModifier { get; private set; } = 2f;
        [field: SerializeField, Range(0.1f, 0.5f)] public float CoyotyTime { get; private set; } = 0.25f;
        [field: SerializeField, Range(1, 20)] public float MaxVerticalFallSpeed { get; private set; } = 10;

        [field: Space]
        [field: SerializeField, Range(0.5f, 5f)] public float JumpHeight { get; private set; } = 2f;
        [field: SerializeField, Range(0.1f, 3f)] public float JumpTimeToApex { get; private set; } = 0.5f;
        [field: SerializeField, Range(1f, 3f)] public float FallGravityModifier { get; private set; } = 1f;

        [field: Space]
        [field: SerializeField, Range(60, 200)] public int FireRateBPM { get; private set; } = 80;       
        [field: SerializeField, Range(5f, 20f)] public float BulletSpeed { get; private set; } = 15f;

        [field: Space]
        [field: SerializeField] public CollisionDetectionConfig CollisionDetectionConfig { get; private set; }

        public float Gravity => (2 * JumpHeight) / (JumpTimeToApex * JumpTimeToApex);
    }
}
