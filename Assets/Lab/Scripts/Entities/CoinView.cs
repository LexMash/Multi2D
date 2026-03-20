using Cysharp.Threading.Tasks;
using Multi2D.Extensions;
using Unity.Netcode;
using UnityEngine;

namespace Multi2D.Assets.Lab.Scripts.Entities
{
    public class CoinView : NetworkBehaviour
    {
        private static readonly int IdleAnimationHash = Animator.StringToHash("idle");
        private static readonly int CollectAnimationHash = Animator.StringToHash("collect");

        [SerializeField] private Animator animator;
        [SerializeField] private new Collider2D collider;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private NetworkObject netObj;
        [SerializeField] private LayerMask ground;
        [SerializeField] private LayerMask player;

        public void Init()
        {
            netObj.Spawn();
            animator.Play(IdleAnimationHash);
            collider.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        public void Kick(Vector2 velocity)
        {
            if (!IsServer) return;

            rb.AddForce(velocity, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!player.Contains(collision.gameObject.layer))
                return;

            HandleCollisionRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void HandleCollisionRpc()
        {
            collider.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;

            WaitAndDespown().Forget();
        }

        private async UniTask WaitAndDespown()
        {
            await animator.PlayAndWait(CollectAnimationHash);

            if (!IsServer) return;

            netObj.Despawn(); //TODO BACK TO POOL
        }
    }
}
