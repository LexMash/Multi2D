using UnityEngine;

namespace Multi2D
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private const int CURRENT_LAYER_INDEX = 0;

        private readonly int IdleHash = Animator.StringToHash("Idle");
        private readonly int RunHash = Animator.StringToHash("Run");
        private readonly int FireHash = Animator.StringToHash("Fire");
        private readonly int RunFireHash = Animator.StringToHash("RunFire");
        private readonly int JumpHash = Animator.StringToHash("Jump");
        private readonly int FallHash = Animator.StringToHash("Fall");
        private readonly int ClimbHash = Animator.StringToHash("Climb");
        private readonly int TakeDamageHash = Animator.StringToHash("TakeDamage");

        [SerializeField] private Animator animator;    

        public float CurrentAnimationProgress => animator.GetCurrentAnimatorStateInfo(CURRENT_LAYER_INDEX).normalizedTime;
        public void PlayRun() => animator.Play(RunHash);
        public void PlayFire() => animator.Play(FireHash);
        public void PlayRunAndFire() => animator.Play(RunFireHash);
        public void PlayIdle() => animator.Play(IdleHash);
        public void PlayJump() => animator.Play(JumpHash);
        public void PlayFall() => animator.Play(FallHash);
        public void PlayClimb() => animator.Play(ClimbHash);
        public void PlayTakeDamage() => animator.Play(TakeDamageHash);

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }
    }
}
