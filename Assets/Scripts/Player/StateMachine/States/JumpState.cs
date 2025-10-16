using UnityEngine;

public class JumpState : PlayerBaseState
{
    public JumpState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("ENTER Jump State");
        animator.SetBool(hashIsMoving, false);
        animator.SetBool(hashIsJumping, true);
        animator.SetBool(hashIsFalling, false);

        motor.Jump();
    }

    // OnUpdate is no longer needed. The global OnAirborne event in the
    // PlayerStateController will handle the transition to the FallState,
    // which is more reliable and centralizes the logic.

    public override void OnExit()
    {
        Debug.Log("EXIT Jump State");
        animator.SetBool(hashIsJumping, false);
    }
}
