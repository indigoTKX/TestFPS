using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class JumpingSlimeController : EnemyCharacterController
    {
        [SerializeField] private GameObject _slimeProjectiles;
        [SerializeField] private Transform _projectilesSpawnPoint;

        private bool _coroutineStarted;
        
        //called by animation event to sync with animation
        public void ShootProjectilesOnJump()
        {
            Instantiate(_slimeProjectiles, _projectilesSpawnPoint.position, Quaternion.identity);
        }

    }
}