using Multi2D.Data.Collisions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multi2D
{
    [Serializable]
    public class CollisionDetectionConfig
    {
        [field: Header("Forward")]
        [field: SerializeField] public Vector2 ForwardOverlapBoxSize { get; private set; } = new Vector2(0.1f, 0.1f);
        [field: SerializeField] public Vector3 ForwardOverlapOffset { get; private set; }
        [field: SerializeField] public LayerMask ForwardMask { get; private set; }

        [field: Header("Top")]
        [field: SerializeField] public Vector2 TopOverlapBoxSize { get; private set; } = new Vector2(0.1f, 0.1f);
        [field: SerializeField] public Vector3 TopOverlapOffset { get; private set; }
        [SerializeField] private List<CollisionLayerMask> topCollisionLayers;

        [field: Header("Bottom")]
        [field: SerializeField] public Vector2 BottomOverlapBoxSize { get; private set; } = new Vector2(0.1f, 0.1f);
        [field: SerializeField] public Vector3 BottomOverlapOffset { get; private set; }

        [SerializeField] private List<CollisionLayerMask> bottomCollisionLayers;

        [field: Header("Common")]
        [field: SerializeField] public LayerMask OverlapMask { get; private set; }     
        [field: SerializeField] public LayerMask ColliderContactCaptureLayers { get; private set; }

        public IReadOnlyList<CollisionLayerMask> TopCollisionsLayers => topCollisionLayers;
        public IReadOnlyList<CollisionLayerMask> BottomCollisionsLayers => bottomCollisionLayers;
    }
}
