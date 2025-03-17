using System;
using UnityEngine;

namespace Multi2D
{
    public class PlayerAnimationController : MonoBehaviour, IAnimationEventReceiver
    {
        private const int CURRENT_LAYER_INDEX = 0;

        private readonly int idleHash = Animator.StringToHash("Idle");
        private readonly int runHash = Animator.StringToHash("Run");
        private readonly int fireHash = Animator.StringToHash("Fire");
        private readonly int jumpHash = Animator.StringToHash("Jump");
        private readonly int fallHash = Animator.StringToHash("Fall");
        private readonly int climbHash = Animator.StringToHash("Climb");
        private readonly int hurtHash = Animator.StringToHash("Hurt");
        private readonly int speedParamHash = Animator.StringToHash("speed");

        [SerializeField] private Animator animator;

        public event Action<AnimationEventType> EventTriggered = delegate { };
        public float CurrentAnimationProgress => animator.GetCurrentAnimatorStateInfo(CURRENT_LAYER_INDEX).normalizedTime;
        public void PlayRun() => animator.Play(runHash);
        public void PlayFire() => animator.Play(fireHash);
        public void PlayIdle() => animator.Play(idleHash);
        public void PlayJump() => animator.Play(jumpHash);
        public void PlayFall() => animator.Play(fallHash);
        public void PlayClimb() => animator.Play(climbHash);
        public void PlayHurt() => animator.Play(hurtHash);
        public void SetSpeed(float speed) => animator.SetFloat(speedParamHash, speed > 0.01f ? 1f : 0f);
        void IAnimationEventReceiver.Receive(AnimationEventType eventType) => EventTriggered.Invoke(eventType);

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }
    }
}
