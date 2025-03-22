using R3;
using UnityEngine;

namespace Multi2D.Data
{
    public class PlayerModel : IReadOnlyPlayerModel
    {
        private readonly ReactiveProperty<Vector2> position;
        private readonly ReactiveProperty<Vector2> velocity;
        private readonly ReactiveProperty<LookDirection> lookDirection;
        private readonly ReactiveProperty<int> coins;
        private readonly ReactiveProperty<PlayerStateType> state;

        public int Id { get; private set; }
        public ReadOnlyReactiveProperty<Vector2> Position => position;
        public ReadOnlyReactiveProperty<Vector2> Velocity => velocity;
        public ReadOnlyReactiveProperty<LookDirection> LookDirection => lookDirection;
        public ReadOnlyReactiveProperty<int> Coins => coins;
        public ReadOnlyReactiveProperty<PlayerStateType> State => state;

        public PlayerModel(int id, Vector2 initialPosition, LookDirection initialLookDirection)
        {
            position = new(initialPosition);
            velocity = new(Vector2.zero);
            lookDirection = new(initialLookDirection);
            coins = new(0);
            state = new(PlayerStateType.None);
            Id = id;
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
        public void SetLookDirection(LookDirection newDirection) => lookDirection.Value = newDirection;
        public void SetPosition(Vector2 newPosition) => position.Value = newPosition;
        public void SetVelocity(Vector2 newVelocity) => velocity.Value = newVelocity;
    }
}
