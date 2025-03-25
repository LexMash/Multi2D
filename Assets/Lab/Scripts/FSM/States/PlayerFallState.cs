using Multi2D.Data;
using Multi2D.Extensions;
using Multi2D.FSM;
using R3;
using System;
using UnityEngine;

namespace Multi2D.States
{
    public class PlayerFallState : PlayerFsmStateBase
    {
        private readonly IInputDataProvider inputDataProvider;
        private readonly PlayerFsmStateChangeRequester stateChangeRequester;
        private PlayerAnimationController animationController;
        private PlayerModel model;
        private PlayerConfig config;
        private CollisionDetector collisionDetector;
        private readonly float gravity;
        private readonly float coyotyTime;
        private float coyotyTimeCounter = 0;

        public PlayerFallState(
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

            gravity = config.Gravity * config.FallGravityModifier;
            coyotyTime = config.CoyotyTime;
        }

        public override void Enter()
        {
            if (model.State.CurrentValue == PlayerStateType.Run)
                coyotyTimeCounter = 0f;

            model.SetState(PlayerStateType.Fall);
            animationController.PlayFall();
        }

        public override void Update(float deltaTime)
        {
            FrameInput frameInput = inputDataProvider.FrameInput;

            if (collisionDetector.IsGrounded())
            {
                if(frameInput.Direction.HasHorizontalComponent())
                    stateChangeRequester.RequestToChangeStateTo<PlayerMoveState>();
                else
                    stateChangeRequester.RequestToChangeStateTo<PlayerIdleState>();
                return;
            }

            if (coyotyTimeCounter <= coyotyTime && frameInput.JumpPerformed)
            {
                stateChangeRequester.RequestToChangeStateTo<PlayerJumpState>();
                return;
            }

            Vector2 velocity = model.Velocity.CurrentValue;
            velocity.y -= gravity * deltaTime;
            velocity.x += frameInput.Direction.x * config.InAirHorizontalModifier * deltaTime;
            coyotyTimeCounter += deltaTime;
            model.SetVelocity(velocity);
        }

        public override void Exit(){}

        public void Dispose()
        {

        }
    }
}
