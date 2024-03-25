using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Input
{
    [DefaultExecutionOrder(-1)]
    public class TouchInputManager : MonoSingleton<TouchInputManager>
    {
        #region Events
        public delegate void StartTouch(Vector2 position, float time);
        public event StartTouch OnStartTouch;
        public delegate void EndTouch(Vector2 position, float time);
        public event EndTouch OnEndTouch;
        #endregion
    
        private PlayerInputAction _playerInputAction;
        private Camera mainCamera;

        protected override void Awake()
        {
            base.Awake();
            _playerInputAction = new PlayerInputAction();
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _playerInputAction.Enable();
        }

        private void OnDisable()
        {
            _playerInputAction.Disable();
            _playerInputAction.Touch.PrimaryContact.started -= StartTouchPrimary;
            _playerInputAction.Touch.PrimaryContact.canceled -= EndTouchPrimary;
        }

        private void Start()
        {
            _playerInputAction.Touch.PrimaryContact.started += StartTouchPrimary;
            _playerInputAction.Touch.PrimaryContact.canceled += EndTouchPrimary;
        }
    
        private void StartTouchPrimary(InputAction.CallbackContext ctx)
        {
            OnStartTouch?.Invoke(Utilities.ScreenToWorld(mainCamera,
                _playerInputAction.Touch.PrimaryPosition.ReadValue<Vector2>()), (float) ctx.startTime);
        }
    
        private void EndTouchPrimary(InputAction.CallbackContext ctx)
        {
            OnEndTouch?.Invoke(Utilities.ScreenToWorld(mainCamera,
                _playerInputAction.Touch.PrimaryPosition.ReadValue<Vector2>()), (float) ctx.time);
        }

        public Vector2 GetPrimaryPosition()
        {
            return Utilities.ScreenToWorld(mainCamera, _playerInputAction.Touch.PrimaryPosition.ReadValue<Vector2>());
        }

    }
}
