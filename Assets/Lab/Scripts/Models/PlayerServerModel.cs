using R3;

namespace Multi2D.Data
{
    public class PlayerServerModel
    {
        private readonly ReactiveProperty<int> coins = new();
        public ReadOnlyReactiveProperty<int> Coins => coins;

        public void AddCoins(int amount) => coins.Value += amount;

        public bool TryRemoveCoins(int amount, out int removed)
        {
            removed = amount;
            int currentValue = coins.CurrentValue;

            if (currentValue == 0)
                return false;

            if (currentValue < amount)
            {
                removed = currentValue;
                coins.Value = 0;
            }
            else
            {
                coins.Value = currentValue - amount;
            }

            return true;
        }
    }
}
