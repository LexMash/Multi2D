using Multi2D.Data.Collisions;
using System.Collections.Generic;
using UnityEngine;

namespace Multi2D
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject, ICollisionsConfig
    {
        [field: SerializeField, Range(1, 5)] public int Rounds { get; private set; } = 1;
        [field: SerializeField, Range(1, 10)] public float RoundTimeInMinutes { get; private set; } = 2;
        [field: SerializeField, Range(1, 10)] public int CoinsLostWhenHit { get; private set; } = 10;
        [field: SerializeField, Range(0.5f, 5f)] public float CoinsSpawnIntervalInSeconds { get; private set; } = 1f;
        [field: SerializeField] public Color[] PlayerColors { get; private set; }

        [SerializeField] private List<CollisionLayerMask> collisionLayers;
        public IReadOnlyList<CollisionLayerMask> CollisionLayers => collisionLayers;
    }
}
