using Multi2D.Data;
using Multi2D.Extensions;
using UnityEngine;

namespace Multi2D.Assets.Lab.Scripts.PlayerComponents
{
    public class CollisionHandler
    {
        private readonly PlayerModel model;
        private readonly CollisionDetector collisionDetector;
        private readonly IInputReader inputReader;

        public CollisionHandler(
            PlayerModel model, 
            CollisionDetector collisionDetector,
            IInputReader inputProvider)
        {
            this.model = model;
            this.collisionDetector = collisionDetector;
            this.inputReader = inputProvider;
        }

        public void HandleCollisions()
        {
            var currentVelocity = model.Velocity.CurrentValue;

            if (!collisionDetector.CanMoveForward())
                currentVelocity.x = 0;
                
            if (!collisionDetector.CanMoveUp())
            {
                inputReader.ResetJumpInput();
                currentVelocity.y = Mathf.Min(0f, currentVelocity.y);
            }

            //if (collisionDetector.IsGrounded() && currentVelocity.y < 0)
            //    currentVelocity.y = 0;

            model.SetVelocity(currentVelocity);
        }
    }
}
