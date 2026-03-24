using UnityEngine;
using UnityEngine.UI;

namespace Multi2D
{
    public class RowOfCells : MonoBehaviour
    {
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color warningColor;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
