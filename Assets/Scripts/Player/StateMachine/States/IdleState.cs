using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("ENTER Idle State");
        animator.SetBool(hashIsMoving, false);
    }
    public override void OnExit()
    {
        Debug.Log("EXIT Idle State");
    }

    public override void OnInput(Vector2 moveInput)
    {
        if (moveInput.sqrMagnitude > 0.01f)
            controller.SetState(LocomotionState.Move);
    }

    // --- Transition Handlers ---

    // A jump is always valid from the Idle state if grounded
    public override void HandleJumpAttempt()
    {
        if (motor.IsGrounded())
        {
            controller.SetState(LocomotionState.Jump);
        }
    }
}
