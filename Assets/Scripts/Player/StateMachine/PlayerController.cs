using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController cc;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravity = -9.81f;

    Vector3 velocity;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            // Camera-relative movement
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * direction.z + camRight * direction.x;

            cc.Move(moveDir * moveSpeed * Time.deltaTime);

            // Face movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 10f * Time.deltaTime);
        }
    }

    public void ApplyGravity()
    {
        if (cc.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small push downward to stay grounded
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (cc.isGrounded)
        {
            velocity.y = jumpForce;
        }
    }

    public bool IsGrounded() => cc.isGrounded;
}
