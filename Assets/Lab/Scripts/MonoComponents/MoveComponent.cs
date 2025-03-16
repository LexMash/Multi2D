using UnityEngine;

namespace Multi2D
{
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private Vector3 velocity;
        public Vector2 CurrentPosition => target.position;

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        private void Update()
        {
            var position = target.position;
            position += velocity;
            target.position = position;
        }
    }
}
