using UnityEngine;
using UnityEngine.InputSystem;

namespace Runner.Core
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput _playerInputs;
        private InputActionMap _actionMap;

        public InputAction TouchPositionAction { get; private set; }
        public InputAction TouchStartPositionAction { get; private set; }
        public InputAction TouchContactAction { get; private set; }

        public Vector2 TouchPosition {  get; private set; }
        public Vector2 TouchStartPosition {  get; private set; }
        public bool IsTouch { get; private set; }


        private void Awake()
        {
            _playerInputs = GetComponent<PlayerInput>();
            _actionMap = _playerInputs.currentActionMap;

            TouchPositionAction = _actionMap.FindAction("SwipePosition");
            TouchStartPositionAction = _actionMap.FindAction("SwipeStartPosition");
            TouchContactAction = _actionMap.FindAction("SwipeContact");
        }

        private void OnEnable()
        {
            TouchPositionAction.performed += OnTouchPosition;
            TouchStartPositionAction.performed += OnStartTouchPosition;
            TouchContactAction.performed += OnTouchContact;

            TouchPositionAction.canceled += OnTouchPosition;
            TouchStartPositionAction.canceled += OnStartTouchPosition;
            TouchContactAction.canceled += OnTouchContact;
        }

        private void OnDisable()
        {
            TouchPositionAction.performed -= OnTouchPosition;
            TouchStartPositionAction.performed -= OnStartTouchPosition;
            TouchContactAction.performed -= OnTouchContact;

            TouchPositionAction.canceled -= OnTouchPosition;
            TouchStartPositionAction.canceled -= OnStartTouchPosition;
            TouchContactAction.canceled -= OnTouchContact;
        }

        private void OnTouchPosition(InputAction.CallbackContext context)
        {
            TouchPosition = context.ReadValue<Vector2>();
        }

        private void OnStartTouchPosition(InputAction.CallbackContext context)
        {
            TouchStartPosition = context.ReadValue<Vector2>();
        }

        private void OnTouchContact(InputAction.CallbackContext context)
        {
            IsTouch = context.control.IsPressed();
        }
    }
}
