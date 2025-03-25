using Multi2D.Data;

namespace Multi2D.Extensions
{
    public static class CollisionDetectorExtensions
    {
        public static bool CanMoveForward(this CollisionDetector collisionDetector) 
            => !collisionDetector.DetectedMask.HasFlag(Data.CollisionDetectionMask.Forward);

        public static bool IsGrounded(this CollisionDetector collisionDetector) 
            => collisionDetector.DetectedMask.HasFlag(Data.CollisionDetectionMask.Ground);

        public static bool CanMoveUp(this CollisionDetector collisionDetector) 
            => !collisionDetector.DetectedMask.HasFlag(CollisionDetectionMask.Top);
    }
}
