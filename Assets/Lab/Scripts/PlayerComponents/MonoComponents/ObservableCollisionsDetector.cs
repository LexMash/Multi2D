using R3.Triggers;
using R3;
using UnityEngine;
using System;

namespace Multi2D
{
    [DisallowMultipleComponent]
    public class ObservableCollisionsDetector : ObservableTriggerBase
    {
        [field: SerializeField] public Collider2D Collider { get; private set; }

        private readonly Subject<Collider2D> onTriggerEnter2D = new Subject<Collider2D>();
        private readonly Subject<Collision2D> onCollisionEnter2D = new Subject<Collision2D>();
        private readonly Subject<Collision2D> onCollisionExit2D = new Subject<Collision2D>();
     
        public Observable<Collider2D> OnTriggerEnter2DAsObservable() => onTriggerEnter2D;
        public Observable<Collision2D> OnCollisionEnter2DAsObservable() => onCollisionEnter2D;
        public Observable<Collision2D> OnCollisionExit2DAsObservable() => onCollisionExit2D;

        protected override void RaiseOnCompletedOnDestroy()
        {
            onTriggerEnter2D.OnCompleted();
            onCollisionEnter2D.OnCompleted();
            onCollisionExit2D.OnCompleted();
        }

        private void OnTriggerEnter2D(Collider2D other) => onTriggerEnter2D.OnNext(other);
        private void OnCollisionEnter2D(Collision2D coll) => onCollisionEnter2D.OnNext(coll);
        private void OnCollisionExit2D(Collision2D coll) => onCollisionExit2D.OnNext(coll);
        private void Reset() => Collider = GetComponent<Collider2D>();

#if UNITY_EDITOR
        private Action drawGizmo;
        public void AddGizmoFunction(Action gizmoFunc) => drawGizmo += gizmoFunc;
        public void RemoveGizmoFunction(Action gizmoFunc) => drawGizmo -= gizmoFunc;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            drawGizmo?.Invoke();
        }
#endif
    }
}