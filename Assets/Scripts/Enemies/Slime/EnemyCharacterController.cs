using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class EnemyCharacterController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 3f;

        [Tooltip("Masks of the objects that enemy should see")]
        [SerializeField] private LayerMask _activeMasks;

        [Tooltip("The distance at which the enemy can spot the player.")] 
        [SerializeField] private float _canSeeRange = 10f;

        [Tooltip("Position from which enemy shoots the Raycast to the Player")] 
        [SerializeField] private Transform _eyesPosition;

        public bool IsIdle { get; private set; } = true;
        
        protected Transform Transform;

        private Rigidbody _rigidbody;
        private GameObject _player;
        private GameStateManager _gameStateManager;
        private Vector3 _playerCenterOffset;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _player = PlayerReference.Instance.Player;
            _playerCenterOffset = _player.GetComponent<CapsuleCollider>().center; 

            
            Transform = transform;

            _gameStateManager = GameStateManager.Instance;
        }

        private void Update()
        {
            IsIdle = true;
            if (_gameStateManager.CurrentGameState != GameState.PLAY) return;
            
            if (CanSeePlayer())
            {
                MoveToPlayer();
            }
        }
        
        private bool CanSeePlayer()
        {
            var direction = (_player.transform.position + _playerCenterOffset) - _eyesPosition.transform.position;
            if (Physics.Raycast(_eyesPosition.position, direction, out var hit, _canSeeRange, _activeMasks))
            {
                if (hit.collider.GetComponent<PlayerReference>())
                {
                    Debug.DrawRay(_eyesPosition.position, direction, Color.green);
                    return true;
                }
            }

            Debug.DrawRay(_eyesPosition.position, direction, Color.red);
            return false;
        }

        protected virtual void MoveToPlayer()
        {
            IsIdle = false;
            
            var playerPosition = _player.transform.position;
            transform.LookAt(playerPosition);
            
            var targetDir = playerPosition - Transform.position;
            targetDir.y = 0;
            
            _rigidbody.velocity = targetDir.normalized * (_moveSpeed * Time.deltaTime);
        }
    }
}