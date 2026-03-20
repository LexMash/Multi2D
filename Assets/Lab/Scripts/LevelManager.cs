using Multi2D.Assets.Lab.Scripts.Entities;
using Unity.Netcode;
using UnityEngine;

namespace Multi2D.Assets.Lab.Scripts
{
    public class LevelManager : NetworkBehaviour
    {
        [SerializeField] private Transform[] transforms;
        [SerializeField] private CoinSpawner coinSpawner;

        protected override void OnNetworkPostSpawn() 
        { 
            if (!IsServer)
                return;

            foreach (var transform in transforms)
                coinSpawner.SpawnAtPosition(transform.position);
        }
    }
}
