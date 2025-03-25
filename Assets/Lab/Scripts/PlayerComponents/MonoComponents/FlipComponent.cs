using Multi2D.Data;
using UnityEngine;

namespace Multi2D
{
    public class FlipComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private PlayerModel model;
        private int currentLookDirection;

        public void Initialize(PlayerModel model)
        {
            this.model = model;
            currentLookDirection = model.LookDirection.CurrentValue;
            target.localScale = new Vector3(currentLookDirection, 1f, 1f);
        }

        public void UpdateDirection(Vector2 direction)
        {
            var x = direction.x;

            if (x == 0)
                return;

            if (Mathf.Abs(x) <= 0.3f) //TODO get it into config treshold
                return;

            int newLookDirection = (int)Mathf.Sign(x);
            if (newLookDirection == currentLookDirection)
                return;

            currentLookDirection = newLookDirection;
            target.localScale = new Vector3(currentLookDirection, 1f, 1f);
            model.SetLookDirection(currentLookDirection);
        }
    }
}
