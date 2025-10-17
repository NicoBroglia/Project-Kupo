using UnityEngine;

public class MoveState : PlayerBaseState
{
    private Vector2 moveInput;

    public MoveState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("ENTER Move State");
        animator.SetBool(hashIsMoving, true);

        // Load the persistent input when entering the state
        moveInput = controller.GetCurrentMoveInput();
    }

    public override void OnExit()
    {
        Debug.Log("EXIT Move State");
        animator.SetBool(hashIsMoving, false);
    }

    public override void OnUpdate()
    {
        // Move continuously every frame using the saved input
        controller.Motor.Move(moveInput);

        // Check grounded stat for falling
        if (!controller.Motor.IsGrounded())
            controller.SetState(LocomotionState.Fall);
    }

    // The Controller calls this for every move input change
    public override void OnInput(Vector2 input)
    {
        moveInput = input;

        // Handle transition to Idle when movement stops (input magnitude is zero)
        if (moveInput.sqrMagnitude < 0.01f)
        {
            controller.SetState(LocomotionState.Idle);
        }
    }

    // --- Transition Handlers ---

    // Only allow jump if grounded
    public override void HandleJumpAttempt()
    {
        if (motor.IsGrounded())
        {
            controller.SetState(LocomotionState.Jump);
        }
    }
}