using Multi2D.Data;
using Multi2D.Data.Collisions;
using Multi2D.Extensions;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multi2D.PlayerComponents
{
    public class CollisionDataHandler// : IDisposable
    {
        private readonly CollisionDetector detector;
        private readonly ReactiveProperty<CollisionStruct> collisionData;
        //private readonly Dictionary<int, CollisionLayerType> collisionLayersMap = new();
        public Observable<CollisionStruct> OnCollisionUpdated => collisionData;

        //public CollisionDataHandler(CollisionDetector detector, ICollisionsConfig config)
        //{
        //    this.detector = detector;
        //    collisionData = new();

        //    foreach (CollisionLayerMask layer in config.CollisionLayers)
        //        collisionLayersMap.Add(layer.LayerMask.value, layer.DetectionMask);

        //    detector.OnCollision += OnCollision;
        //    detector.OnTrigger += OnTrigger;
        //}

        //public void Dispose()
        //{
        //    detector.OnCollision -= OnCollision;
        //    detector.OnTrigger -= OnTrigger;

        //    collisionLayersMap.Clear();
        //}

        //private void OnTrigger(Collider2D collider, CollisionDirectionType direction) => collisionData.Value = new()
        //{
        //    Collider = collider,
        //    Direction = direction,
        //    Collision = null,
        //    Layer = GetCollisionLayerType(collider.gameObject)
        //};

        //private void OnCollision(Collision2D collision, CollisionDirectionType direction) => collisionData.Value = new()
        //{
        //    Collision = collision,
        //    Direction = direction,
        //    Collider = collision.collider,
        //    Layer = GetCollisionLayerType(collision.gameObject)
        //};

        //private CollisionLayerType GetCollisionLayerType(GameObject gameObject)
        //{
        //    collisionLayersMap.TryGetValue(gameObject.ConvertGoLayerIndexToLayerMaskValue(), out CollisionLayerType layerType);
        //    return layerType;
        //}
    }
}
