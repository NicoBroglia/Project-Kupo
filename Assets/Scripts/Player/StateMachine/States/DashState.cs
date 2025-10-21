using UnityEngine;

public class DashState : PlayerBaseState
{
    // A private field to cache the length of the dash animation clip.
    // We initialize to -1 to signify that we haven't found it yet.
    private float _dashClipLength = -1f;
    // Pre-calculate the hash for the animator parameter for efficiency.
    private readonly int hashDashSpeedMultiplier = Animator.StringToHash("DashSpeedMultiplier");

    public DashState(PlayerStateController controller) : base(controller) { }

    public override void OnEnter()
    {
        animator.SetBool(hashIsDashing, true);

        // We now pass the current movement input from the controller to the motor's Dash method.
        motor.Dash(controller.GetCurrentMoveInput());

        // --- NEW LOGIC TO SYNC ANIMATION SPEED ---
        // Find the clip length once and cache it for future use.
        if (_dashClipLength < 0f)
        {
            // Note: This assumes your animation clip is named "Dash".
            // If it has a different name, change it here.
            _dashClipLength = GetClipLength("Dash");
        }

        // Calculate the animation speed multiplier to match the dash duration.
        // Formula: Animation Length / Desired Duration = Required Speed
        float duration = motor.DashDuration;
        if (duration > 0f && _dashClipLength > 0f)
        {
            float speedMultiplier = _dashClipLength / duration;
            animator.SetFloat(hashDashSpeedMultiplier, speedMultiplier);
        }
        else
        {
            // If something goes wrong, default to a normal speed of 1.
            animator.SetFloat(hashDashSpeedMultiplier, 1f);
        }
    }

    /// <summary>
    /// Searches the Animator Controller for a clip with the given name and returns its length.
    /// </summary>
    private float GetClipLength(string clipName)
    {
        // Get all clips from the animator controller.
        var clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == clipName)
            {
                // Found it, return the length and exit.
                return clip.length;
            }
        }
        // If we get here, the clip was not found.
        Debug.LogWarning($"Animation clip '{clipName}' not found in the Animator Controller!");
        return -1f; // Return -1 to indicate an error.
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
