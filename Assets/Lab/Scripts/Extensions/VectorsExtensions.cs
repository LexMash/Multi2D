using UnityEngine;

namespace Multi2D.Extensions
{
    public static class VectorsExtensions
    {
        public static bool HasHorizontalComponent(this Vector2 vector) => HasComponent(vector.x);
        public static bool HasVerticalComponent(this Vector2 vector) => HasComponent(vector.y);

        public static bool HasHorizontalComponent(this Vector3 vector)
        {
            Vector2 horizontalVector = new (vector.x, vector.z);
            return HasComponent(horizontalVector.sqrMagnitude);
        }

        public static bool HasVerticalComponent(this Vector3 vector) => HasComponent(vector.y);

        private static bool HasComponent(float value) => Mathf.Abs(value) > Mathf.Epsilon;
    }
}
