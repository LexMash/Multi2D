using UnityEngine;

namespace Multi2D
{ 
    public class AttackController : MonoBehaviour
    {
        [SerializeField] private Transform attacker;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletOrigin;

        public float BulletSpeed = 5;
        public float firePause = 0.3f;

        private float counter = 0;
        private PlayerAnimationController animController;

        public void Init(PlayerAnimationController animController)
        {
            this.animController = animController;
            animController.EventTriggered += OnFireEventTriggered;
        }

        public void Attack()
        {
            if (counter <= 0)
                animController.PlayAttack();
        }

        public void FireOnMove()
        {
            Fire();
        }

        private void Fire()
        {
            if (counter <= 0)
            {
                var bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
                bullet.transform.localScale = attacker.localScale;
                bullet.Init(attacker.localScale.x * Vector2.right * BulletSpeed);
                counter = firePause;
            }           
        }

        private void OnFireEventTriggered(AnimationEventType type)
        {
            if(type == AnimationEventType.Fire)
            {
                Fire();
            }
        }

        private void Update()
        {
            if(counter > 0)
            {
                counter -= Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            if(animController != null)
                animController.EventTriggered -= OnFireEventTriggered;
        }
    }
}
