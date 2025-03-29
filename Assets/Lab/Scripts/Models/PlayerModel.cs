using R3;
using UnityEngine;

namespace Multi2D.Data
{
    public class PlayerModel : IReadOnlyPlayerModel
    {
        private readonly ReactiveProperty<Vector2> position;
        private readonly ReactiveProperty<Vector2> velocity;
        private readonly ReactiveProperty<int> lookDirection;
        private readonly ReactiveProperty<int> coins;
        private readonly ReactiveProperty<PlayerStateType> state;
        private readonly ReactiveProperty<bool> isAttack;

        public int Id { get; private set; }
        public ReadOnlyReactiveProperty<Vector2> Position => position;
        public ReadOnlyReactiveProperty<Vector2> Velocity => velocity;
        public ReadOnlyReactiveProperty<int> LookDirection => lookDirection;
        public ReadOnlyReactiveProperty<int> Coins => coins;
        public ReadOnlyReactiveProperty<PlayerStateType> State => state;
        public ReadOnlyReactiveProperty<bool> IsAttack => isAttack;

        public PlayerModel(int id, Vector2 initialPosition, int initialLookDirection)
        {
            Id = id;

            position = new(initialPosition);
            velocity = new(Vector2.zero);
            lookDirection = new(initialLookDirection);
            coins = new(0);
            state = new(PlayerStateType.None);
            isAttack = new(false);         
        }

        public void AddCoins(int amount) => coins.Value += amount;

        public bool TryRemoveCoins(int amount, out int removed)
        {
            removed = 0;
            var currentValue = coins.CurrentValue;

            if (currentValue == 0) 
                return false;

            removed = currentValue - amount;
            removed = removed < 0 ? 0 : removed;
            coins.Value = removed;

            return true;
        }

        public void SetState(PlayerStateType newState) => state.Value = newState;
        public void SetLookDirection(int newDirection) => lookDirection.Value = newDirection;
        public void SetPosition(Vector2 newPosition) => position.Value = newPosition;
        public void SetVelocity(Vector2 newVelocity) => velocity.Value = newVelocity;
        public void SetAttackState(bool isAttack) => this.isAttack.Value = isAttack;
    }
}
