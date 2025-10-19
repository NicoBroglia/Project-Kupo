using UnityEngine;

public class MoveState : GroundedState // Inherits from GroundedState
{
    public MoveState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        animator.SetBool(hashIsMoving, true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate(); // Call base to check for falling
        motor.Move(controller.GetCurrentMoveInput());
    }

    public override void OnInput(Vector2 moveInput)
    {
        if (moveInput.sqrMagnitude < 0.01f)
        {
            controller.SetState(LocomotionState.Idle);
        }
    }

    public override void OnExit()
    {
        animator.SetBool(hashIsMoving, false);
    }
}
