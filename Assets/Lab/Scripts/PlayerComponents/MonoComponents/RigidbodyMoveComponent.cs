using UnityEngine;

namespace Multi2D
{
    public class RigidbodyMoveComponent : MonoBehaviour //TODO test it
    {
        [SerializeField] private Rigidbody2D target;

        private Vector2 velocity;
        public Vector2 CurrentPosition => target.position;

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        private void FixedUpdate()
        {
            Vector2 position = target.position;
            position += velocity * Time.fixedDeltaTime;
            target.position = position;
        }
    }
}
