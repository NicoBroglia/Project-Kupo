using UnityEngine;

public abstract class PlayerBaseState
{
    protected readonly PlayerStateController controller;
    protected readonly PlayerMotor motor;
    protected readonly Animator animator;

    // Animator hashes are pre-calculated for performance.
    protected static readonly int hashIsMoving = Animator.StringToHash("IsMoving");
    protected static readonly int hashIsJumping = Animator.StringToHash("IsJumping");
    protected static readonly int hashIsFalling = Animator.StringToHash("IsFalling");
    protected static readonly int hashIsDashing = Animator.StringToHash("IsDashing");

    protected PlayerBaseState(PlayerStateController controller)
    {
        this.controller = controller;
        this.motor = controller.Motor;
        this.animator = controller.Animator;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }

    // Event hooks for states to override
    public virtual void OnInput(Vector2 moveInput) { }
    public virtual void HandleJumpAttempt() { }
    public virtual void OnLanded() { }
    public virtual void OnDashEnded() { }
}
