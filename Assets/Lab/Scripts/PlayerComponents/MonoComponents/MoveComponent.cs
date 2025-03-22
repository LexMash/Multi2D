using UnityEngine;

namespace Multi2D
{
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;

        private Vector3 velocity;
        public Vector2 CurrentPosition => targetTransform.position;

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        private void Update()
        {
            var position = targetTransform.position;
            position += velocity * Time.deltaTime;
            targetTransform.position = position;
        }

        private void Reset()
        {
            targetTransform = GetComponent<Transform>();
        }
    }
}
