using Multi2D.Assets.Lab.Scripts.PlayerComponents;
using Multi2D.Data;
using Multi2D.Extensions;
using Multi2D.FSM;
using Multi2D.States;
using UnityEngine;

namespace Multi2D
{
    [SelectionBase]
    public class PlayerView : MonoBehaviour, IAttacker
    {
        [field: SerializeField] public Transform BulletSpawnRoot { get; private set; }
        [field: SerializeField] public PlayerAnimationController AnimationController { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
        [field: SerializeField] public FlipComponent FlipComponent { get; private set; }
        [field: SerializeField] public ObservableCollisionsDetector ObservableCollisionsDetector { get; private set; }

        [SerializeField] private Rigidbody2D rb;

        public AttackController AttackController; //TODO serializefield

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
        //private PlayerClimbState climbState;
        private PlayerModel model;
        private float dt;

        public CollisionDetectionMask detectedMask;

        public int ID { get; private set; }

        private void Start() //DELETE after tests
        {
            ID = 0; //TODO remove

            rb = GetComponent<Rigidbody2D>(); //TODO remove GetComp

            VectorsExtensions.AxisInputTreshold = PlayerConfig.AxisInputTreshold;

            //if owner

            model = new(1, Vector2.zero, 1);

            collisionDetector = new(ObservableCollisionsDetector, PlayerConfig.CollisionDetectionConfig, model, rb);
            collisionDetector.Initialize();

            inputReader = new LocalInputReader(new LocalMultiplayerInput());
            inputReader.Enable();

            stateChangeRequester = new();
            playerStateMachine = new(stateChangeRequester);

            idleState = new PlayerIdleState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);
            moveState = new PlayerMoveState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector, AttackController);
            jumpState = new PlayerJumpState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);
            fallState = new PlayerFallState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);
            //climbState = new PlayerClimbState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);

            playerStateMachine
                .RegisterState(idleState, true)
                .RegisterState(moveState)
                .RegisterState(jumpState)
                .RegisterState(fallState);
                //.RegisterState(climbState);

            collisionHandler = new(model, collisionDetector, inputReader);
            playerStateMachine.Initialize();
            FlipComponent.Initialize(model);

            dt = Time.fixedDeltaTime / steps;

            AttackController.Init(AnimationController);

            //if owner
        }

        private void FixedUpdate()
        {
            //if owner

            for (int i = 0; i < steps; i++)
            {
                inputReader.UpdateFrameInput();
                FlipComponent.UpdateDirection(inputReader.FrameInput.Direction);
                collisionDetector.UpdateCollisionData();
                detectedMask = collisionDetector.DetectedMask; //TODO remove
                playerStateMachine.Update(dt);
                collisionHandler.HandleCollisions();
                var currentVelocity = model.Velocity.CurrentValue;
                Vector2 velocity = currentVelocity * dt;
                MoveComponent.SetVelocity(velocity);
                AnimationController.SetVerticalSpeed(currentVelocity.y);

                if (inputReader.FrameInput.AttackPerformed) //TODO 
                    AttackController.Attack();

                model.SetPosition(MoveComponent.CurrentPosition);
            }

            //if owner
        }
    }

    public interface IAttacker //TODO remove
    {
        int ID { get; }
    }
}
