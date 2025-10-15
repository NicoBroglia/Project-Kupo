using UnityEngine;

public class MoveState : PlayerBaseState
{
    private Vector2 moveInput;

    public MoveState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        animator.SetBool(hashIsMoving, true);
        controller.Input.OnMove += OnMoveInput;
        controller.Input.OnMoveCanceled += OnMoveCanceled;
    }

    public override void OnExit()
    {
        animator.SetBool(hashIsMoving, false);
        controller.Input.OnMove -= OnMoveInput;
        controller.Input.OnMoveCanceled -= OnMoveCanceled;
    }

    public override void OnUpdate()
    {
       
        // Move continuously every frame
        controller.Motor.Move(moveInput);

        // Check grounded stat for falling
        if (!controller.Motor.IsGrounded())
            controller.SetState(LocomotionState.Fall);
        
    }

    private void OnMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void OnMoveCanceled()
    {
        moveInput = Vector2.zero;
        controller.SetState(LocomotionState.Idle);
    }
}
