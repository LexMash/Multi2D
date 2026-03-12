using Multi2D.Data;

namespace Multi2D.Extensions
{
    public static class CollisionDetectorExtensions
    {
        public static bool CanMoveForward(this CollisionDetector collisionDetector) 
            => !collisionDetector.DetectedMask.HasFlag(CollisionDetectionMask.Forward);

        public static bool IsGrounded(this CollisionDetector collisionDetector) 
            => collisionDetector.DetectedMask.HasFlag(CollisionDetectionMask.Ground);

        public static bool CanMoveUp(this CollisionDetector collisionDetector) 
            => !collisionDetector.DetectedMask.HasFlag(CollisionDetectionMask.Top);

        public static bool CanClimbUp(this CollisionDetector collisionDetector) 
            => collisionDetector.DetectedMask.HasFlag(CollisionDetectionMask.LadderUp);

        public static bool CanClimbDown(this CollisionDetector collisionDetector) 
            => collisionDetector.DetectedMask.HasFlag(CollisionDetectionMask.LadderDown);
    }
}
