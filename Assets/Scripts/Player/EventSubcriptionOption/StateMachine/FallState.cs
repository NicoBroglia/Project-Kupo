using UnityEngine;

public class FallState : PlayerBaseState
{
    public FallState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        // Set falling animation
        animator.SetBool(hashIsFalling, true);
        animator.SetBool(hashIsJumping, false);

        // Stop movement animation while in air
        animator.SetBool(hashIsMoving, false);
    }

    public override void OnUpdate()
    {
        motor.ApplyGravity();

        if (motor.IsGrounded())
            controller.SetState(LocomotionState.Idle);
    }

    public override void OnExit()
    {
        animator.SetBool(hashIsFalling, false);
    }
}
