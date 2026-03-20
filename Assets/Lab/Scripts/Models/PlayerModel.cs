using Multi2D.Lab.Scripts.Data;
using R3;
using UnityEngine;

namespace Multi2D.Data
{
    public class PlayerModel : IReadOnlyPlayerModel
    {
        private readonly ReactiveProperty<Vector2> position;
        private readonly ReactiveProperty<Vector2> velocity;
        private readonly ReactiveProperty<int> lookDirection;
        private readonly ReactiveProperty<PlayerStateType> state;
        private readonly ReactiveProperty<bool> isAttack;

        public int Id { get; private set; }
        public ReadOnlyReactiveProperty<Vector2> Position => position;
        public ReadOnlyReactiveProperty<Vector2> Velocity => velocity;
        public ReadOnlyReactiveProperty<int> LookDirection => lookDirection;
        public ReadOnlyReactiveProperty<PlayerStateType> State => state;
        public ReadOnlyReactiveProperty<bool> IsAttack => isAttack;

        public HitData LastHitData;

        public PlayerModel(int id, Vector2 initialPosition, int initialLookDirection)
        {
            Id = id;

            position = new(initialPosition);
            velocity = new(Vector2.zero);
            lookDirection = new(initialLookDirection);
            state = new(PlayerStateType.None);
            isAttack = new(false);         
        }

        public void SetState(PlayerStateType newState) => state.Value = newState;
        public void SetLookDirection(int newDirection) => lookDirection.Value = newDirection;
        public void SetPosition(Vector2 newPosition) => position.Value = newPosition;
        public void SetVelocity(Vector2 newVelocity) => velocity.Value = newVelocity;
        public void SetAttackState(bool isAttack) => this.isAttack.Value = isAttack;
    }
}
