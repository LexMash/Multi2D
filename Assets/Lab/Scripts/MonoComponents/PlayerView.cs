using UnityEngine;

namespace Multi2D
{
    public class PlayerView : MonoBehaviour
    {
        [field: SerializeField] public PlayerAnimationController AnimationController { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
        [field: SerializeField] public FlipComponent FlipComponent { get; private set; }
        [field: SerializeField] public CollisionDetector CollisionDetector { get; private set; }       
    }
}
