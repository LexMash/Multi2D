using System;

namespace Multi2D
{
    [Serializable]
    public struct PlayerStateStruct
    {
        public int Coins;
        public SimpleVector2Struct Position;
        public PlayerStateType State;
        public int LookDirection;        
    }
}
