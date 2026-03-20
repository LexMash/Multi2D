using UnityEngine;

namespace Multi2D.Extensions
{
    public static class PhysicsExtensions
    {
        public static bool IsSameLayer(this GameObject gameObject, LayerMask layerMask) 
            => (1 << gameObject.layer & layerMask.value) != 0;

        public static int ConvertGoLayerIndexToLayerMaskValue(this GameObject gameObject) 
            => 1 << gameObject.layer;

        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return (layerMask.value & (1 << layer)) != 0;
        }
    }
}
