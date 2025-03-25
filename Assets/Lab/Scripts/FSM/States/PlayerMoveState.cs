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
        private Vector2 velocity;

        public PlayerMoveState(
            IInputDataProvider inputDataProvider,
            PlayerFsmStateChangeRequester stateChangeRequester, 
            PlayerAnimationController animationController, 
            PlayerModel model, 
            PlayerConfig config, 
            CollisionDetector collisionDetector)
        {
            this.inputDataProvider = inputDataProvider;
            this.stateChangeRequester = stateChangeRequester;
            this.animationController = animationController;
            this.model = model;
            this.config = config;
            this.collisionDetector = collisionDetector;
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

            if (!collisionDetector.IsGrounded())
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerFallState>();
                return;
            }

            if (frameInput.JumpPerformed)
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerJumpState>();
                return;
            }

            if (!frameInput.Direction.HasHorizontalComponent())
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerIdleState>();
                return;
            }
                
            if (frameInput.AttackPerformed)
            {
                //model.SetState(PlayerStateType.RunFire);
            }

            velocity.x = frameInput.Direction.x * config.Speed;
            model.SetVelocity(velocity);
        }

        public override void Exit(){}
    }
}
