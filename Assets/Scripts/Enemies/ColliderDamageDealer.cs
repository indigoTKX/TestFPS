using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class ColliderDamageDealer : MonoBehaviour
    {
        [SerializeField] private float _damage;
        
        private void OnCollisionEnter(Collision collision)
        {
            var hitDamageable = collision.gameObject.GetComponent<IDamageable>(); 
            
            if (hitDamageable != null)
            {
                hitDamageable.DealDamage(_damage);
            }
        }
    }
}