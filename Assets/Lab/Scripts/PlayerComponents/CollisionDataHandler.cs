using Multi2D.Data;
using Multi2D.Data.Collisions;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multi2D.PlayerComponents
{
    public class CollisionDataHandler : IDisposable
    {
        private readonly CollisionDetector detector;
        private readonly ReactiveProperty<CollisionStruct> collisionData;
        private readonly Dictionary<LayerMask, CollisionLayerType> collisionLayersMap = new();
        public Observable<CollisionStruct> OnCollisionUpdated => collisionData;

        public CollisionDataHandler(CollisionDetector detector, ICollisionsConfig config)
        {
            this.detector = detector;
            collisionData = new();
            foreach (var layer in config.CollisionLayers)
                collisionLayersMap.Add(layer.Mask, layer.Type);

            detector.OnCollision += OnCollision;
            detector.OnTrigger += OnTrigger;
        }

        public void Dispose()
        {
            detector.OnCollision -= OnCollision;
            detector.OnTrigger -= OnTrigger;

            collisionLayersMap.Clear();
        }

        private void OnTrigger(Collider2D collider, CollisionDirectionType direction)
        {
            CollisionStruct collisionStruct = new() { Collider = collider, Direction = direction };

            collisionData.Value = collisionStruct;
        }

        private void OnCollision(Collision2D collision, CollisionDirectionType direction)
        {
            CollisionStruct collisionStruct = new() { Collision = collision, Direction = direction };

            collisionData.Value = collisionStruct;
        }
    }
}
