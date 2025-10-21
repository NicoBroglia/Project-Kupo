using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ActionSpeedController))]
public class PlayerMotor : MonoBehaviour
{
    public float DashDuration => GetCurrentDashDuration();

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private Transform _cameraTransform;

    [Header("Jump & Gravity")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.2f;
    [SerializeField] private LayerMask _groundMask;

    [Header("Dash")]
    [SerializeField] private float _dashDistance = 5f;
    [Tooltip("The base duration of a dash at 1.0x speed.")]
    [SerializeField] private float _baseDashDuration = 0.35f;

    private bool _isDashing = false;
    private float _dashTimer = 0f;
    private Vector3 _dashDirection;
    private float _dashSpeed = 0f;

    public event Action OnLanded = delegate { };
    public event Action OnAirborne = delegate { };
    public event Action OnJumped = delegate { };
    public event Action OnDash = delegate { };
    public event Action OnDashEnded = delegate { };

    private CharacterController _controller;
    private ActionSpeedController _actionSpeedController;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _wasGrounded;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _actionSpeedController = GetComponent<ActionSpeedController>();
    }

    private void Update()
    {
        // Add a master null check for robustness during scene shutdown.
        if (_controller == null || _actionSpeedController == null) return;

        HandleGravity();
        CheckGroundState();
        UpdateDash();
    }

    private float GetCurrentDashDuration()
    {
        if (_actionSpeedController != null)
        {
            // Logic changed from division to multiplication for clarity.
            float duration = _baseDashDuration * _actionSpeedController.ActionDurationMultiplier;
            return Mathf.Max(0.01f, duration);
        }
        return _baseDashDuration;
    }

    public void Dash(Vector2 moveInput)
    {
        if (_isDashing) return;

        float currentDuration = GetCurrentDashDuration();

        if (currentDuration > 0)
        {
            _dashSpeed = _dashDistance / currentDuration;
        }

        if (moveInput.sqrMagnitude > 0.01f)
        {
            _dashDirection = GetCameraRelativeMoveDirection(moveInput);
        }
        else
        {
            _dashDirection = transform.forward;
        }

        _dashTimer = currentDuration;
        _isDashing = true;
        _velocity.y = 0f;

        OnDash.Invoke();
    }

    private void HandleGravity()
    {
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;

        if (!_isDashing)
        {
            _controller.Move(_velocity * Time.deltaTime);
        }
    }

    private void CheckGroundState()
    {
        bool groundedNow = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (!_wasGrounded && groundedNow)
            OnLanded.Invoke();
        else if (_wasGrounded && !groundedNow)
            OnAirborne.Invoke();

        _wasGrounded = groundedNow;
        _isGrounded = groundedNow;
    }

    public void Move(Vector2 input)
    {
        if (_isDashing) return;

        Vector3 moveDirection = GetCameraRelativeMoveDirection(input);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);
            RotateTowards(moveDirection);
        }
    }

    private Vector3 GetCameraRelativeMoveDirection(Vector2 input)
    {
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        return (camForward * input.y + camRight * input.x).normalized;
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        if (!_isGrounded) return;
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        OnJumped.Invoke();
    }

    private void UpdateDash()
    {
        if (!_isDashing) return;

        _controller.Move(_dashDirection * _dashSpeed * Time.deltaTime);
        _dashTimer -= Time.deltaTime;

        if (_dashTimer <= 0f)
        {
            _isDashing = false;
            OnDashEnded.Invoke();
        }
    }

    public bool IsGrounded() => _isGrounded;
    public bool IsDashing() => _isDashing;
}