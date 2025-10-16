using UnityEngine;

public abstract class PlayerBaseState
{
    protected readonly PlayerStateController controller;
    protected readonly PlayerMotor motor;
    protected readonly Animator animator;

    // Animator hashes for performance
    protected readonly int hashIsMoving = Animator.StringToHash("IsMoving");
    protected readonly int hashIsJumping = Animator.StringToHash("IsJumping");
    protected readonly int hashIsFalling = Animator.StringToHash("IsFalling");
    protected readonly int hashIsDashing = Animator.StringToHash("IsDashing");
    protected readonly int hashDashSpeedMultiplier = Animator.StringToHash("DashSpeedMultiplier");

    protected PlayerBaseState(PlayerStateController controller)
    {
        this.controller = controller;
        this.motor = controller.Motor;
        this.animator = controller.Animator;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }

    // --- Event Hooks for States to Implement ---
    public virtual void OnInput(Vector2 moveInput) { }
    public virtual void HandleJumpAttempt() { }
    public virtual void HandleDashAttempt() { }
    public virtual void OnLanded() { }

    // New hook for the dash ending
    public virtual void OnDashEnded() { }
}
