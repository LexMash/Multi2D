using Multi2D.Assets.Lab.Scripts.PlayerComponents;
using Multi2D.Data;
using Multi2D.Extensions;
using Multi2D.FSM;
using Multi2D.Lab.Scripts.Data;
using Multi2D.States;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Multi2D
{
    [SelectionBase]
    public class PlayerView : NetworkBehaviour, IHitable
    {
        [field: SerializeField] public Transform BulletSpawnRoot { get; private set; }
        [field: SerializeField] public PlayerAnimationController AnimationController { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
        [field: SerializeField] public FlipComponent FlipComponent { get; private set; }
        [field: SerializeField] public ObservableCollisionsDetector ObservableCollisionsDetector { get; private set; }

        [SerializeField] private Rigidbody2D rb;

        [SerializeField] private AttackController attackController;

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
        private PlayerHurtState hurtState;

        private PlayerModel model;
        private float dt;

        private readonly NetworkVariable<int> collisionSync = 
            new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public CollisionDetectionMask detectedMask;

        public event Action<ulong, Vector2> Hited;
        public event Action<ulong> Collected;

        public ulong ID { get; private set; }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                collisionSync.OnValueChanged += OnCollisionChangedRpc;
            }          

            attackController.Init(AnimationController, PlayerConfig);

            if (!IsOwner)
                return;

            ID = NetworkBehaviourId;
            attackController.SetID(NetworkBehaviourId);
            collisionSync.Value = ObservableCollisionsDetector.Collider.excludeLayers;

            rb = GetComponent<Rigidbody2D>(); //TODO remove GetComp
            model = new(1, Vector2.zero, 1);
            collisionDetector = new(ObservableCollisionsDetector, PlayerConfig.CollisionDetectionConfig, model, rb, collisionSync);
            collisionDetector.Initialize();
            collisionDetector.OnCoinsCollision += OnCoinCollisionRpc;

            VectorsExtensions.AxisInputTreshold = PlayerConfig.AxisInputTreshold;

            inputReader = new LocalInputReader(new LocalMultiplayerInput());
            inputReader.Enable();

            stateChangeRequester = new();
            playerStateMachine = new(stateChangeRequester);

            idleState = new PlayerIdleState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);
            moveState = new PlayerMoveState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector, attackController);
            jumpState = new PlayerJumpState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);
            fallState = new PlayerFallState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);
            hurtState = new PlayerHurtState(inputReader, stateChangeRequester, AnimationController, model, PlayerConfig, collisionDetector);

            playerStateMachine
                .RegisterState(idleState, true)
                .RegisterState(moveState)
                .RegisterState(jumpState)
                .RegisterState(fallState)
                .RegisterState(hurtState);

            collisionHandler = new(model, collisionDetector, inputReader);
            playerStateMachine.Initialize();
            FlipComponent.Initialize(model);

            dt = Time.fixedDeltaTime / steps;
        }

        public void SetID(ulong id)
        {
            ID = id;
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
                collisionSync.OnValueChanged -= OnCollisionChangedRpc;

            if (IsOwner)
                collisionDetector.OnCoinsCollision -= OnCoinCollisionRpc;

            base.OnNetworkDespawn();
        }

        [Rpc(SendTo.Owner)]
        void IHitable.TakeHitRpc(HitData hit)
        {
            if (model.State.CurrentValue != PlayerStateType.TakeHit)
            {
                Debug.Log($"HIT {NetworkBehaviourId}");
                model.LastHitData = hit;
                model.SetState(PlayerStateType.TakeHit);
                HitCallbackRpc();
            }
        }

        [Rpc(SendTo.Server)]
        private void HitCallbackRpc()
        {
            Hited?.Invoke(ID, transform.position);
        }

        [Rpc(SendTo.Server)]
        private void OnCollisionChangedRpc(int previousValue, int newValue)
        {
            ObservableCollisionsDetector.Collider.excludeLayers = newValue;
        }

        [Rpc(SendTo.Server)]
        private void OnCoinCollisionRpc() => Collected?.Invoke(ID);

        private void FixedUpdate()
        {
            if (!IsOwner)
                return;

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
                    attackController.Attack();

                model.SetPosition(MoveComponent.CurrentPosition);
            }
        }
    }
}
