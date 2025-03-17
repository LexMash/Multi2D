using UnityEngine;

namespace Multi2D
{
    [SerializeField]
    public struct PlayerStateStruct
    {
        public int Coins;
        public SimpleVector2Struct Position;
        public PlayerStateType State;
        public LookDirection LookDirection;        
    }
}
