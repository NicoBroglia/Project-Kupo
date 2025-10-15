using UnityEngine;

public abstract class PlayerBaseState
{
    protected readonly PlayerStateController controller;
    protected readonly PlayerMotor motor;
    protected readonly Animator animator;

    // protected so child can access, make as hash because Unity is case-sensitive and a typo can cause bugs
    protected readonly int hashIsJumping = Animator.StringToHash("IsJumping");
    protected readonly int hashIsFalling = Animator.StringToHash("IsFalling");
    protected readonly int hashIsMoving = Animator.StringToHash("IsMoving");

    protected PlayerBaseState(PlayerStateController controller)
    {
        this.controller = controller;
        this.motor = controller.Motor;
        this.animator = controller.Animator;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
    public virtual void OnInput(Vector2 moveInput) { }
}
