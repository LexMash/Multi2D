using TMPro;
using UnityEngine;

namespace Multi2D.Lab.Scripts.UI
{
    public class CoinPanel : MonoBehaviour
    {
        private static readonly string FORMAT = "{0}";

        [SerializeField] private TMP_Text panel;

        public void UpdateValue(int value)
        {
            panel.SetText(FORMAT, value);
        }
    }
}
