using Multi2D.Assets.Lab.Scripts.Entities;
using Multi2D.Data;
using Multi2D.Data.Collisions;
using Multi2D.Extensions;
using R3;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Multi2D
{
    public class CollisionDetector : IDisposable
    {
        private readonly Dictionary<int, CollisionDetectionMask> topDetectionLayerMap = new();
        private readonly Dictionary<int, CollisionDetectionMask> bottomDetectionLayerMap = new();
        private readonly CompositeDisposable subscribtions = new CompositeDisposable();
        private readonly ObservableCollisionsDetector monoDetector;
        private readonly CollisionDetectionConfig config;
        private readonly Collider2D collider;

        private readonly Vector2 overlapBoxSize;
        private readonly Vector3 overlapOffset;
        private readonly LayerMask overlapMask;

        private CollisionDetectionMask detectedMask;

        public event Action<Collision2D> OnBulletCollision = delegate { };
        public event Action<CoinView> OnCoinsCollision = delegate { };

        public CollisionDetectionMask DetectedMask => detectedMask;

        public CollisionDetector(ObservableCollisionsDetector monoDetector, CollisionDetectionConfig config)
        {
            this.monoDetector = monoDetector;
            this.config = config;
            this.collider = monoDetector.Collider;
            this.overlapBoxSize = config.OverlapBoxSize;
            this.overlapOffset = config.OverlapOffset;
            this.overlapMask = config.OverlapMask;
        }

        public void Initialize()
        {
            foreach (CollisionLayerMask clm in config.TopCollisionsLayers)
                topDetectionLayerMap[clm.LayerMask.value] = clm.DetectionMask;

            foreach (CollisionLayerMask clm in config.BottomCollisionsLayers)
                bottomDetectionLayerMap[clm.LayerMask.value] = clm.DetectionMask;

            collider.contactCaptureLayers = config.ColliderContactCaptureLayers;
            collider.callbackLayers = config.ColliderContactCaptureLayers;

            subscribtions.Add(monoDetector.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter));
            subscribtions.Add(monoDetector.OnCollisionEnter2DAsObservable().Subscribe(OnCollisionEnter));
        }

        public void ExcludeLayers(LayerMask layerMask) => collider.excludeLayers = layerMask;
        public void IncludeLayers(LayerMask layerMask) => collider.includeLayers = layerMask;

        public void UpdateCollisionData()
        {
            ResetMask();
            CheckTopCollisions(OverlapBoxAllAndReturnColliders(GetTopOverlapBoxOrigin), topDetectionLayerMap);
            CheckTopCollisions(OverlapBoxAllAndReturnColliders(GetBottomOverlapBoxOrigin), bottomDetectionLayerMap);
            //Debug.Log($"Collision data update {detectedMask}");
        }

        public void Dispose()
        {
            bottomDetectionLayerMap.Clear();
            topDetectionLayerMap.Clear();
            subscribtions.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetMask() => detectedMask = CollisionDetectionMask.None;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Collider2D[] OverlapBoxAllAndReturnColliders(Func<Vector3> getOverlapBox) 
            => Physics2D.OverlapBoxAll(getOverlapBox(), overlapBoxSize, 0f, overlapMask);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckTopCollisions(Collider2D[] colliders, Dictionary<int, CollisionDetectionMask> map)
        {
            var count = colliders.Length;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (detectedMask.HasFlag(CollisionDetectionMask.None))
                        detectedMask ^= CollisionDetectionMask.None;

                    GameObject go = colliders[i].gameObject;
                    int gameObjectLayer = go.ConvertGoLayerIndexToLayerMaskValue();
                    CollisionDetectionMask detected = map[gameObjectLayer];
                    detectedMask |= detected;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector3 GetTopOverlapBoxOrigin()
        {
            var bounds = collider.bounds.max;
            bounds += overlapOffset;
            return bounds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector3 GetBottomOverlapBoxOrigin()
        {
            var bounds = collider.bounds.min;
            bounds -= overlapOffset;
            return bounds;
        }

        private void OnTriggerEnter(Collider2D other)
        {
            OnCoinsCollision.Invoke(other.GetComponent<CoinView>());
            Debug.Log($"Coin collected");
        }

        private void OnCollisionEnter(Collision2D collision)
        {
            OnBulletCollision.Invoke(collision);
            Debug.Log($"Bullet hits");
        }
    }
}
