using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Multi2D
{
    public class Bullet : MonoBehaviour
    {
        private readonly int DestroyAnimationHash = Animator.StringToHash("destroy");

        [SerializeField] private Animator animator;
        [SerializeField] private new Collider2D collider;

        private Transform selfTransform;
        private int ownerID;

        private void Start()
        {
            selfTransform = transform;
        }

        private Vector2 velocity = Vector2.zero;

        public void Init(Vector2 velocity)
        {
            this.velocity = velocity;
            collider.enabled = true;
        }

        public void Setup(Vector2 velocity, int ownerID)
        {
            this.velocity = velocity;
            this.ownerID = ownerID;
            collider.enabled = true;
        }

        private void Update()
        {
            selfTransform.Translate(velocity * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var go = collision.gameObject;

            if (go.layer == LayerMask.NameToLayer("Players") //TODO move to config
                && go.TryGetComponent<IAttacker>(out var attacker)
                && attacker.ID == ownerID) 
            {
                return;
            }

            velocity = Vector2.zero;
            collider.enabled = false;
            WaitAnimationAndDestroy().Forget();
        }

        private async UniTask WaitAnimationAndDestroy()
        {
            await animator.PlayAndWait(DestroyAnimationHash);
            Destroy(gameObject); //TODO BACK TO POOL
        }
    }
}
