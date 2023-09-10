using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public interface IDamageable
    {
        public bool IsAlive { get; }
        
        public void DealDamage(float damage);
    }
}