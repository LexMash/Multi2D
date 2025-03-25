using Multi2D.Data;
using Multi2D.Extensions;
using UnityEngine;

namespace Multi2D.Assets.Lab.Scripts.PlayerComponents
{
    public class CollisionHandler
    {
        private readonly PlayerModel model;
        private readonly CollisionDetector collisionDetector;

        public CollisionHandler(
            PlayerModel model, 
            CollisionDetector collisionDetector)
        {
            this.model = model;
            this.collisionDetector = collisionDetector;
        }

        public void HandleCollisions()
        {
            if (!collisionDetector.CanMoveForward())
            {
                //frameVelocity.HorizontalSpeed = 0;
                model.SetVelocity(new Vector2(0, model.Velocity.CurrentValue.y));
            }
                
            if (!collisionDetector.CanMoveUp())
                model.SetVelocity(new Vector2(model.Velocity.CurrentValue.x, Mathf.Min(0f, model.Velocity.CurrentValue.y)));


            if (collisionDetector.IsGrounded())
                model.SetVelocity(new Vector2(model.Velocity.CurrentValue.x, Mathf.Max(0f, model.Velocity.CurrentValue.y)));
        }
    }
}
