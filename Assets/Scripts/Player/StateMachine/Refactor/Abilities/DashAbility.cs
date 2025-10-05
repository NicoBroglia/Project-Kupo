using UnityEngine;

public class DashAbility : IAbility
{
    private readonly AnimationBridge animationBridge;
    private PlayerMotor motor;

    public DashAbility(AnimationBridge animationBridge, PlayerMotor motor )
    {
        this.animationBridge = animationBridge;
        this.motor = motor;
    }

    public void Activate()
    {
        Vector3 dashDir = motor.transform.forward;
        dashDir.y = 0f;

        if (dashDir.sqrMagnitude < 0.01f)
            dashDir = Vector3.forward;

        motor.Dash(dashDir, motor.DashDistance, motor.DashDuration);
        animationBridge?.PlayDash(); // play dash animation
    }
}
