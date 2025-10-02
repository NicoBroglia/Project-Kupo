using UnityEngine;

public class DashAbility : IAbility
{
    private Animator animator;
    private PlayerMotor motor;

    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.2f;

    public DashAbility(Animator animator, PlayerMotor motor)
    {
        this.animator = animator;
        this.motor = motor;
    }

    public void Activate()
    {
      if (animator != null)
            animator.SetTrigger("Dash");

        // Movimiento simple de dash en la dirección actual
        Vector3 dashDir = motor.LastMoveDirection.normalized;
        if (dashDir.sqrMagnitude < 0.01f)
            dashDir = Vector3.forward; // fallback si no hay input

        motor.Dash(dashDir, dashDistance, dashDuration);
    }
}
