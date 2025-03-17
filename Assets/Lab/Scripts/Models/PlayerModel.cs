using R3;
using UnityEngine;

namespace Multi2D.Data
{
    public class PlayerModel : IReadOnlyPlayerModel
    {
        private readonly ReactiveProperty<Vector2> position;
        private readonly ReactiveProperty<LookDirection> lookDirection;
        private readonly ReactiveProperty<int> coins;
        private readonly ReactiveProperty<PlayerStateType> state;

        public int Id { get; private set; }
        public ReadOnlyReactiveProperty<Vector2> Position => position;
        public ReadOnlyReactiveProperty<LookDirection> LookDirection => lookDirection;
        public ReadOnlyReactiveProperty<int> Coins => coins;
        public ReadOnlyReactiveProperty<PlayerStateType> State => state;        
        
        public PlayerModel(int id, Vector2 initialPosition, LookDirection initialLookDirection)
        {
            position = new(initialPosition);
            lookDirection = new(initialLookDirection);
            coins = new(0);
            state = new(PlayerStateType.Idle);
            Id = id;
        }

        public void AddCoins(int amount) => coins.Value += amount;

        public void TryRemoveCoins(int amount)
        {
            int result = coins.CurrentValue - amount;
            coins.Value = result < 0 ? 0 : result;
        }

        public void SetState(PlayerStateType newState) => state.Value = newState;
        public void SetLookDirection(LookDirection newDirection) => lookDirection.Value = newDirection;
        public void SetPosition(Vector2 newPosition) => position.Value = newPosition;
    }
}
