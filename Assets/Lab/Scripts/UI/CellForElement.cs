using UnityEngine;

namespace Multi2D
{
    public class CellForElement : MonoBehaviour
    {
        public void SetElement(Transform element)
        {
            element.SetParent(transform);
            element.localPosition = Vector3.zero;
            element.localScale = Vector3.one;
        }
    }
}
