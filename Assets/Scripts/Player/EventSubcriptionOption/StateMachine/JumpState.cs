using UnityEngine;

public class JumpState : PlayerBaseState
{
    public JumpState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        // Stop movement animation while jumping
        animator.SetBool(hashIsMoving, false);

        // Set jumping animation
        animator.SetBool(hashIsJumping, true);
        animator.SetBool(hashIsFalling, false);

        motor.Jump();
    }

    public override void OnUpdate()
    {
        // Once the player leaves the ground, transition to FallState
        if (!motor.IsGrounded())
        {
            controller.SetState(LocomotionState.Fall);
        }
    }

    public override void OnExit()
    {
        // Clear the jumping flag when leaving jump state
        animator.SetBool(hashIsJumping, false);
    }
}
