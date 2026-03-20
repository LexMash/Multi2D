using TMPro;
using UnityEngine;

namespace Multi2D.Lab.Scripts.UI
{
    public class TimeCounterPanel : MonoBehaviour
    {
        private static readonly string FORMAT = "{0:00}:{1:00}";

        [SerializeField] private TMP_Text panel;

        public void UpdateValue(float value)
        {
            int minutes = Mathf.FloorToInt(value / 60);
            int seconds = Mathf.FloorToInt(value % 60);

            panel.SetText(FORMAT, minutes, seconds);
        }
    }
}
