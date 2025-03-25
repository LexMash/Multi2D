using Multi2D.Data;
using Multi2D.FSM;
using UnityEngine;

namespace Multi2D.States
{
    public class PlayerJumpState : PlayerFsmStateBase
    {
        private readonly IInputDataProvider inputDataProvider;
        private readonly PlayerFsmStateChangeRequester stateChangeRequester;
        private readonly PlayerAnimationController animationController;
        private readonly PlayerModel model;
        private readonly PlayerConfig config;
        private readonly float gravity;
        private readonly float jumpVelocity;

        private bool doesAttackAnimationPlay;
        private Vector2 velocity;

        public PlayerJumpState(
            IInputDataProvider inputDataProvider,
            PlayerFsmStateChangeRequester stateChangeRequester,
            PlayerAnimationController animationController,
            PlayerModel model,
            PlayerConfig config)
        {
            this.inputDataProvider = inputDataProvider;
            this.stateChangeRequester = stateChangeRequester;
            this.animationController = animationController;
            this.model = model;
            this.config = config;

            gravity = config.Gravity;
            jumpVelocity = gravity * config.JumpTimeToApex;
        }

        public override void Enter()
        {
            model.SetVelocity(new Vector2(model.Velocity.CurrentValue.x, jumpVelocity));
            model.SetState(PlayerStateType.Jump);
            animationController.PlayJump();
        }

        public override void Update(float deltaTime)
        {
            velocity = model.Velocity.CurrentValue;
            FrameInput frameInput = inputDataProvider.FrameInput;

            if (!frameInput.JumpPerformed || velocity.y <= 0f)
            {
                velocity.y = 0f;
                model.SetVelocity(velocity);
                stateChangeRequester.RequestToChangeStateTo<PlayerFallState>();
                return;
            }

            if (frameInput.AttackPerformed)
            {
                //TODO
            }

            velocity.y -= gravity * deltaTime;
            model.SetVelocity(velocity);
        }

        public override void Exit()
        {
        }

        private bool CanStartAttackAnimation()
            => !doesAttackAnimationPlay || animationController.CurrentAnimationProgress >= 0.75f;
    }
}
