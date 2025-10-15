using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        animator.SetBool(hashIsMoving, false);
    }

    public override void OnInput(Vector2 moveInput)
    {
        if (moveInput.sqrMagnitude > 0.01f)
            controller.SetState(LocomotionState.Move);
    }
}
