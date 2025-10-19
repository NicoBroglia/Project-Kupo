using UnityEngine;

/// A StateMachineBehaviour to sync animation speed with the duration of a state.
/// Attach this to an animation state in the Animator Controller (e.g., the Dash animation).
public class StateSpeedMatcher : StateMachineBehaviour
{
    // This will be controlled by the state that initiates the animation.
    public static float DesiredDuration { get; set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (DesiredDuration > 0f)
        {
            // Calculate the multiplier needed to make the animation clip fit the desired duration.
            float speedMultiplier = stateInfo.length / DesiredDuration;
            animator.SetFloat("AnimationSpeedMultiplier", speedMultiplier);
        }
        else
        {
            // Default to normal speed if no duration is set.
            animator.SetFloat("AnimationSpeedMultiplier", 1f);
        }
    }
}
