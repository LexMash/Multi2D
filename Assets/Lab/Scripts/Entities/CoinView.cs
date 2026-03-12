using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Multi2D.Assets.Lab.Scripts.Entities
{
    public class CoinView : MonoBehaviour
    {
        private static readonly int IdleAnimationHash = Animator.StringToHash("idle");
        private static readonly int CollectAnimationHash = Animator.StringToHash("collect");

        [SerializeField] private Animator animator;
        [SerializeField] private new Collider2D collider;
        [SerializeField] private Rigidbody2D rb;

        private Transform selfTransform;

        private void Start()
        {
            selfTransform = transform;
        }

        public void Init()
        {
            animator.Play(IdleAnimationHash);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collider.enabled = false;     
            WaitAnimationAndDestroy().Forget();
        }

        private async UniTask WaitAnimationAndDestroy()
        {
            await animator.PlayAndWait(CollectAnimationHash);
            Destroy(gameObject); //TODO BACK TO POOL
        }
    }
}
