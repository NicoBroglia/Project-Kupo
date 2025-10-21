using UnityEngine;

[RequireComponent(typeof(PlayerStateController))]
[RequireComponent(typeof(StaminaController))]
[RequireComponent(typeof(PlayerMotor))]
public class ActionController : MonoBehaviour
{
    [Header("Component References")]
    private PlayerStateController _stateController;
    private StaminaController _staminaController;
    private PlayerMotor _playerMotor;

    [Header("Dash Action")]
    [SerializeField] private float dashStaminaCost = 20f;
    [SerializeField] private float dashCooldown = 1.0f;
    private float _dashCooldownTimer;

    private void Awake()
    {
        _stateController = GetComponent<PlayerStateController>();
        _staminaController = GetComponent<StaminaController>();
        _playerMotor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        DashCooldownTimer();
    }

    #region DASH
    private void DashCooldownTimer()
    {
        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }
    }

    public void TryPerformDash()
    {
        if (!(_stateController.CurrentState is GroundedState)) return;
        if (_dashCooldownTimer > 0f) return;

        if (_staminaController.TryConsumeStamina(dashStaminaCost))
        {
            _stateController.SetState(LocomotionState.Dash);
            _dashCooldownTimer = dashCooldown;
        }
        else
        {
            Debug.Log("Dash Failed: Not enough stamina!");
        }

        #endregion DASH

        // When we add attacking, we will add a method here:
        // public void TryPerformAttack() { ... }
    }
}