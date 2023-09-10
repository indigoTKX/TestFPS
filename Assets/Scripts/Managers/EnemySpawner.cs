using System;
using System.Collections;
using System.Collections.Generic;
using TestFPS.Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestFPS.Gameplay
{
    public class EnemySpawner : SingletonBase<EnemySpawner>
    {
        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private GameObject _slimePrefab;
        [SerializeField] private float _slimeSpawnCooldown = 2f;
        [SerializeField] private GameObject _bigSlimePrefab;
        [SerializeField] private float _bigSlimeSpawnCooldown = 2f;

        private Transform _playerTransform;
        private GameStateManager _gameStateManager;

        private void Start()
        {
            var playerReference = PlayerReference.Instance;
            _playerTransform = playerReference.Player.transform;

            _gameStateManager = GameStateManager.Instance;
            _gameStateManager.OnStateChanged += HandleOnStateChanged;

            StartSpawning();
        }

        private void OnDestroy()
        {
            _gameStateManager.OnStateChanged -= HandleOnStateChanged;
        }

        private void HandleOnStateChanged(GameState gameState)
        {
            if (gameState == GameState.PLAY)
            {
                StartSpawning();
            }
            else
            {
                StopAllCoroutines();
            }
        }
        
        private void StartSpawning()
        {
            StartCoroutine(SpawnSlime(_slimePrefab, _slimeSpawnCooldown));
            StartCoroutine(SpawnSlime(_bigSlimePrefab, _bigSlimeSpawnCooldown));
        }

        private IEnumerator SpawnSlime(GameObject prefab, float cooldown)
        {
            while (true)
            {
                yield return new WaitForSeconds(cooldown);
                var randomIdx = Random.Range(0, _spawnPoints.Length);
                var pickedSpawnPoint = _spawnPoints[randomIdx];
                var enemy = Instantiate(prefab, pickedSpawnPoint.position, Quaternion.identity);
                enemy.transform.LookAt(_playerTransform);
            }
        }
    }
}