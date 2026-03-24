using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Multi2D
{
    public class InputDeviceDefinitionView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image defineImage;
        [SerializeField] private TMP_Text nameLabel;

        public void Setup(Sprite icon, string name)
        {
            image.sprite = icon;
            nameLabel.SetText(name);
        }

        public void Define(bool isDefined) => defineImage.enabled = isDefined;
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
