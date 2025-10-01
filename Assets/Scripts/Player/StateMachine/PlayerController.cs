using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(PlayerInputHandler))]
[RequireComponent (typeof(PlayerStateController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputHandler inputHandler;
    
    private PlayerStateController stateController;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 velocity;

    [Header("References")]
    [SerializeField] private Transform cameraTransform; // assign Main Camera here

    void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        if (inputHandler == null)
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }
      
        if (stateController == null)
        {
            stateController = GetComponent<PlayerStateController>();
        }
      
    }
    void Start()
    {
        var stateDict = new Dictionary<PlayerStates, IPlayerState>
        {
            { PlayerStates.Idle, new PlayerIdleState(this, stateController, inputHandler) },
            { PlayerStates.Move, new PlayerMoveState(this, stateController, inputHandler) }
        };

        stateController.InitializeStates(stateDict);
    }
    void Update()
    {
        if (!IsGrounded())
        {
            Debug.Log("not grounded");
        }
        if(IsGrounded())
        {
            Debug.Log("grounded");
        }
        HandleGravity();


    }

    public void MovePlayer(Vector2 moveInput)
    {
        if (moveInput.sqrMagnitude < 0.01f) return;

        // Camera-relative direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;

        // Rotate player towards movement direction
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(move),
            10f * Time.deltaTime
        );

        // Move
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Gravity
        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void JumpPlayer()
    {
        if (characterController.isGrounded)
            velocity.y = jumpForce;
    }

    private void HandleGravity()
    {
        if (IsGrounded() && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        MovePlayer(new Vector2(0, velocity.y) * Time.deltaTime);
    }
    public bool IsGrounded() => characterController.isGrounded;
}
