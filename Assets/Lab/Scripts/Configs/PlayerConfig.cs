using UnityEngine;

namespace Multi2D
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField, Range(1f, 10f)] public float Speed { get; private set; } = 5f;
        [field: SerializeField, Range(0.5f, 5f)] public float JumpHeight { get; private set; } = 1f;
        [field: SerializeField, Range(0.1f, 3f)] public float JumpTime { get; private set; } = 0.5f;
        [field: SerializeField, Range(0.01f, 1f)] public float FireRate { get; private set; } = 0.1f;
    }
}
