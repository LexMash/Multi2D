using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Multi2D
{
    public class Bullet : MonoBehaviour
    {
        private readonly int DestroyAnimationHash = Animator.StringToHash("destroy");

        [SerializeField] private Animator animator;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private Vector2 velocity = Vector2.zero;

        public void Init(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        private void Update()
        {
            _transform.Translate(velocity * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            velocity = Vector2.zero;
            WaitAnimationAndDestroy().Forget();
        }

        private async UniTask WaitAnimationAndDestroy()
        {
            await animator.PlayAndWait(DestroyAnimationHash);
            Destroy(gameObject);
        }
    }
}
