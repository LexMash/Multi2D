using System;
using UnityEngine;

namespace Multi2D.Data.Collisions
{
    [Serializable]
    public struct CollisionLayerMask
    {
        public CollisionDetectionMask DetectionMask;
        public LayerMask LayerMask;
    }
}
