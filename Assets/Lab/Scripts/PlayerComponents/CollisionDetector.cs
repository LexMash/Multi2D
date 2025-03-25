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
        private readonly PlayerModel model;
        private readonly Collider2D playerCollider;
        private readonly LayerMask overlapMask;

        private CollisionDetectionMask detectedMask;
        private int directionMultiplier;

        public event Action<Collision2D> OnBulletCollision = delegate { };
        public event Action<CoinView> OnCoinsCollision = delegate { };
        public CollisionDetectionMask DetectedMask => detectedMask;

        public CollisionDetector(
            ObservableCollisionsDetector monoDetector, 
            CollisionDetectionConfig config,
            PlayerModel model)
        {
            this.monoDetector = monoDetector;
            this.config = config;
            this.model = model;
            playerCollider = monoDetector.Collider;
            overlapMask = config.OverlapMask;
        }

        public void Initialize()
        {
            foreach (CollisionLayerMask clm in config.TopCollisionsLayers)
                topDetectionLayerMap[clm.LayerMask.value] = clm.DetectionMask;

            foreach (CollisionLayerMask clm in config.BottomCollisionsLayers)
                bottomDetectionLayerMap[clm.LayerMask.value] = clm.DetectionMask;

            playerCollider.contactCaptureLayers = config.ColliderContactCaptureLayers;
            playerCollider.callbackLayers = config.ColliderContactCaptureLayers;

            subscribtions.Add(monoDetector.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter));
            subscribtions.Add(monoDetector.OnCollisionEnter2DAsObservable().Subscribe(OnCollisionEnter));
            subscribtions.Add(model.LookDirection.Subscribe((value) => directionMultiplier = value));

#if UNITY_EDITOR
            monoDetector.AddGizmoFunction(() => Gizmos.DrawWireCube(GetTopOverlapBoxOrigin(), config.TopOverlapBoxSize));
            monoDetector.AddGizmoFunction(() => Gizmos.DrawWireCube(GetForwardOverlapBoxOrigin(), config.ForwardOverlapBoxSize));
            monoDetector.AddGizmoFunction(() => Gizmos.DrawWireCube(GetBottomOverlapBoxOrigin(), config.BottomOverlapBoxSize));
#endif
        }       

        public void ExcludeLayers(LayerMask layerMask) => playerCollider.excludeLayers ^= layerMask;
        public void IncludeLayers(LayerMask layerMask) => playerCollider.includeLayers |= layerMask;

        public void UpdateCollisionData()
        {
            ResetMask();
            CheckCollisions(OverlapBoxAllAndReturnColliders(GetTopOverlapBoxOrigin, config.TopOverlapBoxSize), topDetectionLayerMap);
            CheckForwardCollisions(OverlapBoxAllAndReturnColliders(GetForwardOverlapBoxOrigin, config.ForwardOverlapBoxSize));
            CheckBottomCollisions(OverlapBoxAllAndReturnColliders(GetBottomOverlapBoxOrigin, config.BottomOverlapBoxSize), bottomDetectionLayerMap);
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
        private Collider2D[] OverlapBoxAllAndReturnColliders(Func<Vector3> getOverlapBox, Vector2 overlapBoxSize) 
            => Physics2D.OverlapBoxAll(getOverlapBox(), overlapBoxSize, 0f, overlapMask);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckCollisions(Collider2D[] colliders, Dictionary<int, CollisionDetectionMask> map)
        {
            var count = colliders.Length;

            if (count > 0)
            {
                if (detectedMask.HasFlag(CollisionDetectionMask.None))
                    detectedMask ^= CollisionDetectionMask.None;

                for (int i = 0; i < count; i++)
                {
                    Collider2D collider = colliders[i];
                    GameObject go = collider.gameObject;
                    int gameObjectLayer = go.ConvertGoLayerIndexToLayerMaskValue();
                    CollisionDetectionMask detected = map[gameObjectLayer];
                    detectedMask |= detected;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckBottomCollisions(Collider2D[] colliders, Dictionary<int, CollisionDetectionMask> map)
        {
            var count = colliders.Length;

            if (count > 0)
            {
                if (detectedMask.HasFlag(CollisionDetectionMask.None))
                    detectedMask ^= CollisionDetectionMask.None;

                for (int i = 0; i < count; i++)
                {
                    Collider2D collider = colliders[i];
                    GameObject go = colliders[i].gameObject;
                    int gameObjectLayer = go.ConvertGoLayerIndexToLayerMaskValue();
                    CollisionDetectionMask detected = map[gameObjectLayer];

                    if (detected == CollisionDetectionMask.Ground)
                    {
                        Transform playerTransform = playerCollider.transform.parent; //TODO REFACTOR this                       
                        playerTransform.position = new Vector3(playerTransform.position.x, collider.bounds.max.y, playerTransform.position.z);
                    }

                    detectedMask |= detected;
                }
            }
        }

        private void CheckForwardCollisions(Collider2D[] colliders)
        {
            var count = colliders.Length;
            if (count > 0)
            {
                if (detectedMask.HasFlag(CollisionDetectionMask.None))
                    detectedMask ^= CollisionDetectionMask.None;

                for (int i = 0; i < count; i++)
                {
                    GameObject go = colliders[i].gameObject;
                    if(go.IsSameLayer(config.ForwardMask))
                        detectedMask |= CollisionDetectionMask.Forward;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector3 GetTopOverlapBoxOrigin()
        {
            var bounds = playerCollider.bounds.max;
            bounds += config.TopOverlapOffset;
            return bounds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector3 GetBottomOverlapBoxOrigin()
        {
            var bounds = playerCollider.bounds.min;
            bounds -= config.BottomOverlapOffset;
            return bounds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector3 GetForwardOverlapBoxOrigin()
        {
            var bounds = playerCollider.bounds.center;
            bounds += config.ForwardOverlapOffset * directionMultiplier;
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
