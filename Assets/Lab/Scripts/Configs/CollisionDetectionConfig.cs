using Multi2D.Data.Collisions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multi2D
{
    [Serializable]
    public class CollisionDetectionConfig
    {
        [field: SerializeField] public Vector2 OverlapBoxSize { get; private set; } = new Vector2(0.1f, 0.1f);
        [field: SerializeField] public Vector3 OverlapOffset { get; private set; }
        [field: SerializeField] public LayerMask OverlapMask { get; private set; }

        [SerializeField] private List<CollisionLayerMask> topCollisionLayers;
        [SerializeField] private List<CollisionLayerMask> bottomCollisionLayers;

        [field: SerializeField] public LayerMask ColliderContactCaptureLayers { get; private set; }

        public IReadOnlyList<CollisionLayerMask> TopCollisionsLayers => topCollisionLayers;
        public IReadOnlyList<CollisionLayerMask> BottomCollisionsLayers => bottomCollisionLayers;
    }
}
