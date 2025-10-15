using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform cameraTransform;

    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    public event Action OnLanded;
    public event Action OnAirborne;
    public event Action OnJumped;

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

    public void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(Vector2 input)
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.y + camRight * input.x).normalized;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (!isGrounded) return;
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        OnJumped?.Invoke();
    }

    public bool IsGrounded() => isGrounded;
}
