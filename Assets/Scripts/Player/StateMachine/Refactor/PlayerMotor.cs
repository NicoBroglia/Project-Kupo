using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f; // Added rotation speed here
    public Vector3 LastMoveDirection { get; private set; }

    private bool isDashing;
    private float dashEndTime;
    private Vector3 dashVelocity;

    private AnimationBridge animBridge;

    private void Awake()
    {
        animBridge = GetComponent<AnimationBridge>();
        if (animBridge == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    public void Move(Vector2 input)
    {
        if (isDashing) return; // block input while dashing

        Vector3 dir = new Vector3(input.x, 0, input.y);
        LastMoveDirection = dir;

        if (dir.sqrMagnitude > 0.01f)
        {
            // Move player
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;

            // Rotate player smoothly towards movement direction
            RotateTowards(dir);
        }
        float speed = dir.magnitude * moveSpeed; // velocidad real
        animBridge?.SetMoveSpeed(speed);
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Dash(Vector3 direction, float distance, float duration)
    {
        isDashing = true;
        dashEndTime = Time.time + duration;
        dashVelocity = direction.normalized * (distance / duration);
    }

    void Update()
    {
        animBridge.SetMoveSpeed(LastMoveDirection.magnitude);

        if (isDashing)
        {
            if (Time.time < dashEndTime)
            {
                transform.position += dashVelocity * Time.deltaTime;
            }
            else
            {
                isDashing = false;
            }
        }
    }

    public void Stop()
    {
        isDashing = false;
        dashVelocity = Vector3.zero;
        animBridge?.SetMoveSpeed(0f);
    }
}
