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

        // Simple forward movment for dash
        Vector3 dashDir = motor.LastMoveDirection.normalized;
        if (dashDir.sqrMagnitude < 0.01f)
            dashDir = Vector3.forward; // fallback if no input

        motor.Dash(dashDir, dashDistance, dashDuration);
    }
}
