using System;
using UnityEngine;

namespace Multi2D.Data.Collisions
{
    [Serializable]
    public struct CollisionLayerMask
    {
        public CollisionLayerType Type;
        public LayerMask Mask;
    }
}
