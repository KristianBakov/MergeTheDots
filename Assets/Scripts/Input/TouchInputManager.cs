using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Input
{
    [DefaultExecutionOrder(-1)]
    public class TouchInputManager : MonoSingleton<TouchInputManager>
    {
        #region Events
        public Action OnStartTouchInput;
        public Action OnEndTouchInput;
        #endregion
    
        private PlayerInputAction _playerInputAction;
        private Camera mainCamera;

        public bool IsTouching { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            _playerInputAction = new PlayerInputAction();
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _playerInputAction.Enable();
            _playerInputAction.Touch.PrimaryContact.started += StartTouchPrimary;
            _playerInputAction.Touch.PrimaryContact.canceled += EndTouchPrimary;
        }

        private void OnDisable()
        {
            _playerInputAction.Touch.PrimaryContact.started -= StartTouchPrimary;
            _playerInputAction.Touch.PrimaryContact.canceled -= EndTouchPrimary;
            _playerInputAction.Disable();
        }
    
        private void StartTouchPrimary(InputAction.CallbackContext ctx)
        {
            IsTouching = true;
            OnStartTouchInput?.Invoke();
        }
    
        private void EndTouchPrimary(InputAction.CallbackContext ctx)
        {
            IsTouching = false;
            OnEndTouchInput?.Invoke();
        }

        public Vector2 GetPrimaryPosition()
        {
            return Utilities.ScreenToWorld(mainCamera, _playerInputAction.Touch.PrimaryPosition.ReadValue<Vector2>());
        }
    }
}
