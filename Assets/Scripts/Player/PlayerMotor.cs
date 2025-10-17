using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    public float DashDuration => _dashDuration;

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
    [SerializeField] private float _dashDuration = 0.25f;

    private bool _isDashing = false;
    private float _dashTimer = 0f;
    private Vector3 _dashDirection;
    private float _dashSpeed = 0f;

    // Public Events
    public event Action OnLanded;
    public event Action OnAirborne;
    public event Action OnJumped;
    public event Action OnDash;
    public event Action OnDashEnded;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _wasGrounded;

    private void Awake()
    {
        if (_controller == null)
        {
            _controller = GetComponent<CharacterController>();
        }
        else
        {
            Debug.LogWarning("CharacterController component is missing on the GameObject!", this);
        }
    }

    private void Update()
    {
        ApplyGravity();
        CheckGroundState();
        DashUpdate();
    }

    private void ApplyGravity()
    {
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += _gravity * Time.deltaTime;

        if (!_isDashing)
            _controller.Move(_velocity * Time.deltaTime);
    }

    private void CheckGroundState()
    {
        bool groundedNow = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (!_wasGrounded && groundedNow)
            OnLanded?.Invoke();
        else if (_wasGrounded && !groundedNow)
            OnAirborne?.Invoke();

        _wasGrounded = groundedNow;
        _isGrounded = groundedNow;
    }

    public void Move(Vector2 input)
    {
        if (_isDashing) return;

        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            float inputMagnitude = Mathf.Clamp01(input.magnitude);
            _controller.Move(moveDirection * _moveSpeed * inputMagnitude * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (!_isGrounded) return;
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        OnJumped?.Invoke();
    }

    public void Dash()
    {
        if (_isDashing) return;

        if (_dashDuration <= 0) // Prevent division by zero
        {
            _dashDuration = 0.01f;
        }
        _dashSpeed = _dashDistance / _dashDuration;


        _dashDirection = transform.forward;
        _dashTimer = _dashDuration;
        _isDashing = true;
        _velocity.y = 0f;

        OnDash?.Invoke();
    }

    private void DashUpdate()
    {
        if (!_isDashing) return;

        _controller.Move(_dashDirection * _dashSpeed * Time.deltaTime);
        _dashTimer -= Time.deltaTime;

        if (_dashTimer <= 0f)
        {
            _isDashing = false;

            Vector3 dashEndPos = transform.position;
            OnDashEnded?.Invoke();
        }
    }

    public bool IsGrounded() => _isGrounded;
    public bool IsDashing() => _isDashing;
}