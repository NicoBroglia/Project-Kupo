using UnityEngine;
using System;

[RequireComponent(typeof(StatController))]
public class StaminaController : MonoBehaviour
{
    public float CurrentStamina { get; private set; }
    public float MaxStamina { get; private set; }

    // Event to notify UI or other systems of stamina changes.
    public event Action<float, float> OnStaminaChanged = delegate { };

    [Header("Configuration")]
    [SerializeField] private StatDefinition enduranceStat;
    [SerializeField] private float staminaRegenRate = 20f;
    [SerializeField] private float staminaRegenDelay = 1.5f;

    private StatController _statController;
    private CharacterStat _endurance;
    private float _staminaRegenTimer;

    private void Awake()
    {
        _statController = GetComponent<StatController>();
    }

    private void Start()
    {
        _endurance = _statController.GetStat(enduranceStat);
        if (_endurance == null)
        {
            Debug.LogError("Endurance StatDefinition not found on StatController!", this);
            // Assign a default to prevent errors.
            MaxStamina = 100;
        }
        else
        {
            CalculateMaxStamina();
        }

        CurrentStamina = MaxStamina;
        OnStaminaChanged(CurrentStamina, MaxStamina);
    }

    private void Update()
    {
        if (_staminaRegenTimer <= 0f)
        {
            RegenerateStamina();
        }
        else
        {
            _staminaRegenTimer -= Time.deltaTime;
        }
    }

    private void RegenerateStamina()
    {
        if (CurrentStamina < MaxStamina)
        {
            CurrentStamina += staminaRegenRate * Time.deltaTime;
            CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
            OnStaminaChanged(CurrentStamina, MaxStamina);
        }
    }

    public bool TryConsumeStamina(float amount)
    {
        if (CurrentStamina >= amount)
        {
            CurrentStamina -= amount;
            _staminaRegenTimer = staminaRegenDelay;
            OnStaminaChanged(CurrentStamina, MaxStamina);
            return true;
        }
        return false;
    }

    private void CalculateMaxStamina()
    {
        // Example formula: 100 base stamina + 10 for each point of endurance.
        MaxStamina = 100 + (_endurance.Value * 10f);
    }
}
