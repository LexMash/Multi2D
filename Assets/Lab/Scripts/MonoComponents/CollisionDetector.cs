using System;
using UnityEngine;

namespace Multi2D
{
    public class CollisionDetector : MonoBehaviour
    {
        public event Action<Collision2D> OnCollisionEnter = delegate { };
        public event Action<Collision2D> OnCollisionExit = delegate { };
        public event Action<Collider2D> OnTriggerEnter = delegate { };
        public event Action<Collider2D> OnTriggerExit = delegate { };

        private void OnCollisionEnter2D(Collision2D collision) => OnCollisionEnter.Invoke(collision);
        private void OnCollisionExit2D(Collision2D collision) => OnCollisionExit.Invoke(collision);
        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter.Invoke(collision);
        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit.Invoke(collision);
    }
}
