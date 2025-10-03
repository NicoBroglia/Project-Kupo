using UnityEngine;

public class DashAbility : IAbility
{
    private readonly AnimationBridge animationBridge;
    private PlayerMotor motor;

    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.2f;

    public DashAbility(AnimationBridge animationBridge, PlayerMotor motor )
    {
        this.animationBridge = animationBridge;
        this.motor = motor;
    }

    public void Activate()
    {
        /*
        // Use the player's forward direction (horizontal only)
        Vector3 dashDir = motor.transform.forward;
        dashDir.y = 0f;

        if (dashDir.sqrMagnitude < 0.01f)
            dashDir = Vector3.forward;

        motor.Dash(dashDir, dashDistance, dashDuration);

        // optional: trigger dash animation via the bridge
        // animationBridge?.SetTrigger("Dash"); // implement in AnimationBridge if needed
        */
        Vector3 dashDir = motor.transform.forward;
        dashDir.y = 0f;

        if (dashDir.sqrMagnitude < 0.01f)
            dashDir = Vector3.forward;

        motor.Dash(dashDir, motor.DashDistance, motor.DashDuration);
    }
}
