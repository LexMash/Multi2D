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
        private readonly PlayerConfig config;
        private readonly CollisionDetector collisionDetector;
        private bool doesAttackAnimationPlay = false;
        private bool isBackInMainState = true;

        public PlayerIdleState(
            IInputDataProvider inputProvider,
            PlayerFsmStateChangeRequester stateChangeRequester, 
            PlayerAnimationController animationController,
            PlayerModel model,
            PlayerConfig config,
            CollisionDetector collisionDetector)
        {
            this.inputProvider = inputProvider;
            this.stateChangeRequester = stateChangeRequester;
            this.animationController = animationController;
            this.model = model;
            this.config = config;
            this.collisionDetector = collisionDetector;
        }

        public override void Enter()
        {
            model.SetState(PlayerStateType.Idle);
            animationController.PlayIdle();
            model.SetVelocity(Vector2.zero);
        }

        public override void Update(float deltaTime)
        {
            if (model.State.CurrentValue == PlayerStateType.TakeHit)
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerHurtState>();
                return;
            }

            FrameInput frameInput = inputProvider.FrameInput;
            var velocity = model.Velocity.CurrentValue;

            if (frameInput.JumpPerformed && Time.time - frameInput.JumpPerformedTime <= config.JumpBuffer)
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerJumpState>();
                return;
            }        
            
            if (frameInput.Direction.HasHorizontalComponent())
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerMoveState>();
                return;
            }

            if (collisionDetector.CanClimbUp() &&
                frameInput.Direction.HasVerticalComponent())
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerClimbState>();
                return;
            }

            velocity.y = -config.OnGroundGravity;
            model.SetVelocity(velocity);
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
