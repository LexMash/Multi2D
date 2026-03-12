using UnityEngine;

namespace Multi2D
{ 
    public class AttackController : MonoBehaviour
    {
        [SerializeField] private Transform attacker;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletOrigin;

        public float BulletSpeed = 5; //TODO MOVE TO CONFIG
        public float firePause = 0.2f;

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

        public void FireOnMove() //MOVE HIGHER
        {
            Fire();
        }

        private void Fire()
        {
            if (counter <= 0)
            {
                var bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
                bullet.transform.localScale = attacker.localScale; //remove add flip info
                bullet.Setup(attacker.localScale.x * BulletSpeed * Vector2.right, 0); //TODO remove zero ID
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
