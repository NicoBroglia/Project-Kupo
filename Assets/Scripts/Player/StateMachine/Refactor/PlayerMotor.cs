using System.Collections;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float DashDistance => dashDistance;
    public float DashDuration => dashDuration;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f; // Added rotation speed here
    [SerializeField] private Transform cameraTransform;

    [Header("Gravity & Jump")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 2f;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 1f;

    private CharacterController controller;
    private AnimationBridge animationBridge;
    private PlayerInputHandler playerInputHandler;

    private Vector3 velocity;
    private Vector3 dashVelocity;
    private bool isDashing;
    private float dashEndTime;
    
    public Vector3 LastMoveDirection { get; private set; } = Vector3.zero;
    public bool IsGrounded => controller.isGrounded;


    private void Awake()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();

        //null checks
        if (animationBridge == null)
        {
            animationBridge = GetComponent<AnimationBridge>();
            if (animationBridge == null)
            {
                Debug.LogError("Animator component not found on " + gameObject.name);
            }
        }
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
            if (controller == null)
            {
                Debug.LogError("CharacterController component not found on " + gameObject.name);
            }
        }
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

    }
    void Update()
    {   
        ApplyGravity();
        animationBridge.SetMoveSpeed(LastMoveDirection.magnitude);
    }
    private void ApplyGravity()
    {
        if (isDashing) return; //ignore gravity during dash

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f; // small downward force keeps grounded
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(Vector2 input)
    {

        // Get camera directions on the horizontal plane
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Convert input to world direction
        Vector3 moveDir = camForward * input.y + camRight * input.x;
        LastMoveDirection = moveDir;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            // Apply movement using CharacterController
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            // Rotate player smoothly toward movement direction
            RotateTowards(moveDir);
        }
    }
    private void RotateTowards(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Dash(Vector3 direction, float distance, float duration)
    {
        if (!isDashing)
        {
            StartCoroutine(DashCoroutine(direction.normalized, distance, duration));
        }
      
    }

    private IEnumerator DashCoroutine(Vector3 dir, float distance, float duration)
    {
        playerInputHandler.enabled = false;
        isDashing = true;

        float elapsed = 0f;
        float speed = distance / duration;

        while (elapsed < duration)
        {
            controller.Move(dir * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        playerInputHandler.enabled = true;
    }
   

    public void Stop()
    {
        isDashing = false;
        velocity = Vector3.zero;
        dashVelocity = Vector3.zero;
        animationBridge?.SetMoveSpeed(0f);
    }
}
