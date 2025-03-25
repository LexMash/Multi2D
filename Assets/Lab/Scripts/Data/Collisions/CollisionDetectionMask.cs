using System;

namespace Multi2D.Data
{
    [Flags]
    public enum CollisionDetectionMask : int
    {
        Ground = 1 << 0,
        Top = 1 << 1,
        Right = 1 << 2,
        Left = 1 << 3,
        LadderUp = 1 << 4,
        LadderDown = 1 << 5,       
        Forward = 1 << 6,
        None = 1 << 7,
    }
}
