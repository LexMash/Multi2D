using System;
using UnityEngine;

namespace Multi2D
{
    public class PlayerAnimationEventStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private AnimationEventType eventType;
        [SerializeField, Range(0f, 1f)] private float triggerTime;

        private bool isInitialized = false;

        private IAnimationEventReceiver receiver;
        private bool hasTriggered;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(!isInitialized)
            {
                receiver ??= animator.GetComponent<IAnimationEventReceiver>();
                isInitialized = true;
            }             

            hasTriggered = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float currentTime = stateInfo.normalizedTime % 1f;
            bool canTriggered = currentTime >= triggerTime;

            if (!hasTriggered && canTriggered)
                receiver.Receive(eventType);

            hasTriggered = canTriggered;
        }
    }
}
