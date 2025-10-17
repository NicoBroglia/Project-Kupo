using UnityEngine;

[RequireComponent(typeof(PlayerStateController), typeof(StaminaController))]
public class ActionController : MonoBehaviour
{
    private PlayerStateController _stateController;
    private StaminaController _staminaController;

    [Header("Dash Action")]
    [SerializeField] private float dashStaminaCost = 20f;
    [SerializeField] private float dashCooldown = 1.0f;
    private float _dashCooldownTimer;


    private void Awake()
    {
        _stateController = GetComponent<PlayerStateController>();
        _staminaController = GetComponent<StaminaController>();
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

        // 2. Check if the dash is on cooldown
        if (_dashCooldownTimer > 0f) return;

        // 3. Check for stamina
        if (_staminaController.TryConsumeStamina(dashStaminaCost))
        {
            _stateController.SetState(LocomotionState.Dash);
            // 4. Start the cooldown
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