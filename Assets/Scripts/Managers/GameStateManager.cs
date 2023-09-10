using System;
using System.Collections;
using System.Collections.Generic;
using TestFPS.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestFPS.Gameplay
{
    public class GameStateManager : SingletonBase<GameStateManager>
    {
        public event Action<GameState> OnStateChanged;

        [SerializeField] private float _waitForResetTime = 3f;
        
        public GameState CurrentGameState { get; private set; }
        
        private Damageable _playerDamageable;
        private InputManager _inputManager;
        private bool _isReadyToReset = false;

        protected new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            _isReadyToReset = false;
            CurrentGameState = GameState.PLAY;
            
            var playerReference = PlayerReference.Instance;
            _playerDamageable = playerReference.Player.GetComponent<Damageable>();
            _playerDamageable.OnDie += GameOver;

            _inputManager = InputManager.Instance;
        }

        private void OnDestroy()
        {
            if (_playerDamageable != null)
            {
                _playerDamageable.OnDie -= GameOver;
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void GameOver()
        {
            CurrentGameState = GameState.GAME_OVER;
            OnStateChanged?.Invoke(CurrentGameState);
            StartCoroutine(EnableReset());
            Debug.Log("GAME OVER!");
        }

        private IEnumerator EnableReset()
        {
            yield return new WaitForSeconds(_waitForResetTime);
            _isReadyToReset = true;
        }

        private void Update()
        {
            if (CurrentGameState != GameState.GAME_OVER) return;

            if (_inputManager.GetAneKeyInput() && _isReadyToReset)
            {
                ResetGame();
            }
        }

        private void ResetGame()
        {
            //TODO: make a proper reset algotihm
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Start();
        }
    }

    public enum GameState
    {
        PLAY,
        GAME_OVER
    }
}