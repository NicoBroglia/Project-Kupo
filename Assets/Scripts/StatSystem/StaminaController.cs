using UnityEngine;

[RequireComponent(typeof(StatController))]
public class StaminaController : MonoBehaviour
{
    public float CurrentStamina { get; private set; }
    public float MaxStamina { get; private set; }

    [Header("Configuration")]
    [Tooltip("Drag your 'Endurance' StatDefinition Scriptable Object here.")]
    [SerializeField] private StatDefinition enduranceStat;
    [Tooltip("The base stamina a player has at 0 endurance.")]
    [SerializeField] private float baseStamina = 80f;
    [Tooltip("How much stamina is gained per point of endurance.")]
    [SerializeField] private float staminaPerEndurance = 4f;
    [SerializeField] private float staminaRegenRate = 25f;
    [SerializeField] private float staminaRegenDelay = 1.5f;

    private StatController _statController;
    private float _staminaRegenTimer;

    private void Awake()
    {
        _statController = GetComponent<StatController>();
    }

    private void Start()
    {
        // Calculate initial max stamina and set current stamina.
        CalculateMaxStamina();
        CurrentStamina = MaxStamina;
    }

    private void Update()
    {
        // Max stamina can change if endurance is buffed/debuffed, so we check each frame.
        CalculateMaxStamina();

        // Handle regeneration
        if (_staminaRegenTimer <= 0f)
        {
            if (CurrentStamina < MaxStamina)
            {
                CurrentStamina += staminaRegenRate * Time.deltaTime;
                CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
            }
        }
        else
        {
            _staminaRegenTimer -= Time.deltaTime;
        }
    }

    public bool TryConsumeStamina(float amount)
    {
        if (CurrentStamina >= amount)
        {
            CurrentStamina -= amount;
            _staminaRegenTimer = staminaRegenDelay;
            return true;
        }
        return false;
    }

    private void CalculateMaxStamina()
    {
        CharacterStat endurance = _statController.GetStat(enduranceStat);
        if (endurance != null)
        {
            MaxStamina = baseStamina + (endurance.Value * staminaPerEndurance);
        }
        else
        {
            Debug.LogWarning("Endurance StatDefinition not assigned on the StaminaController!", this);
            MaxStamina = baseStamina;
        }

        // Ensure current stamina doesn't exceed the new max stamina.
        if (CurrentStamina > MaxStamina)
        {
            CurrentStamina = MaxStamina;
        }
    }
}