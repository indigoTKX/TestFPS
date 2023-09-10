using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class EnemyAnimationController : MonoBehaviour
    {
        protected Animator Animator;
        protected EnemyCharacterController EnemyController;
        protected bool IsIdle = false;
        
        private const string MOVING_ANIMATION_UID = "Moving";
        private const string DAMAGE_TAKEN_ANIMATION_UID = "DamageTaken";

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            EnemyController = GetComponent<EnemyCharacterController>();
        }

        private void Update()
        {
            if (EnemyController.IsIdle)
            {
                SetIdleState();
            }
            else
            {
                SetMovingState();
            }
        }

        protected virtual void SetIdleState()
        {
            IsIdle = true;
            Animator.SetBool(MOVING_ANIMATION_UID, false);
        }

        protected virtual void SetMovingState()
        {
            IsIdle = false;
            Animator.SetBool(MOVING_ANIMATION_UID, true);
        }
        
        protected virtual void TakeDamage()
        {
            Animator.SetTrigger(DAMAGE_TAKEN_ANIMATION_UID);
        }
        
        
    }
}