using UnityEngine;

[RequireComponent(typeof(PlayerStateController))]
public class ActionController : MonoBehaviour
{
    private PlayerStateController _stateController;
    private StaminaController _staminaController;
    private PlayerMotor _playerMotor;

    [Header("Dash Action")]
    [SerializeField] private float dashStaminaCost = 20f;
    [SerializeField] private float dashCooldown = 1.0f;
    private float _dashCooldownTimer;

    private void Awake()
    {
        if (_stateController == null)
        {
            _stateController = GetComponent<PlayerStateController>();
        }
        if (_staminaController == null)
        {
            _staminaController = GetComponent<StaminaController>();
        }
        if (_playerMotor == null)
        {
            _playerMotor = GetComponent<PlayerMotor>();
        }
        else
        {
            Debug.LogWarning("Required component is missing on the GameObject!", this);
        }
    }

    private void Update()
    {
        DashCooldownTimer();
    }

    #region DASH
    private void DashCooldownTimer()
    {
        // Tick down the cooldown timer
        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }
    }

    public void TryPerformDash()
    {

        // 1. Check if we are already dashing
        if (_stateController.CurrentState is DashState) return;

        // 2. Check if grounded
        if (!_playerMotor.IsGrounded()) return;

        // 3. Check if the dash is on cooldown
        if (_dashCooldownTimer > 0f) return;

        // 4. Check for stamina
        if (_staminaController.TryConsumeStamina(dashStaminaCost))
        {
            _stateController.SetState(LocomotionState.Dash);
            // 5. Start the cooldown
            _dashCooldownTimer = dashCooldown;
        }
        else
        {
            Debug.Log("Dash Failed: Not enough stamina!");
        }
    }

    #endregion DASH

    // When we add attacking, we will add a method here:
    // public void TryPerformAttack() { ... }
}