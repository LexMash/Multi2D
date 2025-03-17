using Multi2D.Data;
using System;
using UnityEngine;

namespace Multi2D
{
    public class CollisionDetector : MonoBehaviour
    {
        public event Action<Collision2D, CollisionDirectionType> OnCollision = delegate { };
        public event Action<Collider2D, CollisionDirectionType> OnTrigger = delegate { };

        private void OnCollisionEnter2D(Collision2D collision) => OnCollision.Invoke(collision, CollisionDirectionType.Enter);
        private void OnCollisionExit2D(Collision2D collision) => OnCollision.Invoke(collision, CollisionDirectionType.Exit);
        private void OnTriggerEnter2D(Collider2D collision) => OnTrigger.Invoke(collision, CollisionDirectionType.Enter);
        private void OnTriggerExit2D(Collider2D collision) => OnTrigger.Invoke(collision, CollisionDirectionType.Exit);
    }
}
