using UnityEngine;

public class DashState : PlayerBaseState
{
    public DashState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        animator.SetBool(hashIsDashing, true);

        // pass the current movement input from the controller to the motor's Dash method.
        motor.Dash(controller.GetCurrentMoveInput());
    }

    public override void OnDashEnded()
    {
        // Transition to a grounded or falling state based on conditions.
        if (motor.IsGrounded())
        {
            controller.SetState(controller.GetCurrentMoveInput().sqrMagnitude > 0.01f
                ? LocomotionState.Move
                : LocomotionState.Idle);
        }
        else
        {
            controller.SetState(LocomotionState.Fall);
        }
    }

    public override void OnExit()
    {
        animator.SetBool(hashIsDashing, false);
    }
}
