using Unity.Netcode;
using UnityEngine;

namespace Multi2D.Lab.Scripts
{
    public class TimeCounter : NetworkBehaviour
    {
        public NetworkVariable<float> Timer = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private float counter;
        private float nextTickTime;
        private bool isStarted;

        public void Init(int time)
        {
            counter = time;
            nextTickTime = counter;
        }

        public void StartCount()
        {
            isStarted = true;
        }

        public void Stop()
        {
            isStarted = false;
        }

        private void Update()
        {
            if (IsServer && isStarted)
            {
                counter -= Time.deltaTime;

                if (counter <= nextTickTime)
                {
                    float timeToReport = Mathf.Max(0, counter);

                    Timer.Value = timeToReport;

                    nextTickTime = Mathf.Floor(counter);
                }

                if (counter <= 0)
                {
                    isStarted = false;
                }
            }
        }
    }
}
