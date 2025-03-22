using UnityEngine;

namespace Multi2D
{
    [SelectionBase]
    public class PlayerView : MonoBehaviour
    {
        [field: SerializeField] public Transform BulletSpawnRoot { get; private set; }
        [field: SerializeField] public PlayerAnimationController AnimationController { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
        [field: SerializeField] public FlipComponent FlipComponent { get; private set; }
        [field: SerializeField] public ObservableCollisionsDetector ObservableCollisionsDetector { get; private set; }

        public PlayerConfig PlayerConfig;
        private CollisionDetector detector;
        private Vector2 velocity;
        private Rigidbody2D target;
        private LocalMultiplayerInput input;

        private void Start() //DELETE after tests
        {
            detector = new(ObservableCollisionsDetector, PlayerConfig.CollisionDetectionConfig);
            detector.Initialize();
            target = GetComponent<Rigidbody2D>();

            input = new LocalMultiplayerInput();
            input.PlayerControll.Move.performed += ctx => SetVelocity(ctx.ReadValue<Vector2>());
            input.PlayerControll.Move.canceled += ctx => SetVelocity(ctx.ReadValue<Vector2>());
            input.Enable();
        }

        private void FixedUpdate()
        {
            detector.UpdateCollisionData();
            Vector2 linear = target.linearVelocity;
            linear += velocity * Time.fixedDeltaTime;
            target.linearVelocity = linear;
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity * 5f;
        }
    }
}
