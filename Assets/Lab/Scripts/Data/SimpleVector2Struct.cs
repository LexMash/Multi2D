using System;

namespace Multi2D
{
    [Serializable]
    public struct SimpleVector2Struct
    {
        public float X;
        public float Y;

        public SimpleVector2Struct(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
