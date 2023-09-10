using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TestFPS.Gameplay
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private int _playerHP = 3;
        [SerializeField] private GameObject[] _HpCounter;
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private TextMeshProUGUI _ammoText;
        [SerializeField] private GameObject _gameOverLabel;
        [SerializeField] private GameObject _aimPoint;

        private WeaponManager _weaponManager;
        private Damageable _playerDamageable;
        private GameStateManager _gameStateManager;
        
        private void Start()
        {
            _weaponManager = WeaponManager.Instance;
            
            var playerReference = PlayerReference.Instance;
            _playerDamageable = playerReference.Player.GetComponent<Damageable>();

            _gameStateManager = GameStateManager.Instance;
            _gameStateManager.OnStateChanged += HandleChangeGameState;
        }

        private void OnDestroy()
        {
            _gameStateManager.OnStateChanged -= HandleChangeGameState;
        }

        private void Update()
        {
            UpdateHpCounter();
            UpdateWeaponIcon();
            UpdateAmmo();
        }

        private void UpdateAmmo()
        {
            var currentAmmoString = _weaponManager.CurrentWeapon.CurrentAmmo.ToString();
            var maxAmmoString = _weaponManager.CurrentWeapon.MaxAmmo.ToString();
            _ammoText.text = currentAmmoString + "/" + maxAmmoString;
        }
        private void UpdateWeaponIcon()
        {
            _weaponIcon.sprite = _weaponManager.CurrentWeapon.ammoSprite;
        }

        private void UpdateHpCounter()
        {
            _playerHP = (int)_playerDamageable.CurrentHealth;
            
            switch (_playerHP)
            {
                case 3:
                {
                    foreach (var hp in _HpCounter)
                    {
                        hp.SetActive(true);
                    }

                    break;
                }
                case 2:
                    _HpCounter[2].SetActive(false);
                    break;
                case 1:
                    _HpCounter[2].SetActive(false);
                    _HpCounter[1].SetActive(false);
                    break;
                case 0:
                {
                    foreach (var hp in _HpCounter)
                    {
                        hp.SetActive(false);
                    }
                    break;
                }
            }
        }

        private void HandleChangeGameState(GameState gameState)
        {
            var isGameOver = gameState == GameState.GAME_OVER;
            _gameOverLabel.SetActive(isGameOver);
            _aimPoint.SetActive(!isGameOver);
        }
    }
}