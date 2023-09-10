using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class SlimeProjectileScript : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            _transform.position += _transform.forward * (_speed * Time.deltaTime);
        }
    }
}