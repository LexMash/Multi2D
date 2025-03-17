using UnityEngine;

namespace Multi2D.Data.Collisions
{
    public struct CollisionStruct
    {
        public CollisionDirectionType Direction;
        public CollisionLayerType Layer;
        public Collider2D Collider;
        public Collision2D Collision;
    }
}