using Multi2D.Data;
using Multi2D.Extensions;
using Multi2D.FSM;
using UnityEngine;

namespace Multi2D.States
{
    public class PlayerClimbState : PlayerFsmStateBase
    {
        private readonly IInputDataProvider inputDataProvider;
        private readonly PlayerFsmStateChangeRequester stateChangeRequester;
        private readonly PlayerAnimationController animationController;
        private readonly PlayerModel model;
        private readonly PlayerConfig config;
        private readonly CollisionDetector collisionDetector;
        private Vector2 velocity;

        public PlayerClimbState(
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
            model.SetState(PlayerStateType.Climb);
            animationController.PlayClimb();
        }

        public override void Exit(){}

        public override void Update(float deltaTime)
        {
            velocity = model.Velocity.CurrentValue;
            FrameInput frameInput = inputDataProvider.FrameInput;

            if (!collisionDetector.CanClimbDown() && !collisionDetector.CanClimbUp())
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerFallState>();
                return;
            }

            if (frameInput.JumpPerformed)
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerJumpState>();
                return;
            }

            //if (collisionDetector.CanClimbDown() && 
            //    !collisionDetector.CanClimbUp())
            //{
            //    if(frameInput.Direction.HasHorizontalComponent())
            //        stateChangeRequester.RequestToChangeStateTo<PlayerMoveState>();
            //    else
            //        stateChangeRequester.RequestToChangeStateTo<PlayerIdleState>();
            //    return;
            //}

            velocity = frameInput.Direction * config.ClimbingSpeed;
            model.SetVelocity(velocity);
        }
    }
}
