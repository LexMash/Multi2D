using UnityEngine;

namespace Multi2D
{
    [SerializeField]
    public struct PlayerStateStruct
    {
        public SimpleVector2Struct Velocity;
        public SimpleVector2Struct Position;
        public PlayerStateBase State;
        public LookDirection LookDirection;        
    }
}
