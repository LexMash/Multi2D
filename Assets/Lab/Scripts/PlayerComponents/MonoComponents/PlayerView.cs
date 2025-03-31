using Multi2D.Assets.Lab.Scripts.PlayerComponents;
using Multi2D.Data;
using Multi2D.Extensions;
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

        [SerializeField, Range(1, 5)] private int steps = 2;

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
        private float dt;

        private void Start() //DELETE after tests
        {
            VectorsExtensions.AxisInputTreshold = PlayerConfig.AxisInputTreshold;

            model = new(1, Vector2.zero, 1);

            collisionDetector = new(ObservableCollisionsDetector, PlayerConfig.CollisionDetectionConfig, model, GetComponent<Rigidbody2D>());
            collisionDetector.Initialize();

            inputReader = new LocalInputReader(new LocalMultiplayerInput());
            inputReader.Enable();

            stateChangeRequester = new();
            playerStateMachine = new(stateChangeRequester);

            idleState = new PlayerIdleState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig);
            moveState = new PlayerMoveState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);
            jumpState = new PlayerJumpState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig);
            fallState = new PlayerFallState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);

            playerStateMachine
                .RegisterState(idleState, true)
                .RegisterState(moveState)
                .RegisterState(jumpState)
                .RegisterState(fallState);

            collisionHandler = new(model, collisionDetector, inputReader);
            playerStateMachine.Initialize();
            FlipComponent.Initialize(model);

            dt = Time.fixedDeltaTime / steps;
        }

        private void FixedUpdate()
        {          
            for(int i = 0; i < steps; i++)
            {
                inputReader.UpdateFrameInput();
                FlipComponent.UpdateDirection(inputReader.FrameInput.Direction);
                collisionDetector.UpdateCollisionData();           
                playerStateMachine.Update(dt);
                collisionHandler.HandleCollisions();
                Vector2 velocity = model.Velocity.CurrentValue * dt;
                MoveComponent.SetVelocity(velocity);
                model.SetPosition(MoveComponent.CurrentPosition);
            }
        }
    }
}
