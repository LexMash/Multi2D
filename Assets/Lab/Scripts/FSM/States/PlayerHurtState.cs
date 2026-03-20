using Multi2D.Data;
using Multi2D.Extensions;
using Multi2D.FSM;
using UnityEngine;

namespace Multi2D.States
{
    public class PlayerHurtState : PlayerFsmStateBase
    {
        private readonly PlayerFsmStateChangeRequester stateChangeRequester;
        private readonly IInputReader inputReader;
        private readonly PlayerAnimationController animationController;
        private readonly CollisionDetector collisionDetector;
        private readonly PlayerModel model;
        private readonly float hurtTime;
        private readonly float gravity;
        private readonly float jumpVelocity;
        private float counter;
        private Vector2 velocity;

        public PlayerHurtState(
            IInputReader inputReader,
            PlayerFsmStateChangeRequester stateChangeRequester,
            PlayerAnimationController animationController,
            PlayerModel model,
            PlayerConfig playerConfig,
            CollisionDetector collisionDetector)
        {
            this.inputReader = inputReader;
            this.stateChangeRequester = stateChangeRequester;
            this.animationController = animationController;
            this.collisionDetector = collisionDetector;
            this.model = model;
            hurtTime = playerConfig.HurtTime;
            gravity = playerConfig.InitialJumpGravity;
            jumpVelocity = playerConfig.InitialJumpGravity * playerConfig.JumpTimeToApex * 0.5f;
        }

        public override void Enter() 
        {
            inputReader.Disable();
            animationController.PlayHurt();
            counter = 0;
            collisionDetector.ExcludeBulletAndCoinsLayers();
            var hitPos = model.LastHitData.HitPosition;
            var charPos = model.Position.CurrentValue;
            var direction = (charPos - hitPos).normalized;
            var jumpDirectionX = 2 * direction.x; //TODO проверять направление откуда попала пуля
            model.SetVelocity(new Vector2(jumpDirectionX, jumpVelocity));
        }

        public override void Update(float deltaTime)
        {
            counter += deltaTime;
            velocity = model.Velocity.CurrentValue;

            if (counter >= hurtTime)
            {
                if (collisionDetector.IsGrounded())
                    stateChangeRequester.RequestToChangeStateTo<PlayerIdleState>();
                else
                    stateChangeRequester.RequestToChangeStateTo<PlayerFallState>();

                return;
            }

            velocity.y -= gravity * deltaTime;
            model.SetVelocity(velocity);
        }

        public override void Exit()
        {          
            collisionDetector.ResetExcludeLayers();
            inputReader.Enable();
        }
    }
}
