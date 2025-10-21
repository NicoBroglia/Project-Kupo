using UnityEngine;

public class JumpState : PlayerBaseState
{
    public JumpState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        animator.SetBool(hashIsJumping, true);
        motor.Jump();
        // The transition to FallState is now implicitly handled by the PlayerStateController's
        // OnAirborne event, making this state cleaner.
    }

    public override void OnExit()
    {
        animator.SetBool(hashIsJumping, false);
    }
}
