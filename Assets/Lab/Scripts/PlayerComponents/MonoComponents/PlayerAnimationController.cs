using System;
using UnityEngine;

namespace Multi2D
{
    public class PlayerAnimationController : MonoBehaviour, IAnimationEventReceiver
    {
        private const int CURRENT_LAYER_INDEX = 0;

        private static readonly int isMovingHash = Animator.StringToHash("isMoving");
        private static readonly int isInAirHash = Animator.StringToHash("isInAir");
        private static readonly int isClimbingHash = Animator.StringToHash("isClimbing");
        private static readonly int idleHash = Animator.StringToHash("Idle");
        private static readonly int runHash = Animator.StringToHash("Run");
        private static readonly int fireHash = Animator.StringToHash("Fire");
        private static readonly int jumpHash = Animator.StringToHash("Jump");
        private static readonly int fallHash = Animator.StringToHash("Fall");
        private static readonly int climbHash = Animator.StringToHash("Climb");
        private static readonly int hurtHash = Animator.StringToHash("Hurt");
        private static readonly int verticalSpeedParamHash = Animator.StringToHash("verticalSpeed");
        private static readonly int horizontalSpeedParamHash = Animator.StringToHash("horizontalSpeed");

        [SerializeField] private Animator animator;

        public event Action<AnimationEventType> EventTriggered = delegate { };

        public float CurrentAnimationProgress => animator.GetCurrentAnimatorStateInfo(CURRENT_LAYER_INDEX).normalizedTime;

        public void PlayRun()
        {
            animator.SetBool(isMovingHash, true);
            animator.SetBool(isInAirHash, false);
            animator.SetBool(isClimbingHash, false);
            animator.Play(runHash);
        }

        public void PlayAttack() => animator.Play(fireHash);

        public void PlayIdle()
        {
            animator.SetBool(isMovingHash, false);
            animator.SetBool(isClimbingHash, false);
            animator.SetBool(isInAirHash, false);
            animator.Play(idleHash);
        }

        public void PlayJump()
        {
            animator.SetBool(isInAirHash, true);
            animator.SetBool(isClimbingHash, false);
            animator.SetBool(isMovingHash, false);
            animator.Play(jumpHash);
        }

        public void PlayFall()
        {
            animator.SetBool(isInAirHash, true);
            animator.SetBool(isClimbingHash, false);
            animator.SetBool(isMovingHash, false);
            animator.Play(fallHash);
        }

        public void PlayClimb()
        {
            animator.SetBool(isClimbingHash, true);
            animator.SetBool(isInAirHash, false);
            animator.SetBool(isMovingHash, false);
            animator.Play(climbHash);
        }

        public void PlayHurt() => animator.Play(hurtHash);
        public void SetVerticalSpeed(float speed) => animator.SetFloat(verticalSpeedParamHash, speed);
        void IAnimationEventReceiver.Receive(AnimationEventType eventType) => EventTriggered.Invoke(eventType);

        private void Reset() => animator = GetComponent<Animator>();
    }
}
