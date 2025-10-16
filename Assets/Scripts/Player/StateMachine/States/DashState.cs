using UnityEngine;

public class DashState : PlayerBaseState
{
    private float _dashClipLength = -1f; // Use -1 to indicate it hasn't been found yet
    public DashState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("ENTER Dash State");

        // Find the animation clip length the first time we enter
        if (_dashClipLength < 0f)
        {
            // IMPORTANT: Replace "Player_Dash" with the EXACT name of your dash animation clip.
            _dashClipLength = GetClipLength("Dash");
        }

        // Calculate and set the speed multiplier
        float duration = motor.DashDuration;
        if (duration > 0f && _dashClipLength > 0f)
        {
            float speedMultiplier = _dashClipLength / duration;
            animator.SetFloat(hashDashSpeedMultiplier, speedMultiplier);
        }
        else
        {
            // Default to 1 if duration is invalid to prevent errors
            animator.SetFloat(hashDashSpeedMultiplier, 1f);
        }

        animator.SetBool(hashIsDashing, true);
        motor.Dash();
    }


    private float GetClipLength(string clipName)
    {
        var clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        Debug.LogError($"Animation clip '{clipName}' not found!");
        return -1f; // Return -1 if not found
    }


    // Input is ignored during the dash state.
    public override void OnInput(Vector2 moveInput) { }
    public override void HandleJumpAttempt() { }

    // The dash action cannot be re-triggered from within itself.
    public override void HandleDashAttempt() { }

    public override void OnDashEnded()
    {
        // Once the dash is complete, check for persistent movement input
        // to decide whether to transition to Idle or Move.
        if (motor.IsGrounded())
        {
            if (controller.GetCurrentMoveInput().sqrMagnitude > 0.01f)
            {
                controller.SetState(LocomotionState.Move);
            }
            else
            {
                controller.SetState(LocomotionState.Idle);
            }
        }
        else
        {
            controller.SetState(LocomotionState.Fall);
        }
    }

    public override void OnExit()
    {
        Debug.Log("EXIT Dash State");
        animator.SetBool(hashIsDashing, false);
    }
}
