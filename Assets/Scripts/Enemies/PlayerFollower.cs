using System;
using System.Collections;
using System.Collections.Generic;
using TestFPS.Gameplay;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    // [SerializeField] private float _distanceToPlayer = 10f;
    [SerializeField] private Vector3 _offset;

    private Transform _playerTransform;
    private Transform _transform;

    private void Start()
    {
        var playerReference = PlayerReference.Instance;
        _playerTransform = playerReference.Player.transform;
        _transform = transform;
    }

    private void FixedUpdate()
    {
        _transform.rotation = _playerTransform.rotation;
        
        var correctedOffset = _transform.rotation * _offset;
        _transform.position = _playerTransform.position + correctedOffset;
    }
}
