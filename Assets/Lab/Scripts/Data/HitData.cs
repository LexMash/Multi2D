using Unity.Netcode;
using UnityEngine;

namespace Multi2D.Lab.Scripts.Data
{
    public struct HitData : INetworkSerializable
    {
        public Vector2 HitPosition;

        public HitData(Vector2 hitPosition)
        {
            HitPosition = hitPosition;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref HitPosition);
        }
    }
}
