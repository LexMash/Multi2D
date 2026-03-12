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
            if (!collisionDetector.CanMoveForward())
                model.SetVelocity(new Vector2(0, model.Velocity.CurrentValue.y));

            if (!collisionDetector.CanMoveUp())
            {
                inputReader.ResetJumpInput();
                model.SetVelocity(new Vector2(model.Velocity.CurrentValue.x, Mathf.Min(0f, model.Velocity.CurrentValue.y)));
            }
                

            //if (collisionDetector.IsGrounded())
            //    model.SetVelocity(new Vector2(model.Velocity.CurrentValue.x, Mathf.Max(0f, model.Velocity.CurrentValue.y)));
        }
    }
}
