using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TestFPS.Gameplay
{
    public class InputManager : SingletonBase<InputManager>
    {
        public float LookSensitivity = 1f;
        public Vector3 MoveDirection { get; private set; } = Vector3.zero;
        public float LookInputX { get; private set; } = 0f;
        public float LookInputY { get; private set; } = 0f;

        public bool GetJumpInput()
        {
            return CanProcessGameplayInput() && Input.GetKey(KeyCode.Space);
        }
        
        public bool GetFireInput()
        {
            return CanProcessGameplayInput() && Input.GetKeyDown(KeyCode.Mouse0);
        }

        public bool GetFireInputContinuous()
        {
            return CanProcessGameplayInput() && Input.GetKey(KeyCode.Mouse0);
        }

        public bool GetSwitchWeaponInput()
        {
            return CanProcessGameplayInput() && Input.GetKeyDown(KeyCode.Q);
        }
        
        public bool GetReloadInput()
        {
            return CanProcessGameplayInput() && Input.GetKeyDown(KeyCode.R);
        }

        public bool GetAneKeyInput()
        {
            return Input.anyKey;
        }

        private const string MOUSE_AXIS_NAME_HORIZONTAL = "Mouse X";
        private const string MOUSE_AXIS_NAME_VERTICAL = "Mouse Y";
        private const string MOVE_AXIS_NAME_HORIZONTAL = "Horizontal";
        private const string MOVE_AXIS_NAME_VERTICAL = "Vertical";

        private bool _areInputsLocked = false;
        private GameStateManager _gameStateManager;
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _gameStateManager = GameStateManager.Instance;
        }

        private void Update()
        {
            UpdateInputLock();
            
            LookInputX = GetLookInputAxis(MOUSE_AXIS_NAME_HORIZONTAL);
            LookInputY = GetLookInputAxis(MOUSE_AXIS_NAME_VERTICAL);

            MoveDirection = GetMoveInput();
        }

        private void UpdateInputLock()
        {
            var shouldLockInputs = Input.GetKeyDown(KeyCode.Escape);
            
            if (!shouldLockInputs) return;

            if (_areInputsLocked)
            {
                _areInputsLocked = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                _areInputsLocked = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        
        private float GetLookInputAxis(string mouseInputName)
        {
            if (!CanProcessGameplayInput()) return 0f;
            
            var i = Input.GetAxisRaw(mouseInputName);
            i *= LookSensitivity;
            return i;
        }

        private Vector3 GetMoveInput()
        {
            if (!CanProcessGameplayInput()) return Vector3.zero;
            
            var newMoveDir = Vector3.zero;
            newMoveDir.x = Input.GetAxisRaw(MOVE_AXIS_NAME_HORIZONTAL);
            newMoveDir.z = Input.GetAxisRaw(MOVE_AXIS_NAME_VERTICAL);
            newMoveDir.Normalize();
            return newMoveDir;
        }
        
        private bool CanProcessGameplayInput()
        {
            return !_areInputsLocked && _gameStateManager.CurrentGameState == GameState.PLAY;
        }
        
    }
}


