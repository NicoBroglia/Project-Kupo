using UnityEngine;

public class FallState : PlayerBaseState
{
    public FallState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("ENTER Fall State");
        animator.SetBool(hashIsFalling, true);
        animator.SetBool(hashIsJumping, false);
        animator.SetBool(hashIsMoving, false);
    }

    public override void OnUpdate()
    {
        // The PlayerMotor's Update loop already handles applying gravity.
        // The only job of the FallState's Update is to check for a landing.
        if (motor.IsGrounded())
        {
            controller.SetState(LocomotionState.Idle);
        }
    }

    public override void OnLanded()
    {
        // An alternative to checking in Update is using the OnLanded event.
        // This is often cleaner.
        controller.SetState(LocomotionState.Idle);
    }

    public override void OnExit()
    {
        Debug.Log("EXIT Fall State");
        animator.SetBool(hashIsFalling, false);
    }
}
