using UnityEngine;

/// An abstract base state for any state where the player is on the ground (Idle, Move).
/// This helps to reduce code duplication for common grounded actions like jumping.
public abstract class GroundedState : PlayerBaseState
{
    protected GroundedState(PlayerStateController controller) : base(controller) { }

    public override void HandleJumpAttempt()
    {
        if (motor.IsGrounded())
        {
            controller.SetState(LocomotionState.Jump);
        }
    }

    public override void OnUpdate()
    {
        // A grounded state should always check if it has become un-grounded.
        if (!motor.IsGrounded())
        {
            controller.SetState(LocomotionState.Fall);
        }
    }
}
