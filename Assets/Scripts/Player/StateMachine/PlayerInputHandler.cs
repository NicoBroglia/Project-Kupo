using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 GetMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return new Vector3(h, 0, v).normalized;
    }

    public bool JumpPressed() => Input.GetButtonDown("Jump");
    public bool AttackPressed() => Input.GetButtonDown("Fire1");
}
