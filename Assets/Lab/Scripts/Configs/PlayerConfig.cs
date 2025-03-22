using UnityEngine;

namespace Multi2D
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField, Range(1f, 10f)] public float Speed { get; private set; } = 5f;
        [field: SerializeField, Range(1f, 10f)] public float ClimbingSpeed { get; private set; } = 4f;
        [field: SerializeField, Range(1f, 10f)] public float InAirSpeedHorizontal { get; private set; } = 2f;
        [field: SerializeField, Range(0.5f, 5f)] public float JumpHeight { get; private set; } = 1f;
        [field: SerializeField, Range(0.1f, 3f)] public float JumpTime { get; private set; } = 0.5f;
        [field: SerializeField, Range(0.01f, 1f)] public float FireRateBPM { get; private set; } = 80;       
        [field: SerializeField, Range(0.01f, 1f)] public float BulletSpeed { get; private set; } = 2f;
        [field: SerializeField] public CollisionDetectionConfig CollisionDetectionConfig { get; private set; }
    }
}
