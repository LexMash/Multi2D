using Multi2D.Assets.Lab.Scripts.PlayerComponents;
using Multi2D.Data;
using Multi2D.FSM;
using Multi2D.States;
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

        [SerializeField, Range(1,3)] private int steps = 2;
        [SerializeField, Range(0.01f,1f)] private float acceleration = 1f;

        public PlayerConfig PlayerConfig;
        private CollisionDetector collisionDetector;
        private CollisionHandler collisionHandler;
        private IInputReader inputReader;
        private PlayerStateMachine playerStateMachine;
        private PlayerFsmStateChangeRequester stateChangeRequester;
        private PlayerIdleState idleState;
        private PlayerMoveState moveState;
        private PlayerJumpState jumpState;
        private PlayerFallState fallState;
        private PlayerModel model;

        private Rigidbody2D rigidbody2;
        private Vector2 velocity = new Vector2();

        private void Start() //DELETE after tests
        {
            model = new(1, Vector2.zero, 1);

            collisionDetector = new(ObservableCollisionsDetector, PlayerConfig.CollisionDetectionConfig, model);
            collisionDetector.Initialize();

            inputReader = new LocalInputReader(new LocalMultiplayerInput());
            inputReader.Enable();

            stateChangeRequester = new();
            playerStateMachine = new(stateChangeRequester);

            idleState = new PlayerIdleState(inputReader, stateChangeRequester, AnimationController, model);
            moveState = new PlayerMoveState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);
            jumpState = new PlayerJumpState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig);
            fallState = new PlayerFallState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);

            playerStateMachine
                .RegisterState(idleState, true)
                .RegisterState(moveState)
                .RegisterState(jumpState)
                .RegisterState(fallState);

            //collisionHandler = new(model, collisionDetector);
            playerStateMachine.Initialize();
            FlipComponent.Initialize(model);

            rigidbody2 = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var dt = Time.fixedDeltaTime / steps;

            for(int i = 0; i < steps; i++)
            {
                inputReader.UpdateFrameInput();
                FlipComponent.UpdateDirection(inputReader.FrameInput.Direction);
                collisionDetector.UpdateCollisionData();           
                playerStateMachine.Update(dt);
                var velocity = model.Velocity.CurrentValue * dt;
                print(velocity);             
                MoveComponent.SetVelocity(velocity);
                model.SetPosition(MoveComponent.CurrentPosition);
            }
        }

        private void Update()
        {
            
        }
    }
}
