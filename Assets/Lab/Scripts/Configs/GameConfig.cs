using UnityEngine;

namespace Multi2D
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField, Range(1, 5)] public int Rounds { get; private set; } = 1;
        [field: SerializeField, Range(1f, 10f)] public float RoundTimeInMinutes { get; private set; } = 2f;
        [field: SerializeField] public Color[] PlayerColors { get; private set; }
    }
}
