using Multi2D.Assets.Lab.Scripts.Entities;
using Multi2D.Data;
using Multi2D.Lab.Scripts.UI;
using R3;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Multi2D.Lab.Scripts.NetworkManagement
{
    public class NetGameManager : NetworkBehaviour
    {
        [SerializeField] private int playersAmount = 2;
        [SerializeField] private PlayerView prefab;
        [SerializeField] private Transform[] positions;

        [Space]
        [SerializeField] private TimeCounter counter;
        [SerializeField] private CoinPanel[] coinPanels;
        [SerializeField] private TimeCounter timeCounter;
        [SerializeField] private TimeCounterPanel timePanel;

        [Space]
        [SerializeField] private CoinSpawner spawner;

        private NetworkVariable<int> coins1 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private NetworkVariable<int> coins2 = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private int playersCount = 0;
        private Dictionary<ulong, PlayerServerModel> models = new();
        private Dictionary<ulong, PlayerView> views = new();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                coins1.OnValueChanged += UpdateCoins1Rpc;
                coins2.OnValueChanged += UpdateCoins2Rpc;
                timeCounter.Timer.OnValueChanged += OnTimeChangedRpc;
            }

            if (!IsServer) return;

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                coins1.OnValueChanged -= UpdateCoins1Rpc;
                coins2.OnValueChanged -= UpdateCoins2Rpc;
                timeCounter.Timer.OnValueChanged -= OnTimeChangedRpc;
            }

            if (!IsServer) return;

            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        private void OnClientConnected(ulong id)
        {
            Debug.Log($"PLAYER CONNECTED {id}");

            if (playersCount + 1 > playersAmount)
                return;

            playersCount++;

            Debug.Log($"PLAYERS COUNT {playersCount}");

            var instance = Instantiate(prefab, positions[playersCount - 1]);
            instance
                .GetComponent<NetworkObject>()
                .SpawnWithOwnership(id);

            //instance.SetID(id);
            views[instance.ID] = instance;

            var model = new PlayerServerModel();
            models[instance.ID] = model;
            instance.Collected += OnCollected;
            instance.Hited += OnHitRpc;

            if (playersCount == 1)
                model.Coins.Subscribe(value => coins1.Value = value);

            if (playersCount == 2)
                model.Coins.Subscribe(value => coins2.Value = value);

            if (playersCount != playersAmount)
               return;

            coins1.Value = 0;
            coins2.Value = 0;

            timeCounter.Init(200);
            timeCounter.StartCount();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void OnTimeChangedRpc(float previousValue, float newValue) => timePanel.UpdateValue(newValue);
        [Rpc(SendTo.ClientsAndHost)]
        private void UpdateCoins1Rpc(int previousValue, int newValue) => coinPanels[0].UpdateValue(newValue);
        [Rpc(SendTo.ClientsAndHost)]
        private void UpdateCoins2Rpc(int previousValue, int newValue) => coinPanels[1].UpdateValue(newValue);

        [Rpc(SendTo.Server)]
        private void OnHitRpc(ulong id, Vector2 vector)
        {
            if (models[id].TryRemoveCoins(3, out var removed))
            {
                const float fanAngle = 60;
                float startAngle = -fanAngle / 2f;
                float baseSpeed = 10;

                for (int i = 0; i < removed; i++)
                {
                    float angle = startAngle + fanAngle / (removed - 1.1f) * i;

                    float angleRad = angle * Mathf.Deg2Rad;
                    Vector2 direction = new(Mathf.Sin(angleRad), Mathf.Cos(angleRad));

                    spawner.SpawnAtPositionAndThrow(vector, direction * baseSpeed);
                }
            }
        }

        private void OnClientDisconnected(ulong id)
        {
            Debug.Log($"Disconnected {id}");
            var instance = views[id];
            Debug.Log($"Disconnected instance id {instance.ID}");
            views.Remove(id);

            instance.Collected -= OnCollected;
            instance.Hited -= OnHitRpc;
            instance
                .GetComponent<NetworkObject>()
                .Despawn();
        }

        private void OnCollected(ulong id) => models[id].AddCoins(1);
    }
}
