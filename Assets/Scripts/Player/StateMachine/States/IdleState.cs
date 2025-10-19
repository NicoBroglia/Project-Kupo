using UnityEngine;

public class IdleState : GroundedState // Inherits from GroundedState
{
    public IdleState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        animator.SetBool(hashIsMoving, false);
    }

    public override void OnInput(Vector2 moveInput)
    {
        // Transition to MoveState if there is input.
        if (moveInput.sqrMagnitude > 0.01f)
            controller.SetState(LocomotionState.Move);
    }
}
