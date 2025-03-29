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
        private readonly Rigidbody2D playerRb;
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
            PlayerModel model,
            Rigidbody2D playerRb)
        {
            this.monoDetector = monoDetector;
            this.config = config;
            this.model = model;
            this.playerRb = playerRb;
            playerCollider = monoDetector.Collider;
            overlapMask = config.OverlapMask;

            Physics2D.queriesHitTriggers = false;
            Physics2D.queriesStartInColliders = false;
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
            monoDetector.AddGizmoFunction(() => Gizmos.DrawWireCube(GetOverlapBoxOrigin(config.TopOverlapOffset), config.TopOverlapBoxSize));
            monoDetector.AddGizmoFunction(() => Gizmos.DrawWireCube(GetOverlapBoxOrigin(config.ForwardOverlapOffset), config.ForwardOverlapBoxSize));
            monoDetector.AddGizmoFunction(() => Gizmos.DrawWireCube(GetOverlapBoxOrigin(config.BottomOverlapOffset), config.BottomOverlapBoxSize));
#endif
        }       

        public void ExcludeLayers(LayerMask layerMask) => playerCollider.excludeLayers ^= layerMask;
        public void ResetExcludeLayers() => playerCollider.excludeLayers = default;

        public void UpdateCollisionData()
        {
            ResetMask();
            Vector2 origin = GetOverlapBoxOrigin(config.BottomOverlapOffset);
            Collider2D[] colliders = OverlapBoxAllAndReturnColliders(origin, config.BottomOverlapBoxSize);
            CheckBottomCollisions(colliders);

            origin = GetOverlapBoxOrigin(config.TopOverlapOffset);
            colliders = OverlapBoxAllAndReturnColliders(origin, config.TopOverlapBoxSize);
            CheckTopCollisions(colliders);

            origin = GetOverlapBoxOrigin(config.ForwardOverlapOffset);
            colliders = OverlapBoxAllAndReturnColliders(origin, config.ForwardOverlapBoxSize);
            CheckCollisions(colliders, config.ForwardMask, CollisionDetectionMask.Forward);
        }

        public void Dispose()
        {
            topDetectionLayerMap.Clear();
            subscribtions.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetMask() => detectedMask = CollisionDetectionMask.None;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Collider2D[] OverlapBoxAllAndReturnColliders(in Vector2 origin, Vector2 overlapBoxSize) 
            => Physics2D.OverlapBoxAll(origin, overlapBoxSize, 0f, overlapMask);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckTopCollisions(Collider2D[] colliders)
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
                    CollisionDetectionMask detected = topDetectionLayerMap[gameObjectLayer];
                    detectedMask |= detected;
                }
            }
        }

        private void CheckBottomCollisions(Collider2D[] colliders)
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
                    CollisionDetectionMask detected = bottomDetectionLayerMap[gameObjectLayer];
                    detectedMask |= detected;

                    if (detected == CollisionDetectionMask.Ground)
                        CorrectYPosition();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CorrectYPosition()
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                GetOverlapBoxOrigin(config.BottomOverlapOffset) + Vector2.up * 0.3f, config.BottomOverlapBoxSize, 0f, Vector2.down, 0.5f, config.ForwardMask); //TODO refactor

            if (hit)
            {
                var playerPosition = playerRb.position;
                var point = hit.point;

                if (hit.normal == Vector2.up)
                {
                    detectedMask |= CollisionDetectionMask.Ground;

                    if(playerPosition.y < point.y)
                        playerRb.position = new Vector3(playerRb.position.x, point.y);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckCollisions(Collider2D[] colliders, LayerMask layerMask, CollisionDetectionMask detectionMask)
        {
            var count = colliders.Length;

            if (count > 0)
            {
                if (detectedMask.HasFlag(CollisionDetectionMask.None))
                    detectedMask ^= CollisionDetectionMask.None;

                for (int i = 0; i < count; i++)
                {
                    GameObject go = colliders[i].gameObject;
                    if (go.IsSameLayer(layerMask))
                        detectedMask |= detectionMask;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 GetOverlapBoxOrigin(Vector2 offset) 
            => playerRb.position + new Vector2(offset.x * directionMultiplier, offset.y);

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
