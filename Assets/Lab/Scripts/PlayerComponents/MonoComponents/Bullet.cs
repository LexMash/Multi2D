using Cysharp.Threading.Tasks;
using Multi2D.Lab.Scripts.Data;
using Unity.Netcode;
using UnityEngine;

namespace Multi2D
{
    public class Bullet : NetworkBehaviour
    {
        private readonly int DestroyAnimationHash = Animator.StringToHash("destroy");

        [SerializeField] private Animator animator;
        [SerializeField] private new Collider2D collider;

        private Transform selfTransform;
        private ulong ownerID;

        private void Start()
        {
            selfTransform = transform;
        }

        private Vector2 velocity = Vector2.zero;
        private int mask;

        public void Init(Vector2 velocity)
        {
            this.velocity = velocity;
            collider.enabled = true;
        }

        public void Setup(Vector2 velocity, ulong ownerID)
        {
            this.velocity = velocity;
            this.ownerID = ownerID;
            collider.enabled = true;

            mask = LayerMask.NameToLayer("Players"); //TODO remove hardcode
        }

        private void Update()
        {
            if (!IsServer) return;

            selfTransform.Translate(velocity * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsServer) return;

            var go = collision.gameObject;

            if (go.layer == mask //TODO move to config
                && go.TryGetComponent<IHitable>(out var hitable)) 
            {
                Debug.Log($"PlayerCollision attacker {hitable.ID}, owner {ownerID}");

                if (hitable.ID == ownerID) 
                    return;

                hitable.TakeHitRpc(new HitData(selfTransform.position));
            }

            HandleCollisionRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void HandleCollisionRpc()
        {
            velocity = Vector2.zero;
            collider.enabled = false;

            AwaitAndDespawn().Forget();
        }

        private async UniTask AwaitAndDespawn()
        {
            if (!IsServer) return;

            await animator.PlayAndWait(DestroyAnimationHash);
            
            GetComponent<NetworkObject>().Despawn(); //TODO BACK TO POOL
        }
    }
}
