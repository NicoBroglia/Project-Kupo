using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetIdle(bool value) => anim.SetBool("isIdle", value);
    public void SetMoving(bool value) => anim.SetBool("isMoving", value);
    public void SetJump() => anim.SetTrigger("jump");
}
