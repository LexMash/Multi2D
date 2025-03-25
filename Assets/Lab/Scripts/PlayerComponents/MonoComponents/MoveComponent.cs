using UnityEngine;

namespace Multi2D
{
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;

        public Vector2 CurrentPosition => targetTransform.position;

        public void SetVelocity(Vector2 velocity)
        {
            var position = targetTransform.position;
            position += (Vector3)velocity;
            targetTransform.position = position;
        }

        private void Reset()
        {
            targetTransform = GetComponent<Transform>();
        }
    }
}
