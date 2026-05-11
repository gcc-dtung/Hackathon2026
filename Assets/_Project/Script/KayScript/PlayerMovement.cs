using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.2f;

    [Header("Look Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lookSensitivity = 0.2f;
    [SerializeField] private float gamepadLookSensitivity = 150f;
    [SerializeField] private float topClamp = -89f;
    [SerializeField] private float bottomClamp = 89f;
    [SerializeField] private TouchField touchField;

    [Header("Input Reference")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference lookAction;

    private CharacterController _controller;
    private Vector2 _moveDirection = Vector2.zero;
    private Vector2 _lookInput = Vector2.zero;
    private Vector3 _velocity;
    private bool _isGrounded;
    private float _xRotation = 0f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (jumpAction != null) jumpAction.action.Enable();
        if (lookAction != null) lookAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (jumpAction != null) jumpAction.action.Disable();
        if (lookAction != null) lookAction.action.Disable();
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        HandleMovement();
        HandleLook();
        HandleJump();
        ApplyGravityAndMove();
    }

    private void HandleMovement()
    {
        if (moveAction != null)
        {
            _moveDirection = moveAction.action.ReadValue<Vector2>();
        }
    }

    private void HandleJump()
    {
        if (jumpAction != null && jumpAction.action.WasPressedThisFrame())
        {
            if (_isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }

    private void ApplyGravityAndMove()
    {
        _velocity.y += gravity * Time.deltaTime;
        Vector3 move = transform.right * _moveDirection.x + transform.forward * _moveDirection.y;
        
        Vector3 finalMovement = move * moveSpeed + _velocity;

        _controller.Move(finalMovement * Time.deltaTime);
    }

    private void HandleLook()
    {
        if (playerCamera != null)
        {
            float lookX = 0f;
            float lookY = 0f;

            // 1. Kiểm tra TouchField (Mobile / Touch)
            if (touchField != null && touchField.TouchDelta.sqrMagnitude > 0.001f)
            {
                lookX += touchField.TouchDelta.x * lookSensitivity;
                lookY += touchField.TouchDelta.y * lookSensitivity;
            }

            // 2. Kiểm tra Look Action (Gamepad / Mouse)
            if (lookAction != null)
            {
                Vector2 lookValue = lookAction.action.ReadValue<Vector2>();
                if (lookValue.sqrMagnitude > 0.001f)
                {
                    // Kiểm tra xem input có phải từ Gamepad không
                    bool isGamepad = lookAction.action.activeControl != null && 
                                     lookAction.action.activeControl.device is Gamepad;

                    if (isGamepad)
                    {
                        // Joystick tay cầm trả về giá trị (-1 đến 1), cần nhân với Sensitivity riêng và deltaTime
                        lookX += lookValue.x * gamepadLookSensitivity * Time.deltaTime;
                        lookY += lookValue.y * gamepadLookSensitivity * Time.deltaTime;
                    }
                    else if (touchField == null) // Chỉ dùng chuột nếu không có TouchField
                    {
                        // Chuột trả về delta pixel, dùng chung lookSensitivity
                        lookX += lookValue.x * lookSensitivity;
                        lookY += lookValue.y * lookSensitivity;
                    }
                }
            }

            _xRotation -= lookY;
            _xRotation = Mathf.Clamp(_xRotation, topClamp, bottomClamp);

            playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            
            transform.Rotate(Vector3.up * lookX);
        }
    }
}
