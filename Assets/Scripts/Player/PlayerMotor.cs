using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    public float DashDuration => dashDuration;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform cameraTransform;

    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.25f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector3 dashDirection;
    // Keep dashSpeed as a field, but it will be updated.
    private float dashSpeed = 0f;

    // --- DEBUG VARIABLES ---
    private Vector3 dashStartPos;
    private float actualDashTime = 0f;
    // --- END DEBUG VARIABLES ---

    // Public Events
    public event Action OnLanded;
    public event Action OnAirborne;
    public event Action OnJumped;
    public event Action OnDash;
    public event Action OnDashEnded;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();
        CheckGroundState();
        DashUpdate();
    }

    private void CheckGroundState()
    {
        bool groundedNow = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (!wasGrounded && groundedNow)
            OnLanded?.Invoke();
        else if (wasGrounded && !groundedNow)
            OnAirborne?.Invoke();

        wasGrounded = groundedNow;
        isGrounded = groundedNow;
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        if (!isDashing)
            controller.Move(velocity * Time.deltaTime);
    }

    public void Move(Vector2 input)
    {
        if (isDashing) return;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            float inputMagnitude = Mathf.Clamp01(input.magnitude);
            controller.Move(moveDirection * moveSpeed * inputMagnitude * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (!isGrounded) return;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        OnJumped?.Invoke();
    }

    public void Dash()
    {
        if (isDashing) return;

        if (dashDuration <= 0) // Prevent division by zero
        {
            dashDuration = 0.01f;
        }
        dashSpeed = dashDistance / dashDuration;


        dashDirection = transform.forward;
        dashTimer = dashDuration;
        isDashing = true;
        velocity.y = 0f;

        dashStartPos = transform.position;
        actualDashTime = 0f;

        OnDash?.Invoke();
    }

    private void DashUpdate()
    {
        if (!isDashing) return;

        controller.Move(dashDirection * dashSpeed * Time.deltaTime);
        dashTimer -= Time.deltaTime;
        actualDashTime += Time.deltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;

            Vector3 dashEndPos = transform.position;
            float totalDistanceTraveled = Vector3.Distance(dashStartPos, dashEndPos);

            OnDashEnded?.Invoke();
        }
    }

    public bool IsGrounded() => isGrounded;
    public bool IsDashing() => isDashing;
}