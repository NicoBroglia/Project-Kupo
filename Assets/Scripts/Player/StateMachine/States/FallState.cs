using UnityEngine;

public class FallState : PlayerBaseState
{
    public FallState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        animator.SetBool(hashIsFalling, true);
        animator.SetBool(hashIsJumping, false); // Ensure jump is reset
    }

    public override void OnUpdate()
    {
        // Allow for air control by applying movement from input.
        motor.Move(controller.GetCurrentMoveInput());
    }

    public override void OnLanded()
    {
        // On landing, transition to idle or move based on player input.
        controller.SetState(controller.GetCurrentMoveInput().sqrMagnitude > 0.01f
            ? LocomotionState.Move
            : LocomotionState.Idle);
    }

    public override void OnExit()
    {
        animator.SetBool(hashIsFalling, false);
    }
}
