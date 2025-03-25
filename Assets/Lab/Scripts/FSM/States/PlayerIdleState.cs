using Multi2D.Data;
using Multi2D.Extensions;
using Multi2D.FSM;
using UnityEngine;

namespace Multi2D.States
{
    public class PlayerIdleState : PlayerFsmStateBase
    {
        private readonly IInputDataProvider inputProvider;
        private readonly PlayerFsmStateChangeRequester stateChangeRequester;
        private readonly PlayerAnimationController animationController;
        private readonly PlayerModel model;

        private bool doesAttackAnimationPlay = false;
        private bool isBackInMainState = true;

        public PlayerIdleState(
            IInputDataProvider inputProvider,
            PlayerFsmStateChangeRequester stateChangeRequester, 
            PlayerAnimationController animationController,
            PlayerModel model)
        {
            this.inputProvider = inputProvider;
            this.stateChangeRequester = stateChangeRequester;
            this.animationController = animationController;
            this.model = model;
        }

        public override void Enter()
        {
            model.SetState(PlayerStateType.Idle);
            animationController.PlayIdle();
            model.SetVelocity(Vector2.zero);
        }

        public override void Update(float deltaTime)
        {
            FrameInput frameInput = inputProvider.FrameInput;

            if (frameInput.JumpPerformed)
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerJumpState>();
                return;
            }        
            
            if (frameInput.Direction.HasHorizontalComponent())
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerMoveState>();
                return;
            }

            if (frameInput.AttackPerformed && CanStartAttackAnimation())
            {
                //TODO
            }
            else if (!isBackInMainState)
            {
                //TODO
            }
        }

        public override void Exit()
        {
            doesAttackAnimationPlay = false;
            isBackInMainState = true;
        }

        private bool CanStartAttackAnimation() 
            => !doesAttackAnimationPlay || animationController.CurrentAnimationProgress >= 0.75f;
    }
}
