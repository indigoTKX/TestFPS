using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class SlimeAnimationController : EnemyAnimationController
    {
        [SerializeField] private float _timeBeforeNextIdleAnimation = 3f;
        [SerializeField] private float _yOffsetAfterDie = 4f;
        [SerializeField] private float _fallingSpeed = 1f;
        
        private const string NEXT_IDLE_ANIMATION_UID = "NextIdle";
        private const string DAMAGE_TAKEN_ANIMATION_UID = "DamageTaken";
        private const string DIE_ANIMATION_UID = "Die";

        private bool _coroutineStarted = false;
        private Rigidbody _rigidBody;
        private Damageable _damageable;

        protected override void Awake()
        {
            base.Awake();
            _rigidBody = GetComponent<Rigidbody>();
            _damageable = GetComponent<Damageable>();
            _damageable.OnDamaged += (f) => HandleOnDamaged();
            _damageable.OnDie += Die;
        }

        private void OnDestroy()
        {
            _damageable.OnDamaged -= (f) => HandleOnDamaged();
            _damageable.OnDie -= Die;
        }

        protected override void SetIdleState()
        {
            base.SetIdleState();
            if (!_coroutineStarted)
            {
                StartCoroutine(IdleIteration());
            }
        }

        private IEnumerator IdleIteration()
        {
            _coroutineStarted = true;
            yield return new WaitForSeconds(_timeBeforeNextIdleAnimation);
            if (IsIdle)
            {
                Animator.SetTrigger(NEXT_IDLE_ANIMATION_UID);
            }

            _coroutineStarted = false;
        }
        
        private void HandleOnDamaged()
        {
            Animator.SetTrigger(DAMAGE_TAKEN_ANIMATION_UID);
        }
        
        private void Die()
        {
            _rigidBody.isKinematic = true;
            Animator.SetTrigger(DIE_ANIMATION_UID);
        }
        
        //called by animation event to sync with animation
        public void MoveUnderground()
        {
            var newPosition = transform.position + new Vector3(0, -_yOffsetAfterDie, 0);
            StartCoroutine(MoveAndDestroy(newPosition));
        }
        
        private IEnumerator MoveAndDestroy(Vector3 newPosition)
        {
            while (transform.position != newPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * _fallingSpeed);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}