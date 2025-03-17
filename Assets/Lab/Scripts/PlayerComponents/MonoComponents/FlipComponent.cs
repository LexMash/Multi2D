using UnityEngine;

namespace Multi2D
{
    public class FlipComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;

        public void SetDirection(LookDirection lookDirection) 
            => target.localScale = lookDirection == LookDirection.Right ? Vector3.right : Vector3.left;
    }
}
