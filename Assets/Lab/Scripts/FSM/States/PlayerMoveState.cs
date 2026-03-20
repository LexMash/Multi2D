using Multi2D.Data;
using Multi2D.Extensions;
using Multi2D.FSM;
using UnityEngine;

namespace Multi2D.States
{
    public class PlayerMoveState : PlayerFsmStateBase
    {
        private readonly IInputDataProvider inputDataProvider;
        private readonly PlayerFsmStateChangeRequester stateChangeRequester;
        private readonly PlayerAnimationController animationController;
        private readonly PlayerModel model;
        private readonly PlayerConfig config;
        private readonly CollisionDetector collisionDetector;
        private readonly AttackController attackController;
        private Vector2 velocity;

        public PlayerMoveState(
            IInputDataProvider inputDataProvider,
            PlayerFsmStateChangeRequester stateChangeRequester, 
            PlayerAnimationController animationController, 
            PlayerModel model, 
            PlayerConfig config, 
            CollisionDetector collisionDetector,
            AttackController attackController)
        {
            this.inputDataProvider = inputDataProvider;
            this.stateChangeRequester = stateChangeRequester;
            this.animationController = animationController;
            this.model = model;
            this.config = config;
            this.collisionDetector = collisionDetector;
            this.attackController = attackController;
        }

        public override void Enter()
        {
            model.SetState(PlayerStateType.Run);
            animationController.PlayRun();
        }

        public override void Update(float deltaTime)
        {
            velocity = model.Velocity.CurrentValue;
            FrameInput frameInput = inputDataProvider.FrameInput;

            if (model.State.CurrentValue == PlayerStateType.TakeHit)
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerHurtState>();
                return;
            }

            if (frameInput.AttackPerformed)
                attackController.FireOnMove();

            if (!collisionDetector.IsGrounded())
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerFallState>();
                return;
            }

            if (frameInput.JumpPerformed && Time.time - frameInput.JumpPerformedTime <= config.JumpBuffer)
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerJumpState>();
                return;
            }

            if (!frameInput.Direction.HasHorizontalComponent())
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerIdleState>();
                return;
            }

            //if (collisionDetector.CanClimbUp() &&
            //    frameInput.Direction.HasVerticalComponent())
            //{
            //    stateChangeRequester.RequestToChangeStateTo<PlayerClimbState>();
            //    return;
            //}

            velocity.x = frameInput.Direction.x * config.Speed;
            velocity.y = -config.OnGroundGravity;
            model.SetVelocity(velocity);
        }

        public override void Exit(){}
    }
}
