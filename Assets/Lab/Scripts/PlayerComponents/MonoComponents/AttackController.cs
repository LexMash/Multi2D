using Unity.Netcode;
using UnityEngine;

namespace Multi2D
{ 
    public class AttackController : NetworkBehaviour
    {
        [SerializeField] private Transform attacker;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletOrigin;

        private float BulletSpeed;
        private float firePause;

        private ulong ownerID;
        private float counter = 0;
        private PlayerAnimationController animController;

        public void Init(PlayerAnimationController animController, PlayerConfig config)
        {
            this.animController = animController;
            BulletSpeed = config.BulletSpeed;
            firePause = 60f / config.FireRateBPM;
            animController.EventTriggered += OnFireEventTriggered;
        }

        public void SetID(ulong id)
        {
            ownerID = id;
        }

        public void Attack()
        {
            if (counter <= 0)
                animController.PlayAttack();
        }

        public void FireOnMove() //MOVE HIGHER
        {
            TryFire();
        }

        [Rpc(SendTo.Server)]
        private void FireRpc()
        {
            Bullet bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
            bullet.GetComponent<NetworkObject>().Spawn();
            bullet.transform.localScale = attacker.localScale; //remove add flip info
            bullet.Setup(attacker.localScale.x * BulletSpeed * Vector2.right, ownerID);
        }

        private void OnFireEventTriggered(AnimationEventType type)
        {
            if (type == AnimationEventType.Fire)
            {
                TryFire();
            }
        }

        private void TryFire()
        {
            if (!IsOwner) return;

            if (counter <= 0)
            {
                FireRpc();
                counter = firePause;
            }
        }

        private void Update()
        {
            if (!IsOwner) return;

            if(counter > 0)
            {
                counter -= Time.deltaTime;
            }
        }

        public override void OnDestroy()
        {
            if(animController != null)
                animController.EventTriggered -= OnFireEventTriggered;

            base.OnDestroy();
        }
    }
}
