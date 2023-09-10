using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TestFPS.Gameplay
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField] public Sprite ammoSprite;
        [SerializeField] public int MaxAmmo = 10;
        [SerializeField] public float shotCooldown;
        [SerializeField] public bool IsAutomatic = false;
        [SerializeField] public float ReloadTime;

        
        [SerializeField] private Projectile _bulletPrefab;
        [SerializeField] private float _damage;
        [SerializeField] private float _speed;

        [HideInInspector] public Transform BulletSpawnPoint;
        
        [HideInInspector] public bool IsReloading = false;

        public int CurrentAmmo { get; private set; } = 0;

        private float _lastShotTime = 0f;

        public void Reload()
        {
            CurrentAmmo = MaxAmmo;
            IsReloading = false;
            _lastShotTime = 0f;
        }
        
        public void TryToShoot(Vector3 aimDirection, Vector3 upDirection)
        {
            if (IsReloading) return;
            
            var isOnCooldown = Time.time - _lastShotTime <= shotCooldown;
            if (isOnCooldown || CurrentAmmo <= 0) return;

            Shoot(aimDirection, upDirection);
            CurrentAmmo -= 1;
            _lastShotTime = Time.time;
        }
        private void Shoot(Vector3 aimDirection, Vector3 upDirection)
        {
            var bulletRotation = Quaternion.LookRotation(aimDirection, upDirection);
            var bullet = Instantiate(_bulletPrefab, BulletSpawnPoint.position, bulletRotation);
            bullet.Damage = _damage;
            bullet.Speed = _speed;
        }
    }
}