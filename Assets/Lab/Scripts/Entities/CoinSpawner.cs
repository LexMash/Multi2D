using Unity.Netcode;
using UnityEngine;

namespace Multi2D.Assets.Lab.Scripts.Entities
{
    public class CoinSpawner : NetworkBehaviour
    {
        [SerializeField] private CoinView prefab;

        public static CoinSpawner instance;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
                return;

            if(instance != null)
            {
                GetComponent<NetworkObject>().Despawn();
                return;
            }

            instance = this;
        }

        public void SpawnAtPosition(Vector2 position)
        {
            CoinView instance = Instantiate(prefab, position, Quaternion.identity);
            instance.Init();
        }

        public void SpawnAtPositionAndThrow(Vector2 position, Vector2 kickVelocity)
        {
            var instance = Instantiate(prefab, position, Quaternion.identity);
            instance.Init();
            instance.Kick(kickVelocity);
        }
    }
}
