using Multi2D.Data;
using Multi2D.Extensions;
using Multi2D.FSM;
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
        private readonly float coyoteTime;

        private bool coyoteTimeAvailable = false;
        private float fallPerformedTime;

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

            gravity = config.InitialJumpGravity * config.FallGravityModifier;
            coyoteTime = config.CoyoteTime;
        }

        public override void Enter()
        {
            coyoteTimeAvailable = model.State.CurrentValue == PlayerStateType.Run;
            if (coyoteTimeAvailable)
                fallPerformedTime = Time.time;

            model.SetState(PlayerStateType.Fall);
            animationController.PlayFall();
        }

        public override void Update(float deltaTime)
        {
            FrameInput frameInput = inputDataProvider.FrameInput;

            if (TrySwitchState(frameInput))
                return;

            Vector2 velocity = model.Velocity.CurrentValue;
            velocity.y -= gravity * deltaTime;
            velocity.y = Mathf.Max(velocity.y, -config.MaxVerticalFallSpeed);

            if(frameInput.Direction.HasHorizontalComponent() && Mathf.Sign(frameInput.Direction.x) != Mathf.Sign(velocity.x))
                velocity.x = frameInput.Direction.x * config.InAirHorizontalModifier;

            model.SetVelocity(velocity);
        }

        public override void Exit(){}

        private bool TrySwitchState(FrameInput frameInput)
        {
            if (collisionDetector.IsGrounded())
            {
                if (frameInput.JumpPerformed && Time.time - frameInput.JumpPerformedTime <= config.JumpBuffer)
                {
                    stateChangeRequester.RequestToChangeStateTo<PlayerJumpState>();
                    return true;
                }

                if (frameInput.Direction.HasHorizontalComponent())
                    stateChangeRequester.RequestToChangeStateTo<PlayerMoveState>();
                else
                    stateChangeRequester.RequestToChangeStateTo<PlayerIdleState>();
                return true;
            }

            if (coyoteTimeAvailable && frameInput.JumpPerformed && fallPerformedTime - frameInput.JumpPerformedTime <= coyoteTime)
            {
                coyoteTimeAvailable = false;
                stateChangeRequester.RequestToChangeStateTo<PlayerJumpState>();
                return true;
            }

            return false;
        }
    }
}
