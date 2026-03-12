using R3;
using UnityEngine;

namespace Multi2D.Data
{
    public interface IReadOnlyPlayerModel
    {
        int Id { get; }
        ReadOnlyReactiveProperty<int> Coins { get; }
        ReadOnlyReactiveProperty<int> LookDirection { get; }
        ReadOnlyReactiveProperty<Vector2> Position { get; }
        ReadOnlyReactiveProperty<PlayerStateType> State { get; }
        ReadOnlyReactiveProperty<bool> IsAttack { get; }
    }
}