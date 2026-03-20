using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Multi2D
{
    public class NetPanel : MonoBehaviour
    {
        public Button Host;
        public Button Client;

        private void Start()
        {
            Host.onClick.AddListener(StartHost);
            Client.onClick.AddListener(StartClient);
        }

        private void StartClient()
        {
            NetworkManager.Singleton.StartClient();
        }

        private void StartHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        private void OnDestroy()
        {
            Host.onClick.RemoveListener(StartHost);
            Client.onClick.RemoveListener(StartClient);
        }
    }
}
