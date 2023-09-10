using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class WeaponManager : SingletonBase<WeaponManager>
    {
        [SerializeField] public Weapon[] Weapons;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _bulletSpawnPoint;
        
        [Tooltip("It will shoot to the object with selected mask")]
        [SerializeField] private LayerMask _aimColliderMask = -1;

        public Weapon CurrentWeapon { get; private set; }
        
        private const float MAX_SHOOT_RAY_DISTANCE = 999f;
        
        private InputManager _inputManager;
        private Vector3 _mouseWorldPosition = Vector3.zero;
        private Vector3 _aimDirection = Vector3.zero;

        private int _currentWeaponIdx = 0;
    
        private void Start()
        {
            _inputManager = InputManager.Instance;
            
            CurrentWeapon = Weapons[_currentWeaponIdx];
            foreach (var weapon in Weapons)
            {
                weapon.Reload();
                weapon.BulletSpawnPoint = _bulletSpawnPoint;
            }
        }
        
        private void Update()
        {
            CreateAimPoint();

            HandleSwitchInput();

            if (_inputManager.GetReloadInput())
            {
                StartCoroutine(ReloadCoroutine(CurrentWeapon));
            }

            var fireInput = CurrentWeapon.IsAutomatic
                ? _inputManager.GetFireInputContinuous()
                : _inputManager.GetFireInput();
            
            if (fireInput)
            {
                CurrentWeapon.TryToShoot(_aimDirection, _playerCamera.transform.up);
            }
        }
        
        private void CreateAimPoint()
        {
            CastRayAtTheCenterOfTheScreen();
            _aimDirection = (_mouseWorldPosition - _bulletSpawnPoint.position).normalized;
        }
        
        private void CastRayAtTheCenterOfTheScreen()
        {
            var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = _playerCamera.ScreenPointToRay(screenCenterPoint);
            Debug.DrawRay(ray.origin, ray.direction * 999f, Color.blue);

            var isAimingToSomething = Physics.Raycast(ray, out var raycastHit, MAX_SHOOT_RAY_DISTANCE, _aimColliderMask);
            _mouseWorldPosition = isAimingToSomething
                ? raycastHit.point
                : ray.origin + ray.direction * MAX_SHOOT_RAY_DISTANCE;
        }

        private void HandleSwitchInput()
        {
            if (!_inputManager.GetSwitchWeaponInput()) return;
            
            _currentWeaponIdx += 1;
            if (_currentWeaponIdx >= Weapons.Length)
            {
                _currentWeaponIdx = 0;
            }

            CurrentWeapon = Weapons[_currentWeaponIdx];
        }

        private IEnumerator ReloadCoroutine(Weapon weapon)
        {
            weapon.IsReloading = true;
            yield return new WaitForSeconds(weapon.ReloadTime);
            weapon.Reload();
        }
    }
}