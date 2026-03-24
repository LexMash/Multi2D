using UnityEngine;

namespace Multi2D
{
    public class CellForElement : MonoBehaviour
    {
        public void SetElement(Transform element)
        {
            transform.SetParent(element);
            element.position = Vector3.zero;
            element.localScale = Vector3.one;
        }
    }
}
