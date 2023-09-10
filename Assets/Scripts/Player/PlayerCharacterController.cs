using System;
using System.Collections;
using System.Collections.Generic;
using TestFPS.Gameplay;
using Unity.VisualScripting;
using UnityEngine;


namespace TestFPS.Gameplay
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _maxSpeedOnGround = 20f;
        [SerializeField] private float _groundCheckDistance = 0.05f;
        [SerializeField] private float _rotationSpeed = 200f;
        [SerializeField] private float _jumpAcceleration = 19f;
        [SerializeField] private float _gravityAcceleration = 10f;
        [SerializeField] private LayerMask _groundLayers = -1;

        private const float MAX_CAMERA_VERTICAL_ANGLE = 89f;
        private const float MIN_CAMERA_VERTICAL_ANGLE = -89f;

        private const float JUMP_SNAPPING_PREVENTION_TIME = 0.2f;
        
        private InputManager _inputManager;
        private CapsuleCollider _playerCapsule;
        private Rigidbody _playerRigidbody;
        
        private bool _isGrounded;
        private Vector3 _playerTargetVelocity;
        private float _cameraVerticalAngle = 0f;
        private float _lastJumpTime = -1f;
        private float _verticalVelocity = 0f;

        private void Start()
        {
            _inputManager = InputManager.Instance;
            _playerCapsule = GetComponent<CapsuleCollider>();
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var horizontalRotationVector = new Vector3(0f, _inputManager.LookInputX * _rotationSpeed, 0f);
            transform.Rotate(horizontalRotationVector, Space.Self);
            
            _cameraVerticalAngle += _inputManager.LookInputY * _rotationSpeed;
            _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, MIN_CAMERA_VERTICAL_ANGLE, MAX_CAMERA_VERTICAL_ANGLE);
            _playerCamera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
            
            _playerTargetVelocity = Vector3.zero;

            GroundCheck();

            HandleVerticalMovement();
            HandleHorizontalMovement();

            _playerRigidbody.velocity = _playerTargetVelocity;
        }

        private void GroundCheck()
        {
            _isGrounded = false;

            if (!(Time.time >= _lastJumpTime + JUMP_SNAPPING_PREVENTION_TIME)) return;

            var capsuleBottomPoint = _playerCapsule.transform.position;
            if (!Physics.Raycast(capsuleBottomPoint, Vector3.down, out var hit, _groundCheckDistance, 
                    _groundLayers, QueryTriggerInteraction.Ignore)) return;

            _isGrounded = true;
            _verticalVelocity = 0f;

            //snapping is instant, so without accounting frame time
            // var snapVector = Vector3.down * (hit.distance / 2);
            // transform.position += snapVector;
        }

        private void HandleVerticalMovement()
        {
            if (!_isGrounded)
            {
                _verticalVelocity -= _gravityAcceleration * Time.fixedDeltaTime;
                _playerTargetVelocity += Vector3.up * _verticalVelocity;
                return;
            }

            if (!_inputManager.GetJumpInput()) return;
            
            _lastJumpTime = Time.time;
            _isGrounded = false;
            _verticalVelocity += _jumpAcceleration * Time.fixedDeltaTime;
            
            _playerTargetVelocity += Vector3.up * _verticalVelocity;
        }

        private void HandleHorizontalMovement()
        {
            var worldSpaceMoveInput = transform.TransformVector(_inputManager.MoveDirection);
            var targetHorizontalVelocity = worldSpaceMoveInput * (_maxSpeedOnGround * Time.fixedDeltaTime);
            _playerTargetVelocity += targetHorizontalVelocity;
        }
    }
}