using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] public float Damage = 50f;
        [SerializeField] public float Speed = 10f;
        
        [SerializeField] private float _maxLifeTime = 5f;
        [SerializeField] private Transform _sphereTransform;
        [SerializeField] private LayerMask _damageableLayers = -1;

        private Collider _collider;
        private Vector3 _lastFramePosition;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _lastFramePosition = _transform.position;
            
            Destroy(gameObject, _maxLifeTime);
        }

        private void FixedUpdate()
        {
            var thisFramePosition = _transform.position;
            thisFramePosition += _transform.forward * (Speed * Time.fixedDeltaTime);
            _transform.position = thisFramePosition;

            var travelledDistance = Vector3.Distance(_lastFramePosition, thisFramePosition);
            var travelledDirection = thisFramePosition - _lastFramePosition;
            var sphereSize = _sphereTransform.lossyScale.x / 2;
            
            var isHit = Physics.SphereCast(_lastFramePosition, sphereSize, travelledDirection,
                out var hit, travelledDistance, _damageableLayers, QueryTriggerInteraction.Collide);

            _lastFramePosition = thisFramePosition;
            if (!isHit) return;

            var damageable = hit.collider.GetComponent<IDamageable>();
            damageable?.DealDamage(Damage);

            Destroy(gameObject);
        }
    }
}