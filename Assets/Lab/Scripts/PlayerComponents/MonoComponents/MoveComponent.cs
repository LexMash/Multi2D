using UnityEngine;

namespace Multi2D
{
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D target;

        public Vector2 CurrentPosition => target.position;
        public void SetVelocity(Vector2 velocity) => target.position += velocity;
        private void Reset() => target = GetComponent<Rigidbody2D>();
    }
}
